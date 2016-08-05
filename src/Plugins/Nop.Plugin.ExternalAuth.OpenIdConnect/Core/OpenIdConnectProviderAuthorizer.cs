using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Stores;
using Nop.Core;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nop.Core.Domain.Customers;
using System.Net;
using System.IO;

namespace Nop.Plugin.ExternalAuth.OpenIdConnect.Core
{
    public class OpenIdConnectProviderAuthorizer : IOAuth2ProviderOpenIdConnectAuthorizer
    {
        private IExternalAuthorizer _authorizer;
        private ExternalAuthenticationSettings _externalAuthenticationSettings;
        private OpenIdConnectExternalAuthSettings _oidcExternalAuthSettings;
        private HttpContextBase _httpContext;
        private IWebHelper _webHelper;

        public OpenIdConnectProviderAuthorizer(
            IExternalAuthorizer authorizer,
            ExternalAuthenticationSettings externalAuthenticationSettings,
            OpenIdConnectExternalAuthSettings oidcExternalAuthSettings,
            HttpContextBase httpContext,
            IWebHelper webHelper
            )
        {
            _authorizer = authorizer;
            _externalAuthenticationSettings = externalAuthenticationSettings;
            _oidcExternalAuthSettings = oidcExternalAuthSettings;
            _httpContext = httpContext;
            _webHelper = webHelper;
        }


        public AuthorizeState Authorize(string returnUrl, bool? verifyResponse = default(bool?))
        {
            var code = _httpContext.Request.QueryString["code"];
            var state = _httpContext.Request.QueryString["state"];

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
            {
                return new AuthorizeState(returnUrl, OpenAuthenticationStatus.Error);
            }

            HttpRequestMessage requestMessage = CreateOidcRequestMessage("connect/token", code: code);

            using (var client = new HttpClient())
            {
                var response = client.SendAsync(requestMessage).Result;

                if (response.IsSuccessStatusCode)
                {
                    var oauth = OAuthTokenResponse.Success(JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result));

                    var parameters = new OpenIdConnectAuthenticationParameters(Provider.SystemName)
                    {
                        //ExternalIdentifier = oauth.ProviderUserId,
                        //OAuthToken =
                        OAuthAccessToken = oauth.AccessToken
                    };

                    if (_externalAuthenticationSettings.AutoRegisterEnabled)
                        ParseClaims(oauth, parameters);

                    var result = _authorizer.Authorize(parameters);

                    return new AuthorizeState(returnUrl, result);
                }
                else
                {
                    return new AuthorizeState(returnUrl, OpenAuthenticationStatus.Error);
                }
            }
        }

        private HttpRequestMessage CreateOidcRequestMessage(string path, string code = null, string accesstoken = null)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_oidcExternalAuthSettings.OpenIdConnectServer}{path}");
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Headers.TryAddWithoutValidation("Content-Type", "application/json");

            if (!string.IsNullOrEmpty(code)) {
                var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "client_id", _oidcExternalAuthSettings.ClientId },
                { "redirect_uri", _oidcExternalAuthSettings.RedirectUrl }, //"http://localhost:15536/Plugins/ExternalAuthOpenIdConnect/LoginCallback" },
                { "code", code },
                { "client_secret", _oidcExternalAuthSettings.ClientSecret },
                { "grant_type", "authorization_code" },
            };

                requestMessage.Content = new FormUrlEncodedContent(tokenRequestParameters);
            }else if(!string.IsNullOrEmpty(accesstoken))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
                requestMessage.Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>());
            }
            else
            {
                throw new Exception("code or access_token not specified");
            }
            
            return requestMessage;
        }

        private void ParseClaims(OAuthTokenResponse authenticationResult, OpenIdConnectAuthenticationParameters parameters)
        {
            var request = CreateOidcRequestMessage("connect/userinfo", accesstoken: authenticationResult.AccessToken);
            JObject userProfile;
            using (var client = new HttpClient())
            {
                var result = client.SendAsync(request).Result;
                userProfile = JsonConvert.DeserializeObject<JObject>(result.Content.ReadAsStringAsync().Result);
            }

            var claims = new UserClaims();
            parameters.ExternalIdentifier = userProfile.Value<string>("sub");
            claims.Contact = new ContactClaims();
            claims.Contact.Email = userProfile.Value<string>("email");
            claims.Name = new NameClaims();
            claims.Name.First = userProfile.Value<string>("given_name");
            claims.Name.Last = userProfile.Value<string>("family_name");

            parameters.AddClaim(claims);
        }

        private string RequestUserProdile(string accessToken)
        {
            var request = WebRequest.Create("https://graph.facebook.com/me?fields=email&access_token=" + accessToken);
            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        var responseFromServer = reader.ReadToEnd();
                        var userInfo = JObject.Parse(responseFromServer);
                        if (userInfo["email"] != null)
                        {
                            return userInfo["email"].ToString();
                        }
                    }
                }
            }

            return string.Empty;
        }

        public class OAuthTokenResponse
        {
            private OAuthTokenResponse(JObject response)
            {
                Response = response;
                AccessToken = response.Value<string>("access_token");
                TokenType = response.Value<string>("token_type");
                RefreshToken = response.Value<string>("refresh_token");
                ExpiresIn = response.Value<string>("expires_in");
            }

            private OAuthTokenResponse(Exception error)
            {
                Error = error;
            }

            public static OAuthTokenResponse Success(JObject response)
            {
                return new OAuthTokenResponse(response);
            }

            public static OAuthTokenResponse Failed(Exception error)
            {
                return new OAuthTokenResponse(error);
            }

            public JObject Response { get; set; }
            public string AccessToken { get; set; }
            public string TokenType { get; set; }
            public string RefreshToken { get; set; }
            public string ExpiresIn { get; set; }
            public Exception Error { get; set; }
        }
    }
}

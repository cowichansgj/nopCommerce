using Nop.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Commissions.ProductExtension.Models
{
    public class ProductExtensionsModel
    {
        [NopResourceDisplayName("Plugins.Commissions.ProductExtension.PersonalVolume")]
        public decimal PersonalVolume { get; set; }

        [NopResourceDisplayName("Plugins.Commissions.ProductExtension.GroupVolume")]
        public decimal GroupVolume { get; set; }
        [NopResourceDisplayName("Plugins.Commissions.ProductExtension.AdditionalVolume")]
        public decimal AdditionalVolume { get; set; }

        public int ProductId { get; set; }
    }
}

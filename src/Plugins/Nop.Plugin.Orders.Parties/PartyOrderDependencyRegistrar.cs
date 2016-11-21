using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Orders.Parties.Data;
using Nop.Plugin.Orders.Parties.Domain;
using Nop.Plugin.Orders.Parties.Services;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Orders.Parties
{
    public class PartyOrderDependencyRegistrar : IDependencyRegistrar
    {
        private const string CONTEXT_NAME = "nop_object_context_party_orders";

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<PartyOrderService>().As<IPartyOrderService>().InstancePerLifetimeScope();

            //data context
            this.RegisterPluginDataContext<PartyOrderObjectContext>(builder, CONTEXT_NAME);

            //override required repository with our custom context
            builder.RegisterType<EfRepository<PartyOrder>>()
                .As<IRepository<PartyOrder>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
        }

        public int Order { get { return 1; } }
    }
}

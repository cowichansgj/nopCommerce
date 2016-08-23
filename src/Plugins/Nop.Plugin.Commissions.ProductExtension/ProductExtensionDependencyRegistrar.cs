using Nop.Core.Infrastructure.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Autofac;
using Nop.Plugin.Commissions.ProductExtension.Services;
using Nop.Web.Framework.Mvc;
using Nop.Plugin.Commissions.ProductExtension.Data;
using Nop.Plugin.Commissions.ProductExtension.Domain;
using Nop.Data;
using Nop.Core.Data;
using Autofac.Core;

namespace Nop.Plugin.Commissions.ProductExtension
{
    public class ProductExtensionDependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get
            {
                return 0;
            }
        }

        private const string ContextName = "nop_object_context_commissions_product_extensions";

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<ProductCommissionService>()
                .As<IProductCommissionService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderCommissionService>()
                .As<IOrderCommissionService>()
                .InstancePerLifetimeScope();

            this.RegisterPluginDataContext<CommissionsProductExtensionContext>(builder, ContextName);

            builder.RegisterType<EfRepository<ProductCommission>>()
                .As<IRepository<ProductCommission>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<OrderCommission>>()
                .As<IRepository<OrderCommission>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<OrderItemCommission>>()
                .As<IRepository<OrderItemCommission>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(ContextName))
                .InstancePerLifetimeScope();



        }
    }
}

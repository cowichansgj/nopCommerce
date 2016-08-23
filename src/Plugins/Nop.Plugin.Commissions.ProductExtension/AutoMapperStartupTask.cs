using AutoMapper;
using Nop.Core.Infrastructure;
using Nop.Plugin.Commissions.ProductExtension.Domain;
using Nop.Plugin.Commissions.ProductExtension.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Commissions.ProductExtension
{
    public class AutoMapperStartupTask : IStartupTask
    {
        public int Order
        {
            get
            {
                return 0;
            }
        }

        public void Execute()
        {
            Mapper.Initialize(c =>
            {
                c.AddProfile(new CommissionProductExtensionProfile());
            });
        }
    }

    public class CommissionProductExtensionProfile : Profile
    {
        [Obsolete]
        protected override void Configure()
        {
            CreateMap<ProductCommission, ProductExtensionsModel>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId));
            
        }
    }

}

using Nop.Data.Mapping;
using Nop.Plugin.Commissions.ProductExtension.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Commissions.ProductExtension.Data.Mapping
{
    public class OrderItemCommissionMap : NopEntityTypeConfiguration<OrderItemCommission>
    {
        public OrderItemCommissionMap()
        {
            ToTable("OrderItemCommission");
            HasKey(p => p.Id);
            Property(p => p.OrderId).IsRequired();
            Property(p => p.OrderItemGuid).IsRequired();
            Property(p => p.ProductId).IsRequired();
            Property(p => p.PersonalVolume).IsRequired();
            Property(p => p.GroupVolume).IsRequired();
            Property(p => p.AdditionalVolume).IsRequired();
            Property(p => p.PriceExclTax);
            Property(p => p.UnitPriceExclTax);
            Property(p => p.PersonalVolumeTotal);
            Property(p => p.GroupVolumeTotal);
            Property(p => p.AdditionalVolumeTotal);

            HasRequired(i => i.OrderCommission)
                .WithMany(o => o.OrderItemCommissions)
                .HasForeignKey(i => i.OrderCommissionId);
        }
    }
}

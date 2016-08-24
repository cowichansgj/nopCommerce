using Nop.Data.Mapping;
using Nop.Plugin.Commissions.ProductExtension.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Commissions.ProductExtension.Data.Mapping
{
    public class OrderCommissionMap : NopEntityTypeConfiguration<OrderCommission>
    {
        public OrderCommissionMap()
        {
            ToTable("OrderCommission");
            HasKey(p => p.Id);
            Property(p => p.CustomerId);
            Property(p => p.OrderId);
            Property(p => p.SponsorId).HasMaxLength(64);
            Property(p => p.DistributorId).HasMaxLength(64);
            Property(p => p.OrderGuid);
            Property(p => p.OrderSubTotalExclTax).HasPrecision(18, 4);
            Property(p => p.OrderSubTotalExclTax).HasPrecision(18, 4);
        }
    }
}

using Nop.Core;
using Nop.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Commissions.ProductExtension.Domain
{
    public class OrderItemCommission : BaseEntity
    {
        public int OrderId { get; set; }
        public int OrderCommissionId { get; set; }
        public Guid OrderItemGuid { get; set; }
        public int ProductId { get; set; }
        public decimal PersonalVolume { get; set; }
        public decimal GroupVolume { get; set; }
        public decimal AdditionalVolume { get; set; }
        public decimal UnitPriceExclTax { get; set; }
        public decimal PriceExclTax { get; set; }
        public decimal PersonalVolumeTotal { get; set; }
        public decimal GroupVolumeTotal { get; set; }
        public decimal AdditionalVolumeTotal { get; set; }

        public virtual OrderCommission OrderCommission { get; set; }
        
    }
}

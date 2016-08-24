using Nop.Core;
using Nop.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Commissions.ProductExtension.Domain
{
    public class OrderCommission : BaseEntity
    {
        private ICollection<OrderItemCommission> _orderItems;

        public int CustomerId { get; set; }
        public string SponsorId { get; set; }
        public string DistributorId { get; set; }
        public int OrderId { get; set; }
        public Guid OrderGuid { get; set; }
        public decimal OrderSubTotalExclTax { get; set; }
        public decimal OrderSubTotalDiscountExclTax { get; set; }

        public virtual ICollection<OrderItemCommission> OrderItemCommissions
        {
            get { return _orderItems ?? (_orderItems = new List<OrderItemCommission>()); }
            protected set { _orderItems = value; }
        }
    }
}

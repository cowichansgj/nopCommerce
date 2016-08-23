using Nop.Core.Domain.Orders;
using Nop.Plugin.Commissions.ProductExtension.Domain;
using Nop.Plugin.Commissions.ProductExtension.Services;
using Nop.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Commissions.ProductExtension.Events
{
    public class OrderPlacedEventConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly IProductCommissionService _productCommissionService;
        private readonly IOrderCommissionService _orderCommissionService;

        public OrderPlacedEventConsumer(
            IProductCommissionService productCommissionService,
            IOrderCommissionService orderCommissionService)
        {
            _productCommissionService = productCommissionService;
            _orderCommissionService = orderCommissionService;
        }
        public void HandleEvent(OrderPlacedEvent eventMessage)
        {
            var order = eventMessage.Order;

            var orderCommission = new OrderCommission
            {
                OrderGuid = order.OrderGuid,
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                OrderSubTotalExclTax = order.OrderSubtotalExclTax,
                OrderSubTotalDiscountExclTax = order.OrderSubTotalDiscountExclTax,
            };

            foreach(var orderItem in order.OrderItems)
            {
                var productCommission = _productCommissionService.GetProductCommissionById(orderItem.ProductId);

                var orderItemCommission = new OrderItemCommission
                {
                    OrderId = order.Id,
                    ProductId = orderItem.ProductId,
                    OrderItemGuid = orderItem.OrderItemGuid,
                    PersonalVolume = productCommission.PersonalVolume,
                    GroupVolume = productCommission.GroupVolume,
                    AdditionalVolume = productCommission.AdditionalVolume,
                    PriceExclTax = orderItem.PriceExclTax,
                    UnitPriceExclTax = orderItem.UnitPriceExclTax
                };

                orderCommission.OrderItemCommissions.Add(orderItemCommission);
            }

            _orderCommissionService.InsertOrderCommission(orderCommission);
        }
    }
}

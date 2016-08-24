using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Commissions.ProductExtension.Domain;
using Nop.Plugin.Commissions.ProductExtension.Services;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.Logging;
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
        private readonly ICustomerAttributeParser _customerAttributeParser;
        private readonly ILogger _log;
        public OrderPlacedEventConsumer(
            IProductCommissionService productCommissionService,
            IOrderCommissionService orderCommissionService,
            ICustomerAttributeParser customerAttributeParser,
            ILogger log)
        {
            _productCommissionService = productCommissionService;
            _orderCommissionService = orderCommissionService;
            _customerAttributeParser = customerAttributeParser;
            _log = log;
        }
        public void HandleEvent(OrderPlacedEvent eventMessage)
        {
            try
            {
                var order = eventMessage.Order;

                var customAttributesXml = order.Customer.GetAttribute<string>(SystemCustomerAttributeNames.CustomCustomerAttributes);

                var attributes = _customerAttributeParser.ParseCustomerAttributes(customAttributesXml);
                var sponsorIdAttribute = attributes.FirstOrDefault(x => x.Name == "Sponsor ID");
                var distributorIdAttribute = attributes.FirstOrDefault(x => x.Name == "Distributor ID");

                string sponsorId = null;
                string distributorId = null;

                if (sponsorIdAttribute != null)
                {
                    sponsorId = _customerAttributeParser.ParseValues(customAttributesXml, sponsorIdAttribute.Id).SingleOrDefault();
                }

                if (distributorIdAttribute != null)
                {
                    distributorId = _customerAttributeParser.ParseValues(customAttributesXml, distributorIdAttribute.Id).SingleOrDefault();
                }

                _log.Information($"Recording order commission [orderId={order.Id}, sponsorId={sponsorId}, distributorId={distributorId}]");

                var orderCommission = new OrderCommission
                {
                    OrderGuid = order.OrderGuid,
                    OrderId = order.Id,
                    SponsorId = sponsorId,
                    DistributorId = distributorId,
                    CustomerId = order.CustomerId,
                    OrderSubTotalExclTax = order.OrderSubtotalExclTax,
                    OrderSubTotalDiscountExclTax = order.OrderSubTotalDiscountExclTax,
                };

                _orderCommissionService.InsertOrderCommission(orderCommission);

                foreach (var orderItem in order.OrderItems)
                {
                    var productCommission = _productCommissionService.GetProductCommissionById(orderItem.ProductId);

                    if (productCommission == null)
                    {
                        _log.Warning($"No Commission Values set for product [{orderItem.ProductId}]");
                    }

                    var orderItemCommission = new OrderItemCommission
                    {
                        OrderId = order.Id,
                        ProductId = orderItem.ProductId,
                        OrderItemGuid = orderItem.OrderItemGuid,
                        PersonalVolume = productCommission?.PersonalVolume ?? 0,
                        GroupVolume = productCommission?.GroupVolume ?? 0,
                        AdditionalVolume = productCommission?.AdditionalVolume ?? 0,
                        PriceExclTax = orderItem.PriceExclTax,
                        UnitPriceExclTax = orderItem.UnitPriceExclTax
                    };
                    orderCommission.OrderItemCommissions.Add(orderItemCommission);

                    _orderCommissionService.UpdateOrderCommission(orderCommission);
                }
            }catch(Exception e)
            {
                _log.Error($"Failed to add commissions for order [{eventMessage.Order.Id}]", e);
            }
        }
    }
}

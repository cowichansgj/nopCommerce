using Nop.Core.Data;
using Nop.Plugin.Commissions.ProductExtension.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Commissions.ProductExtension.Services
{

    public interface IOrderCommissionService
    {
        void InsertOrderCommission(OrderCommission orderCommission);
        void UpdateOrderCommission(OrderCommission orderCommission);

        
    }

    public class OrderCommissionService : IOrderCommissionService
    {
        private readonly IRepository<OrderCommission> _orderCommissionRepository;
        private readonly IRepository<OrderItemCommission> _orderItemCommissionRepository;

        public OrderCommissionService(
            IRepository<OrderCommission> orderCommissionRepository,
            IRepository<OrderItemCommission> orderItemCommissionRepository)
        {
            _orderCommissionRepository = orderCommissionRepository;
            _orderItemCommissionRepository = orderItemCommissionRepository;
        }
        public void InsertOrderCommission(OrderCommission orderCommission)
        {
            _orderCommissionRepository.Insert(orderCommission);
        }

        public void UpdateOrderCommission(OrderCommission orderCommission)
        {
            _orderCommissionRepository.Update(orderCommission);
        }
    }
}

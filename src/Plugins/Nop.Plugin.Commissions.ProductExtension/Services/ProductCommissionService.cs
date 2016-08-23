using Nop.Core.Data;
using Nop.Plugin.Commissions.ProductExtension.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Commissions.ProductExtension.Services
{
    public interface ICommissionsProductExtensionService
    {
        ProductCommission GetProductCommissionById(int productId);

        void InsertProductCommission(ProductCommission entity);
        void UpdateProductCommission(ProductCommission entity);
    }

    public class CommissionsProductExtensionService : ICommissionsProductExtensionService
    {
        private readonly IRepository<ProductCommission> _productCommissionRepository;
        public CommissionsProductExtensionService(IRepository<ProductCommission> productCommissionRepository)
        {
            _productCommissionRepository = productCommissionRepository;
        }

        public ProductCommission GetProductCommissionById(int productId)
        {
            if (productId == 0)
                return null;

            return _productCommissionRepository.GetById(productId);
        }

        public void InsertProductCommission(ProductCommission entity)
        {
            _productCommissionRepository.Insert(entity);
        }

        public void UpdateProductCommission(ProductCommission entity)
        {
            _productCommissionRepository.Update(entity);
        }
    }
}

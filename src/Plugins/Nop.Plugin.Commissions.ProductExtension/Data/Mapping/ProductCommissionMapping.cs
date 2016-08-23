using Nop.Data.Mapping;
using Nop.Plugin.Commissions.ProductExtension.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Commissions.ProductExtension.Data.Mapping
{
    public class ProductCommissionMapping : NopEntityTypeConfiguration<ProductCommission>
    {
        protected override void PostInitialize()
        {
            ToTable("ProductCommission");
            HasKey(p => p.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.PersonalVolume);
            Property(p => p.GroupVolume);
            Property(p => p.AdditionalVolume);

            base.PostInitialize();

        }
    }
}

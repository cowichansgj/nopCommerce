using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Commissions.ProductExtension.Domain
{
    public class ProductCommission : BaseEntity
    {
        public virtual decimal PersonalVolume { get; set; }
        public virtual decimal GroupVolume { get; set; }
        public virtual decimal AdditionalVolume { get; set; }
    }
}

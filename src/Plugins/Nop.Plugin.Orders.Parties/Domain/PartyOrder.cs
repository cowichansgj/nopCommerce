using Nop.Core;

namespace Nop.Plugin.Orders.Parties.Domain
{
    public class PartyOrder : BaseEntity
    {
        public virtual int OrderId { get; set; }
        public virtual int PartyId { get; set; }
    }
}

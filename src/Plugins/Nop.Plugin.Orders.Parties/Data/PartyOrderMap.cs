using Nop.Plugin.Orders.Parties.Domain;
using System.Data.Entity.ModelConfiguration;

namespace Nop.Plugin.Orders.Parties.Data
{
    public class PartyOrderMap : EntityTypeConfiguration<PartyOrder>
    {
        public PartyOrderMap()
        {
            ToTable("PartyOrders");

            HasKey(m => new {m.OrderId, m.PartyId});
            Property(m => m.OrderId);
            Property(m => m.PartyId);
        }
    }
}

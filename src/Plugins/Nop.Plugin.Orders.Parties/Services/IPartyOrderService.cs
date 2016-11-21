using Nop.Plugin.Orders.Parties.Domain;

namespace Nop.Plugin.Orders.Parties.Services
{
    public interface IPartyOrderService
    {
        void Associate(PartyOrder partyOrder);
    }
}

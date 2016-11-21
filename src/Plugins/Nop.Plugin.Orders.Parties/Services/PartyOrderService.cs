using Nop.Core.Data;
using Nop.Plugin.Orders.Parties.Domain;

namespace Nop.Plugin.Orders.Parties.Services
{
    public class PartyOrderService : IPartyOrderService
    {
        private readonly IRepository<PartyOrder> _repository;

        public PartyOrderService(IRepository<PartyOrder> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Adds a record to associate an order with a party.
        /// </summary>
        /// <param name="partyOrder">The record.</param>
        public void Associate(PartyOrder partyOrder)
        {
            
        }
    }
}

using Nop.Core.Plugins;
using Nop.Plugin.Orders.Parties.Data;

namespace Nop.Plugin.Orders.Parties
{
    public class PartyOrderPlugin : BasePlugin
    {
        private readonly PartyOrderObjectContext _context;

        public PartyOrderPlugin(PartyOrderObjectContext context)
        {
            _context = context;
        }

        public override void Install()
        {
            _context.Install();
            base.Install();
        }

        public override void Uninstall()
        {
            _context.Uninstall();
            base.Uninstall();
        }
    }
}

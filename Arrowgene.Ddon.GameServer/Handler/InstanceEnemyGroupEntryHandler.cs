using Arrowgene.Ddon.GameServer.Context;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceEnemyGroupEntryHandler : StructurePacketHandler<GameClient, C2SInstanceEnemyGroupEntryNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceEnemyGroupEntryHandler));

        public InstanceEnemyGroupEntryHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceEnemyGroupEntryNtc> packet)
        {
            var layout = packet.Structure.LayoutId;

            //Somehow we've entered twice.
            if (client.Character.EnemyLayoutOwnership.ContainsKey(layout.AsTuple()))
            {
                Logger.Debug($"{client.Character.FirstName} double entry at {layout}.");
                //return;
            }

            //Check if anybody else is in this layout.
            var otherClients = client.Party.Clients.Where(x => x != client && x.Character.EnemyLayoutOwnership.ContainsKey(layout.AsTuple()));
            if (otherClients.Any())
            {
                //Somebody else got here first, so wait in line.
                //Logger.Debug($"{client.Character.FirstName} waiting for {layout}, owned by {otherClients.First().Character.FirstName}");
                client.Character.EnemyLayoutOwnership[layout.AsTuple()] = false;
            }
            else
            {
                //Take ownership of it
                ContextManager.AssignMaster(client, layout);
            }
        }
    }
}

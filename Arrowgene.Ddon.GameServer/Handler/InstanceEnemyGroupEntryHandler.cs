using Arrowgene.Ddon.GameServer.Context;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

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
            List<ulong> enemiesIds = ContextManager.CreateEnemyUIDs(client.Party.InstanceEnemyManager, packet.Structure.LayoutId);
            foreach (var enemyId in enemiesIds)
            {
                if (ContextManager.GetContext(client.Party, enemyId) == null)
                {
                    continue;
                }

                // By observing packet captures we see this packet is sent.
                // Still not 100% sure on what it is actually doing.
                // To theorize, I think it may be attmepting to change
                // Which client in the party is controlling the battle data
                // for this enemy but I am not 100% confident that is the case yet.
                Logger.Debug("=====================================================================");
                Logger.Debug($"StageId={layout.StageId}, LayoutNo={layout.LayerNo}, GroupId={layout.GroupId}");
                Logger.Debug($"InstanceEnemyGroupEntryHandler: Spawning in 0x{enemyId:x16}");
                Logger.Debug("=====================================================================");
                client.Party.SendToAll(new S2CContextMasterChangeNtc()
                {
                    Info = new List<CDataMasterInfo>()
                    {
                        new CDataMasterInfo()
                        {
                            UniqueId = enemyId,
                            MasterIndex = 0
                        }
                    }
                });
            }
        }
    }
}

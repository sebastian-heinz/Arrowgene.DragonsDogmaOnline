using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Enemy;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceEnemyGroupEntryHandler : StructurePacketHandler<GameClient, C2SInstanceEnemyGroupEntryNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceEnemyGroupEntryHandler));

        private readonly EnemyManager _enemyManager;

        public InstanceEnemyGroupEntryHandler(DdonGameServer server) : base(server)
        {
            _enemyManager = server.EnemyManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceEnemyGroupEntryNtc> packet)
        {
            // TODO: Something
        }
    }
}
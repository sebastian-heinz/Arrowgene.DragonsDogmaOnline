using Arrowgene.Ddon.GameServer.Experience;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceEnemyKillHandler : StructurePacketHandler<GameClient, C2SInstanceEnemyKillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceEnemyKillHandler));

        private readonly ExpManager _expManager;

        public InstanceEnemyKillHandler(DdonGameServer server) : base(server)
        {
            _expManager = server.ExpManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceEnemyKillReq> packet)
        {
            client.Send(new S2CInstanceEnemyKillRes());

            // TODO: Calculate somehow
            uint gainedExp = 100;
            uint extraBonusExp = 0; // TODO: Figure out what this is for
            this._expManager.AddExp(client, gainedExp, extraBonusExp);

            // TODO: Exp and Lvl for the client's pawns in the party
        }
    }
}
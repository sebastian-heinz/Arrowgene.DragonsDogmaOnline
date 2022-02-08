using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCharacterPointReviveHandler : StructurePacketHandler<GameClient, C2SCharacterCharacterPointReviveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterCharacterPointReviveHandler));

        public CharacterCharacterPointReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterCharacterPointReviveReq> packet)
        {
            S2CCharacterCharacterPointReviveRes res = new S2CCharacterCharacterPointReviveRes();
            res.RevivePoint = 2;

            client.Send(res);
        }
    }
}
using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCharacterPenaltyReviveHandler : StructurePacketHandler<GameClient, C2SCharacterCharacterPenaltyReviveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterCharacterPenaltyReviveHandler));

        public CharacterCharacterPenaltyReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterCharacterPenaltyReviveReq> packet)
        {
            S2CCharacterCharacterPenaltyReviveRes res = new S2CCharacterCharacterPenaltyReviveRes();

            client.Send(res);
        }
    }
}
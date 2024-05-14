using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCharacterGoldenReviveHandler : StructurePacketHandler<GameClient, C2SCharacterCharacterGoldenReviveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterCharacterGoldenReviveHandler));

        public CharacterCharacterGoldenReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterCharacterGoldenReviveReq> packet)
        {
            S2CCharacterCharacterGoldenReviveRes res = new S2CCharacterCharacterGoldenReviveRes();
            res.GP = 0; // TODO: Implement

            client.Send(res);
        }
    }
}
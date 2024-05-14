using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterPawnGoldenReviveHandler : StructurePacketHandler<GameClient, C2SCharacterPawnGoldenReviveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterPawnGoldenReviveHandler));


        public CharacterPawnGoldenReviveHandler(DdonGameServer server) : base(server)
        {
        }


        public override void Handle(GameClient client, StructurePacket<C2SCharacterPawnGoldenReviveReq> req)
        {
            S2CCharacterPawnGoldenReviveRes res = new S2CCharacterPawnGoldenReviveRes(req.Structure);
            res.GoldenGemstonePoint = 0; // TODO: Implement
            client.Send(res);
        }
    }
}

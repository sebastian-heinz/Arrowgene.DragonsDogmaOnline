using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterPawnPointReviveHandler : StructurePacketHandler<GameClient, C2SCharacterPawnPointReviveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterPawnPointReviveHandler));


        public CharacterPawnPointReviveHandler(DdonGameServer server) : base(server)
        {
        }


        public override void Handle(GameClient client, StructurePacket<C2SCharacterPawnPointReviveReq> req)
        {
            S2CCharacterPawnPointReviveRes res = new S2CCharacterPawnPointReviveRes(req.Structure);
            res.RevivePoint = 3;
            client.Send(res);
        }
    }
}

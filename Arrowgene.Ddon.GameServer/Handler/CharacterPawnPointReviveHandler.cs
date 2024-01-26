using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using System;

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
            client.Character.StatusInfo.RevivePoint = (byte) Math.Max(0, client.Character.StatusInfo.RevivePoint-1);
            Database.UpdateStatusInfo(client.Character);

            S2CCharacterPawnPointReviveRes res = new S2CCharacterPawnPointReviveRes();
            res.RevivePoint = client.Character.StatusInfo.RevivePoint;
            client.Send(res);

            S2CCharacterUpdateRevivePointNtc ntc = new S2CCharacterUpdateRevivePointNtc()
            {
                CharacterId = client.Character.CharacterId,
                RevivePoint = client.Character.StatusInfo.RevivePoint
            };
            client.Party.SendToAllExcept(ntc, client);
        }
    }
}

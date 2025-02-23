using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterPawnPointReviveHandler : GameRequestPacketHandler<C2SCharacterPawnPointReviveReq, S2CCharacterPawnPointReviveRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterPawnPointReviveHandler));


        public CharacterPawnPointReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterPawnPointReviveRes Handle(GameClient client, C2SCharacterPawnPointReviveReq request)
        {
            client.Character.StatusInfo.RevivePoint = (byte)Math.Max(0, client.Character.StatusInfo.RevivePoint - 1);
            Database.UpdateStatusInfo(client.Character);

            S2CCharacterPawnPointReviveRes res = new S2CCharacterPawnPointReviveRes();
            res.RevivePoint = client.Character.StatusInfo.RevivePoint;

            S2CCharacterUpdateRevivePointNtc ntc = new S2CCharacterUpdateRevivePointNtc()
            {
                CharacterId = client.Character.CharacterId,
                RevivePoint = client.Character.StatusInfo.RevivePoint
            };
            client.Party.SendToAllExcept(ntc, client);

            return res;
        }
    }
}

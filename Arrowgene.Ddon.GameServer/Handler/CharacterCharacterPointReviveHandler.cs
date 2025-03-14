using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCharacterPointReviveHandler : GameRequestPacketHandler<C2SCharacterCharacterPointReviveReq, S2CCharacterCharacterPointReviveRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterCharacterPointReviveHandler));

        public CharacterCharacterPointReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterCharacterPointReviveRes Handle(GameClient client, C2SCharacterCharacterPointReviveReq request)
        {
            client.Character.StatusInfo.RevivePoint = (byte)Math.Max(0, client.Character.StatusInfo.RevivePoint - 1);
            Database.UpdateStatusInfo(client.Character);

            S2CCharacterCharacterPointReviveRes res = new S2CCharacterCharacterPointReviveRes();
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

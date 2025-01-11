using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterChargeRevivePointHandler : GameRequestPacketHandler<C2SCharacterChargeRevivePointReq, S2CCharacterChargeRevivePointRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterChargeRevivePointHandler));
        
        public CharacterChargeRevivePointHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterChargeRevivePointRes Handle(GameClient client, C2SCharacterChargeRevivePointReq request)
        {
            // TODO: Expose to settings.
            client.Character.StatusInfo.RevivePoint = 3;
            Server.Database.UpdateStatusInfo(client.Character);
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo jstZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            DateTime jstNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, jstZone);
            Server.LastRevivalPowerRechargeTime[client.Character.CharacterId] = jstNow;

            S2CCharacterUpdateRevivePointNtc ntc = new S2CCharacterUpdateRevivePointNtc()
            {
                CharacterId = client.Character.CharacterId,
                RevivePoint = client.Character.StatusInfo.RevivePoint
            };
            client.Party.SendToAllExcept(ntc, client);

            return new S2CCharacterChargeRevivePointRes()
            {
                RevivePoint = client.Character.StatusInfo.RevivePoint
            };
        }
    }
}

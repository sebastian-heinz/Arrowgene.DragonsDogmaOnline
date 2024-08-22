using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterGetReviveChargeableTimeHandler : GameStructurePacketHandler<C2SCharacterGetReviveChargeableTimeReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterGetReviveChargeableTimeHandler));
        


        public CharacterGetReviveChargeableTimeHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterGetReviveChargeableTimeReq> packet)
        {
            S2CCharacterGetReviveChargeableTimeRes res = new S2CCharacterGetReviveChargeableTimeRes();

            if(Server.LastRevivalPowerRechargeTime.ContainsKey(client.Character.CharacterId))
            {
                // Refresh revival at 12:00AM JST
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo jstZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
                DateTime jstNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, jstZone);
                DateTime nextMidnightJST = new DateTime(jstNow.Year, jstNow.Month, jstNow.Day, 0, 0, 0, jstNow.Kind).AddDays(1);
                TimeSpan remainTimeSpan = nextMidnightJST - jstNow;
                res.RemainTime = (uint) Math.Max(0, remainTimeSpan.TotalSeconds);
            }
            else
            {
                res.RemainTime = 0;
            }

            client.Send(res);
        }
    }
}

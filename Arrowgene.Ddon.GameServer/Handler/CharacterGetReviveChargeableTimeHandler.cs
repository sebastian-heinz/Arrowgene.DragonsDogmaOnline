using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterGetReviveChargeableTimeHandler : GameRequestPacketHandler<C2SCharacterGetReviveChargeableTimeReq, S2CCharacterGetReviveChargeableTimeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterGetReviveChargeableTimeHandler));
        
        public CharacterGetReviveChargeableTimeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterGetReviveChargeableTimeRes Handle(GameClient client, C2SCharacterGetReviveChargeableTimeReq request)
        {
            S2CCharacterGetReviveChargeableTimeRes res = new S2CCharacterGetReviveChargeableTimeRes();

            //  Refresh revival at 5:00AM JST.

            if (Server.GpCourseManager.InfiniteReviveRefresh())
            {
                res.RemainTime = 0;
            }
            else if(Server.LastRevivalPowerRechargeTime.ContainsKey(client.Character.CharacterId))
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo jstZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
                DateTime jstNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, jstZone);
                DateTime todayTimerJST = new DateTime(jstNow.Year, jstNow.Month, jstNow.Day, 5, 0, 0, DateTimeKind.Local);
                DateTime nextDayTimerJST = todayTimerJST.AddDays(1);
                DateTime lastRechargeTime = Server.LastRevivalPowerRechargeTime[client.Character.CharacterId];
                DateTime nextRechargeTime = (lastRechargeTime >= todayTimerJST) ? nextDayTimerJST : todayTimerJST;
                TimeSpan remainTimeSpan = nextRechargeTime - jstNow;
                res.RemainTime = (uint)Math.Max(0, remainTimeSpan.TotalSeconds);
            }
            else
            {
                res.RemainTime = 0;
            }

            return res;
        }
    }
}

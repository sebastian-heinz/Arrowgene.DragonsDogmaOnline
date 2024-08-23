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

           //  Refresh revival at 12:00AM JST. jstNow is needed to allow the else statement to update LastRevivalPowerRechargeTime if necessary.

            if(Server.LastRevivalPowerRechargeTime.ContainsKey(client.Character.CharacterId))
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo jstZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
                DateTime jstNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, jstZone);
                DateTime todayMidnightJST = new DateTime(jstNow.Year, jstNow.Month, jstNow.Day, 0, 0, 0, DateTimeKind.Local);
                DateTime nextMidnightJST = todayMidnightJST.AddDays(1);
                DateTime lastRechargeTime = Server.LastRevivalPowerRechargeTime[client.Character.CharacterId];
                DateTime nextRechargeTime = (lastRechargeTime >= todayMidnightJST) ? nextMidnightJST : todayMidnightJST;
                TimeSpan remainTimeSpan = nextRechargeTime - jstNow;
                res.RemainTime = (uint)Math.Max(0, remainTimeSpan.TotalSeconds);
            }
            else
            {
                res.RemainTime = 0;
            }

            client.Send(res);
        }
    }
}
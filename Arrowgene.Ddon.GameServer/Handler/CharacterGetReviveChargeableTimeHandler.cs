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
                DateTime lastRechargeTime = Server.LastRevivalPowerRechargeTime[client.Character.CharacterId];
                DateTime nextRechargeTime = lastRechargeTime.Add(DdonGameServer.RevivalPowerRechargeTimeSpan);
                TimeSpan remainTimeSpan = nextRechargeTime - DateTime.Now;
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
using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterChargeRevivePointHandler : GameStructurePacketHandler<C2SCharacterChargeRevivePointReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterChargeRevivePointHandler));
        
        public CharacterChargeRevivePointHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterChargeRevivePointReq> packet)
        {
            client.Character.StatusInfo.RevivePoint = 3;
            Server.Database.UpdateStatusInfo(client.Character);

            Server.LastRevivalPowerRechargeTime[client.Character.CharacterId] = DateTime.Now;

            client.Send(new S2CCharacterChargeRevivePointRes() {
                RevivePoint = client.Character.StatusInfo.RevivePoint
            });

            S2CCharacterUpdateRevivePointNtc ntc = new S2CCharacterUpdateRevivePointNtc()
            {
                CharacterId = client.Character.CharacterId,
                RevivePoint = client.Character.StatusInfo.RevivePoint
            };
            client.Party.SendToAllExcept(ntc, client);
        }
    }
}
using System;
using System.Collections.Generic;
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
            Server.Database.UpdateCharacterStatusInfo(client.Character);

            Server.LastRevivalPowerRechargeTime[client.Character.Id] = DateTime.Now;

            client.Send(new S2CCharacterChargeRevivePointRes() {
                RevivePoint = client.Character.StatusInfo.RevivePoint
            });
        }
    }
}
using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCharacterPointReviveHandler : StructurePacketHandler<GameClient, C2SCharacterCharacterPointReviveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterCharacterPointReviveHandler));

        public CharacterCharacterPointReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterCharacterPointReviveReq> packet)
        {
            client.Character.StatusInfo.RevivePoint = (byte) Math.Max(0, client.Character.StatusInfo.RevivePoint-1);
            Database.UpdateCharacterStatusInfo(client.Character);

            S2CCharacterCharacterPointReviveRes res = new S2CCharacterCharacterPointReviveRes();
            res.RevivePoint = client.Character.StatusInfo.RevivePoint;
            client.Send(res);

            S2CCharacterUpdateRevivePointNtc ntc = new S2CCharacterUpdateRevivePointNtc()
            {
                CharacterId = client.Character.Id,
                RevivePoint = client.Character.StatusInfo.RevivePoint
            };
            client.Party.SendToAllExcept(ntc, client);
        }
    }
}
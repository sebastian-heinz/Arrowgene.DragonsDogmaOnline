using System;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnLostPawnPointReviveHandler : GameRequestPacketHandler<C2SPawnLostPawnPointReviveReq, S2CPawnLostPawnPointReviveRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnLostPawnPointReviveHandler));
        
        public PawnLostPawnPointReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnLostPawnPointReviveRes Handle(GameClient client, C2SPawnLostPawnPointReviveReq request)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == request.PawnId).Single();
            pawn.PawnState = PawnState.Wait;
            Server.Database.UpdatePawnBaseInfo(pawn);

            client.Character.StatusInfo.RevivePoint = (byte) Math.Max(0, client.Character.StatusInfo.RevivePoint-1);
            Database.UpdateStatusInfo(client.Character);

            S2CCharacterUpdateRevivePointNtc ntc = new S2CCharacterUpdateRevivePointNtc()
            {
                CharacterId = client.Character.CharacterId,
                RevivePoint = client.Character.StatusInfo.RevivePoint
            };
            client.Party.SendToAllExcept(ntc, client);

            return new S2CPawnLostPawnPointReviveRes()
            {
                PawnId = request.PawnId,
                RevivePoint = client.Character.StatusInfo.RevivePoint
            };
        }
    }
}
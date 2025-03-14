using System;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnLostPawnWalletReviveHandler : GameRequestPacketHandler<C2SPawnLostPawnWalletReviveReq, S2CPawnLostPawnWalletReviveRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnLostPawnWalletReviveHandler));
        
        public PawnLostPawnWalletReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnLostPawnWalletReviveRes Handle(GameClient client, C2SPawnLostPawnWalletReviveReq request)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == request.PawnId).Single();
            pawn.PawnState = PawnState.None;
            Server.Database.UpdatePawnBaseInfo(pawn);

            if (request.Type != WalletType.RiftPoints)
            {
                // As far as im aware it can only be paid with RP
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CLIENT_REQ_ERROR);
            }

            // TODO: Validate ReviveCost value, validate that the player has the appropriate passport course effect active

            bool walletUpdate = Server.WalletManager.RemoveFromWalletNtc(client, client.Character, request.Type, request.ReviveCost);
            if (!walletUpdate)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_WARP_LACK_RIM);
            }
            uint updatedValue = Server.WalletManager.GetWalletAmount(client.Character, request.Type); 

            return new S2CPawnLostPawnWalletReviveRes()
            {
                PawnId = request.PawnId,
                Type = request.Type,
                Value = updatedValue
            };
        }
    }
}

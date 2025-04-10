using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnRentRegisteredPawnHandler : GameRequestPacketHandler<C2SPawnRentRegisteredPawnReq, S2CPawnRentRegisteredPawnRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnRentRegisteredPawnHandler));

        public PawnRentRegisteredPawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnRentRegisteredPawnRes Handle(GameClient client, C2SPawnRentRegisteredPawnReq request)
        {
            Pawn pawn = null;

            // Make sure this pawn was not already rented (we don't allow duplicates)
            if (client.Character.RentedPawns.Where(x => x.PawnId == request.RequestedPawnId).FirstOrDefault() != null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_ALREADY_RENTED);
            }

            if (Server.WalletManager.GetWalletAmount(client.Character, WalletType.RiftPoints) < request.RentalCost)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_DATA_NO_RIM);
            }

            uint ownerCharacterId = Server.Database.GetPawnOwnerCharacterId(request.RequestedPawnId);
            if (ownerCharacterId == 0)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_PAWN_PARAM_NOT_FOUND);
            }
            
            var ownerCharacter = Server.CharacterManager.SelectCharacter(ownerCharacterId);
            for (int i = 0; i < ownerCharacter.Pawns.Count; i++)
            {
                if (ownerCharacter.Pawns[i].PawnId != request.RequestedPawnId)
                {
                    continue;
                }

                pawn = ownerCharacter.Pawns[i];
                break;
            }

            if (pawn == null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_REGISTERD_DATA_NOT_FOUND);
            }

            var walletUpdate = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.RiftPoints, request.RentalCost)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_FAIL, "Insufficient Rim for pawn rental.");

            // TODO: Add pawn to rented pawn list for player
            // TODO: Store snapshot in DB
            pawn.PawnType = PawnType.Support;
            pawn.PawnState = PawnState.None;
            client.Character.RentedPawns.Add(pawn);

            Server.AchievementManager.HandleHirePawn(client).Send();

            return new S2CPawnRentRegisteredPawnRes()
            {
                TotalRim = walletUpdate.Value
            };
        }
    }
}

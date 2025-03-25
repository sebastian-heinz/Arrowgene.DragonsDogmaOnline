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
    public class PawnLostPawnGoldenReviveHandler : GameRequestPacketHandler<C2SPawnLostPawnGoldenReviveReq, S2CPawnLostPawnGoldenReviveRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnLostPawnGoldenReviveHandler));
        
        public PawnLostPawnGoldenReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnLostPawnGoldenReviveRes Handle(GameClient client, C2SPawnLostPawnGoldenReviveReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);
            pawn.PawnState = PawnState.None;
            Server.Database.UpdatePawnBaseInfo(pawn);
            
            bool walletUpdate = Server.WalletManager.RemoveFromWalletNtc(client, client.Character, WalletType.GoldenGemstones, 1); // TODO: Get price from settings.
            if (!walletUpdate)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_GP_LACK_GP);
            }
            byte updatedGP = (byte) Server.WalletManager.GetWalletAmount(client.Character, WalletType.GoldenGemstones); 

            return new S2CPawnLostPawnGoldenReviveRes()
            {
                PawnId = request.PawnId,
                GP = updatedGP
            };
        }
    }
}

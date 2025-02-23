using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpAreaWarpHandler : GameRequestPacketHandler<C2SWarpAreaWarpReq, S2CWarpAreaWarpRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpAreaWarpHandler));

        public WarpAreaWarpHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CWarpAreaWarpRes Handle(GameClient client, C2SWarpAreaWarpReq request)
        {
            uint price = request.Price; // TODO: Don't trust packet.Structure.Price and check its price server side

            CDataUpdateWalletPoint updateWallet = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.RiftPoints, price)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_WARP_LACK_RIM);

            S2CWarpAreaWarpRes response = new S2CWarpAreaWarpRes();
            response.WarpPointId = request.WarpPointId;
            response.Rim = updateWallet.Value;
   
            if(request.Price > 0) {
                S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
                updateCharacterItemNtc.UpdateWalletList.Add(updateWallet);
                client.Send(updateCharacterItemNtc);
            }

            client.LastWarpPointId = request.WarpPointId;
            client.LastWarpDateTime = DateTime.UtcNow;

            return response;
        }
    }
}

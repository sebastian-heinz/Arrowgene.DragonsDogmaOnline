using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpWarpHandler : GameRequestPacketHandler<C2SWarpWarpReq, S2CWarpWarpRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetWarpPointListHandler));

        public WarpWarpHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CWarpWarpRes Handle(GameClient client, C2SWarpWarpReq request)
        {
            uint price = request.Price; // TODO: Don't trust packet.Structure.Price and check its price server side

            CDataUpdateWalletPoint updateWallet = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.RiftPoints, price)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_WARP_LACK_RIM);

            S2CWarpWarpRes response = new S2CWarpWarpRes();
            response.WarpPointId = request.DestPointId;
            response.Rim = updateWallet.Value;

            if(request.Price > 0) {
                S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
                updateCharacterItemNtc.UpdateWalletList.Add(updateWallet);
                client.Send(updateCharacterItemNtc);
            }

            client.LastWarpPointId = request.DestPointId;
            client.LastWarpDateTime = DateTime.UtcNow;

            return response;
        }
    }
}

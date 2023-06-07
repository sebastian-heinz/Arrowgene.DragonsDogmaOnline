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
    public class WarpWarpHandler : StructurePacketHandler<GameClient, C2SWarpWarpReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetWarpPointListHandler));


        public WarpWarpHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpWarpReq> packet)
        {
            uint price = packet.Structure.Price; // TODO: Don't trust packet.Structure.Price and check its price server side

            CDataWalletPoint walletPoint = client.Character.WalletPointList.Where(wp => wp.Type == WalletType.RiftPoints).Single();
            walletPoint.Value -= price;
            Database.UpdateWalletPoint(client.Character.CharacterId, walletPoint);

            S2CWarpWarpRes response = new S2CWarpWarpRes();
            response.WarpPointId = packet.Structure.DestPointId;
            response.Rim = walletPoint.Value;
            client.Send(response);

            if(packet.Structure.Price > 0) {
                S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
                CDataUpdateWalletPoint updateWallet = new CDataUpdateWalletPoint();
                updateWallet.Type = WalletType.RiftPoints;
                updateWallet.AddPoint = (int) -price;
                updateWallet.Value = walletPoint.Value;
                updateCharacterItemNtc.UpdateWalletList.Add(updateWallet);
                client.Send(updateCharacterItemNtc);
            }

            client.LastWarpPointId = packet.Structure.DestPointId;
            client.LastWarpDateTime = DateTime.Now;
        }
    }
}
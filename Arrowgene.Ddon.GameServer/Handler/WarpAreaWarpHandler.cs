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
    public class WarpAreaWarpHandler : StructurePacketHandler<GameClient, C2SWarpAreaWarpReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpAreaWarpHandler));

        public WarpAreaWarpHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpAreaWarpReq> packet)
        {
            uint price = packet.Structure.Price; // TODO: Don't trust packet.Structure.Price and check its price server side

            CDataWalletPoint walletPoint = client.Character.WalletPointList.Where(wp => wp.Type == WalletType.RiftPoints).Single();
            walletPoint.Value -= price;
            Database.UpdateWalletPoint(client.Character.CharacterId, walletPoint);

            S2CWarpAreaWarpRes obj = new S2CWarpAreaWarpRes();
            obj.WarpPointId = packet.Structure.WarpPointId;
            obj.Rim = walletPoint.Value;
            client.Send(obj);

            if(packet.Structure.Price > 0) {
                S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
                CDataUpdateWalletPoint updateWallet = new CDataUpdateWalletPoint();
                updateWallet.Type = WalletType.RiftPoints;
                updateWallet.AddPoint = (int) -price;
                updateWallet.Value = walletPoint.Value;
                updateCharacterItemNtc.UpdateWalletList.Add(updateWallet);
                client.Send(updateCharacterItemNtc);
            }

            client.LastWarpPointId = packet.Structure.WarpPointId;
            client.LastWarpDateTime = DateTime.Now;
        }
    }
}

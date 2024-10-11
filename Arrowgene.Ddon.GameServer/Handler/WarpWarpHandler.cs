using System;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpWarpHandler : GameStructurePacketHandler<C2SWarpWarpReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetWarpPointListHandler));

        public WarpWarpHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpWarpReq> packet)
        {
            uint price = packet.Structure.Price; // TODO: Don't trust packet.Structure.Price and check its price server side

            CDataUpdateWalletPoint updateWallet = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.RiftPoints, price);

            S2CWarpWarpRes response = new S2CWarpWarpRes();
            response.WarpPointId = packet.Structure.DestPointId;
            response.Rim = updateWallet.Value;
            client.Send(response);

            if(packet.Structure.Price > 0) {
                S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
                updateCharacterItemNtc.UpdateWalletList.Add(updateWallet);
                client.Send(updateCharacterItemNtc);
            }

            client.LastWarpPointId = packet.Structure.DestPointId;
            client.LastWarpDateTime = DateTime.UtcNow;
        }
    }
}

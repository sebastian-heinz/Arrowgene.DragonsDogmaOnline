#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpChangeCapToGpHandler : GameRequestPacketHandler<C2SGpChangeCapToGpReq, S2CGpChangeCapToGpRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpChangeCapToGpHandler));

        public GpChangeCapToGpHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGpChangeCapToGpRes Handle(GameClient client, C2SGpChangeCapToGpReq request)
        {
            S2CGpChangeCapToGpRes res = new S2CGpChangeCapToGpRes();

            // TODO: fetch number of GP via some asset for change list IDs
            res.ChangeListID = request.ChangeListID;
            uint changeListGPValue = 1;

            // TODO: based on Settlement ID figure out which currency was used
            uint walletPoints = Server.WalletManager.AddToWalletNtc(client, client.Character, WalletType.GoldenGemstones, changeListGPValue);

            res.GP = walletPoints;
            // TODO: store CAP somewhere?
            res.CAP = 0;

            return res;
        }
    }
}

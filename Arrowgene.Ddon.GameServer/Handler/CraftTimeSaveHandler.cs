using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftTimeSaveHandler : GameRequestPacketQueueHandler<C2SCraftTimeSaveReq, S2CCraftTimeSaveRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftCancelCraftHandler));

        public CraftTimeSaveHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SCraftTimeSaveReq request)
        {
            PacketQueue packetQueue = new();

            CraftProgress craftProgress = Server.Database.SelectPawnCraftProgress(client.Character.CharacterId, request.PawnID)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_CRAFT_INVALID_CRAFT_STAGE);
            craftProgress.RemainTime = 0;

            // TODO: Fetch the actual cost via the ID in the req which points to some time save config sent also via craft setting handler
            bool walletUpdate = Server.WalletManager.RemoveFromWalletNtc(client, client.Character, WalletType.GoldenGemstones, request.Num);
            if (!walletUpdate)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_GP_LACK_GP);
            }

            Server.Database.UpdatePawnCraftProgress(craftProgress);
            client.Enqueue(new S2CCraftTimeSaveRes { PawnID = request.PawnID, RemainTime = 0 }, packetQueue);
            client.Enqueue(new S2CCraftFinishCraftNtc { PawnId = request.PawnID }, packetQueue);
            return packetQueue;
        }
    }
}

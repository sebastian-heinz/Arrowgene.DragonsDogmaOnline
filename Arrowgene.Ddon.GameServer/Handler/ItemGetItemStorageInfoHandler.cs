#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemGetItemStorageInfoHandler : GameRequestPacketHandler<C2SItemGetItemStorageInfoReq, S2CItemGetItemStorageInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemGetItemStorageInfoHandler));

        public ItemGetItemStorageInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CItemGetItemStorageInfoRes Handle(GameClient client, C2SItemGetItemStorageInfoReq request)
        {
            S2CItemGetItemStorageInfoRes res = new S2CItemGetItemStorageInfoRes();

            var itemPost = client.Character.Storage.GetStorage(StorageType.ItemPost);
            res.GameItemStorageInfoList.Add(new CDataGameItemStorageInfo
            {
                GameItemStorage = new CDataGameItemStorage
                {
                    StorageType = StorageType.ItemPost
                },
                UsedSlotNum = itemPost.EmptySlots(),
                MaxSlotNum = itemPost.MaxSlots()
            });

            return res;
        }
    }
}

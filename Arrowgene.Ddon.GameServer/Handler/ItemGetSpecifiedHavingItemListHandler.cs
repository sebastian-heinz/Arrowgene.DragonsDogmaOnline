using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemGetSpecifiedHavingItemListHandler : GameRequestPacketHandler<C2SItemGetSpecifiedHavingItemListReq, S2CItemGetSpecifiedHavingItemListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemGetSpecifiedHavingItemListHandler));

        public ItemGetSpecifiedHavingItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CItemGetSpecifiedHavingItemListRes Handle(GameClient client, C2SItemGetSpecifiedHavingItemListReq request)
        {
            var result = new S2CItemGetSpecifiedHavingItemListRes();

            var characterStorage = Server.Database.SelectAllStoragesByCharacterId(client.Character.CharacterId);
            foreach (var storage in characterStorage.GetAllStorages())
            {
                var matches = storage.Value.FindItemsById(request.ItemId);
                foreach (var match in matches)
                {
                    var item = match.Item2;
                    result.ItemList.Add(new CDataItemList()
                    {
                        AddStatusParamList = item.AddStatusParamList,
                        Bind = false,
                        Color = item.Color,
                        EquipElementParamList = item.EquipElementParamList,
                        EquipPoint = item.EquipPoints,
                        ItemId = item.ItemId,
                        ItemUId = item.UId,
                        ItemNum = match.Item3,
                        PlusValue = item.PlusValue,
                        SlotNo = match.Item1,
                        StorageType = storage.Key,
                        EquipStatParamList = item.EquipStatParamList,
                        SafetySetting = item.SafetySetting,
                    });
                }
            }

            return result;
        }
    }
}

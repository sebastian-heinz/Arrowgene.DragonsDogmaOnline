using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using YamlDotNet.Core.Tokens;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemEmbodyItemsHandler : GameRequestPacketQueueHandler<C2SItemEmbodyItemsReq, S2CItemEmbodyItemsRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemEmbodyItemsHandler));

        public ItemEmbodyItemsHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SItemEmbodyItemsReq request)
        {
            var packets = new PacketQueue();

            var bbmCharacter = client.Character;

            Server.Database.ExecuteInTransaction(connection =>
            {
                var normalCharacter = Server.Database.SelectCharacter(client.Character.CharacterId, connection);

                bool toItemBag = ItemManager.SendToItemBag(request.StorageType);
                foreach (var embodyItem in request.ItemList)
                {
                    (StorageType storageType, Tuple<ushort, Item, uint> itemProps) = bbmCharacter.Storage.FindItemByUIdInStorage(ItemManager.BbmEmbodyStorages, embodyItem.UId);
                    var (slotNo, item, amount) = itemProps;

                    if (normalCharacter.Storage.GetStorage(storageType).EmptySlots() == 0)
                    {
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_STORAGE_CAPACITY_OVER, $"Player storage {storageType} is full");
                    }

                    var exchangeItemNtc = new S2CItemUpdateCharacterItemNtc()
                    {
                        UpdateType = ItemNoticeType.SwitchingStorage
                    };

                    exchangeItemNtc.UpdateItemList.Add(Server.ItemManager.ConsumeItemByUId(Server, bbmCharacter, storageType, embodyItem.UId, embodyItem.Num, connection));
                    client.Enqueue(exchangeItemNtc, packets);

                    // Remove the item from the BBM character
                    if (storageType == StorageType.CharacterEquipment)
                    {
                        var equipNtc = new S2CItemUpdateCharacterItemNtc()
                        {
                            UpdateType = ItemNoticeType.GatherEquipItem
                        };
                        equipNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(bbmCharacter, item, storageType, slotNo, 0, 0));
                        equipNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(null, new Item() { ItemId = 0, UId = "" }, storageType, slotNo, 1, 1));
                        client.Enqueue(equipNtc, packets);

                        S2CEquipChangeCharacterEquipNtc changeCharacterEquipNtc = new S2CEquipChangeCharacterEquipNtc()
                        {
                            CharacterId = bbmCharacter.CharacterId,
                            EquipItemList = bbmCharacter.Equipment.AsCDataEquipItemInfo(EquipType.Performance),
                            VisualEquipItemList = bbmCharacter.Equipment.AsCDataEquipItemInfo(EquipType.Visual)
                        };
                        client.Enqueue(changeCharacterEquipNtc, packets);
                    }

                    if (storageType == StorageType.CharacterEquipment)
                    {
                        storageType = StorageType.ItemBagEquipment;
                    }

                    Server.ItemManager.EmbodyItem(Server, normalCharacter, storageType, item, amount, connection);

                    client.Enqueue(new S2CItemUpdateCharacterItemNtc()
                    {
                        UpdateWalletList = new List<CDataUpdateWalletPoint>()
                        {
                            Server.WalletManager.RemoveFromWallet(client.Character, request.WalletType, 3, connection)
                                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INTERNAL_ERROR, "Lacking item embody price.") // TODO: Why is the price not passed?
                        }
                    }, packets);

                    // We don't send an NTC with item updates so the storage doesn't update again on the
                    // BBM character we are currently play as. When we switch back to the other game mode,
                    // the proper NTCs will be sent.
                }
            });

            client.Enqueue(new S2CItemEmbodyItemsRes(), packets);

            return packets;
        }
    }
}

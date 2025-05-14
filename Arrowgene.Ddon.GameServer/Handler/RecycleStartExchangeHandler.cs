using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class RecycleStartExchangeHandler : GameRequestPacketHandler<C2SRecycleStartExchangeReq, S2CRecycleStartExchangeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RecycleStartExchangeHandler));

        public RecycleStartExchangeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CRecycleStartExchangeRes Handle(GameClient client, C2SRecycleStartExchangeReq request)
        {
            var updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            
            var equipmentRecycleMixin = Server.ScriptManager.MixinModule.Get<IEquipmentRecycleMixin>("equipment_recycle") ??
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_REQUIRED_SERVER_SCRIPT_MISSING, "Couldn't find an object for the script 'equipment_cycle.csx'");

            var item = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, request.ItemUID)?.Item2.Item2 ??
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_NOT_FOUND, $"Unable to find the item '{request.ItemUID}' in the players storage");

            Server.Database.ExecuteInTransaction(connection =>
            {
                ClientItemInfo itemInfo = Server.ItemManager.LookupInfoByUID(Server, request.ItemUID, connection) ??
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_CLIENT_ITEM_INFO_MISSING, $"Unable to find item information for '{request.ItemUID}'");

                var recycleRewards = equipmentRecycleMixin.GetRecycleRewards(Server.AssetRepository, itemInfo, item);
                for (var i = 0; i < Math.Min(recycleRewards.ItemRewards.Count, recycleRewards.NumRewards); i++)
                {
                    var rolledSlot = Random.Shared.Next(0, recycleRewards.ItemRewards.Count);
                    var item = recycleRewards.ItemRewards[rolledSlot];

                    var amount = item.Amount;
                    if (amount > 1)
                    {
                        // Randomize the amount if it is set to more than 1
                        amount = (uint)Random.Shared.Next(1, (int)amount + 1);
                    }

                    updateCharacterItemNtc.UpdateItemList.AddRange(Server.ItemManager.AddItem(Server, client.Character, StorageUtils.StorageToBag(request.StorageType), (uint)item.ItemId, amount, connectionIn: connection));
                }

                foreach (var walletPoint in recycleRewards.WalletRewards)
                {
                    updateCharacterItemNtc.UpdateWalletList.Add(Server.WalletManager.AddToWallet(client.Character, walletPoint.WalletType, walletPoint.Amount, connectionIn: connection));
                }

                updateCharacterItemNtc.UpdateItemList.AddRange(Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.EquipmentStorages, request.ItemUID, 1, connectionIn: connection));

                byte attempts = (byte)(Server.Database.GetRecycleEquipmentAttempts(client.Character.CharacterId, connectionIn: connection) + 1);
                Server.Database.UpsertRecycleEquipmentRecord(client.Character.CharacterId, attempts, connectionIn: connection);
            });
            client.Send(updateCharacterItemNtc);

            return new()
            {
                Unk0 = 0,
                ItemUpdateResultList = updateCharacterItemNtc.UpdateItemList,
                WalletPointList = updateCharacterItemNtc.UpdateWalletList.Select(x => new CDataWalletPoint()
                {
                    Type = x.Type,
                    Value = x.Value
                }).ToList(),
            };
        }
    }
}

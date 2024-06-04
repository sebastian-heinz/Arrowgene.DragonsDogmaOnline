#nullable enable
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Chat.Command.Commands;
using System.Data.Entity;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartEquipGradeUpHandler : GameStructurePacketHandler<C2SCraftStartEquipGradeUpReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartEquipGradeUpHandler));
        private static readonly List<StorageType> STORAGE_TYPES = new List<StorageType> {
            StorageType.ItemBagConsumable, StorageType.ItemBagMaterial, StorageType.ItemBagEquipment, StorageType.ItemBagJob, 
            StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion, StorageType.StorageChest
        };

        private readonly ItemManager _itemManager;

        public CraftStartEquipGradeUpHandler(DdonGameServer server) : base(server)
        {
            _itemManager = server.ItemManager;;
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftStartEquipGradeUpReq> packet)
        {
            string equipItemUID = packet.Structure.EquipItemUID;
             //TODO need to get access to RecipeList, since this contains a reference to Gold/Cost, etc.

            CDataMDataCraftGradeupRecipe json_data = new CDataMDataCraftGradeupRecipe();

            // Define local variables for calculations
            uint gearupgradeID = json_data.GradeupItemID;
            uint goldRequired = json_data.Cost;
            uint nextGrade = json_data.Unk0;
            uint currentTotalEquipPoint = 0; // Equip Points are probably handled elsewhere, since its
            uint addEquipPoint = 350;         // not in the JSON or Request.

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = 0;

            foreach (var craftMaterial in packet.Structure.CraftMaterialList)
            {
                try
                {
                    List<CDataItemUpdateResult> updateResults = _itemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, STORAGE_TYPES, craftMaterial.ItemUId, craftMaterial.ItemNum);
                    updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                }
                catch (NotEnoughItemsException e)
                {
                    Logger.Exception(e);
                    client.Send(new S2CCraftStartCraftRes()
                    {
                        Result = 1
                    });
                    return;
                }
            }

            if(packet.Structure.CraftSupportPawnIDList.Count > 0)
            {
                goldRequired = (uint)(goldRequired*0.95);
            }

            // Substract craft price
            CDataWalletPoint wallet = client.Character.WalletPointList.Where(wp => wp.Type == WalletType.Gold).Single();
            wallet.Value = (uint)Math.Max(0, (int)wallet.Value - (int)goldRequired);
            Database.UpdateWalletPoint(client.Character.CharacterId, wallet);
            updateCharacterItemNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint()
            {
                Type = WalletType.Gold,
                AddPoint = (int)-goldRequired,
                Value = wallet.Value
            });

            // Exchange upgraded items
            List<CDataItemUpdateResult> AddItemResult = _itemManager.AddItem(Server, client.Character, false, 1674, 1 * 1);
            List<CDataItemUpdateResult> RemoveItemResult = _itemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, STORAGE_TYPES, equipItemUID, 1);
            updateCharacterItemNtc.UpdateItemList.AddRange(AddItemResult);
            updateCharacterItemNtc.UpdateItemList.AddRange(RemoveItemResult);

            // TODO we need to implement Pawn craft levels since that affects the points that get added


            var res = new S2CCraftStartEquipGradeUpRes()
            {
                GradeUpItemUID = equipItemUID, // This seems to need tot be your current UID.
                GradeUpItemID = gearupgradeID, // This has to be the upgrade step ID.
                TotalEquipPoint = currentTotalEquipPoint + addEquipPoint, // Dummy math just to make the bar slide up (HMMM HAPPY CHEMICALS)
                EquipGrade = nextGrade, // It expects a valid number or it won't show the result when you enhance, (presumably we give this value when filling the bar)
                IsGreatSuccess = true, // Just changes the banner from "Success" to "GreatSuccess" we'd have to augment the addEquipPoint value when this is true.
            };
            client.Send(updateCharacterItemNtc);
            client.Send(res);
        }
    }
}
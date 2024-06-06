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
using Arrowgene.Ddon.Database.Sql;

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
            _itemManager = server.ItemManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftStartEquipGradeUpReq> packet)
        {
            string equipItemUID = packet.Structure.EquipItemUID;


            // Finding the Recipe we need based on the requested UID.
            uint equipItemID = _itemManager.LookupItemByUID(Server, equipItemUID); 
  
            uint charid = client.Character.CharacterId;
            uint pawnid = packet.Structure.CraftMainPawnID;


            // this ^ didn't seem to make any differences anywhere, possibly because our request packet isn't requested equipped gear.

            // Getting access to the GradeUpRecipe JSON data.
            CDataMDataCraftGradeupRecipe json_data = Server.AssetRepository.CraftingGradeUpRecipesAsset
                .SelectMany(recipes => recipes.RecipeList)
                .Where(recipe => recipe.ItemID == equipItemID)
                .First();

            // Define local variables for calculations
            uint gearupgradeID = json_data.GradeupItemID;
            uint goldRequired = json_data.Cost;
            uint nextGrade = json_data.Unk0; // This might be Unk0 in the JSON but is probably in the DB or something.
            uint currentTotalEquipPoint = 0; // Equip Points are probably handled elsewhere, since its not in the JSON or Request.
            uint addEquipPoint = 350;     
            bool dogreatsucess = true;

            // TODO: we need to implement Pawn craft levels since that affects the points that get added
            // TODO: we need a dice roll to decide if greatsuccess is true or not. (also needs to add some amount of extra points if true.)


            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = 0;


            // Remove crafting materials
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
                    client.Send(new S2CCraftStartEquipGradeUpRes()
                    {
                        Result = 1
                    });
                    return;
                }
            }


            // Subtract less Gold if supportpawn is used.
            if(packet.Structure.CraftSupportPawnIDList.Count > 0)
            {
                goldRequired = (uint)(goldRequired*0.95);
            }

            // Substract Gold based on JSON cost.
            // TODO: use WalletManager instead.
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
            List<CDataItemUpdateResult> RemoveItemResult = _itemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, STORAGE_TYPES, equipItemUID, 1);
            List<CDataItemUpdateResult> AddItemResult = _itemManager.AddItem(Server, client.Character, false, gearupgradeID, 1 * 1);
            updateCharacterItemNtc.UpdateItemList.AddRange(RemoveItemResult);
            updateCharacterItemNtc.UpdateItemList.AddRange(AddItemResult);

            // Supplying the response packet with data
            var res = new S2CCraftStartEquipGradeUpRes()
            {
                GradeUpItemUID = equipItemUID, // This seems to need tot be your current UID.
                GradeUpItemID = gearupgradeID, // This has to be the upgrade step ID.
                TotalEquipPoint = currentTotalEquipPoint + addEquipPoint, // Dummy math just to make the bar slide up (HMMM HAPPY CHEMICALS)
                EquipGrade = nextGrade, // It expects a valid number or it won't show the result when you enhance, (presumably we give this value when filling the bar)
                IsGreatSuccess = dogreatsucess, // Just changes the banner from "Success" to "GreatSuccess" we'd have to augment the addEquipPoint value when this is true.
                BeforeItemID = equipItemID, // I don't know why the response wants the "beforeid" its unclear what this means too? should it be 0 if step 1? hmm.
                Unk0 = true, // Unk0 being true says "Grade Up" when filling rather than "Grade Max", so we need to track when we hit max upgrade.
            };
            client.Send(updateCharacterItemNtc);
            client.Send(res);
        }
    }
}
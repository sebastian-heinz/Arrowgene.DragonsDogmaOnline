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

            // Instantiate CDataCraftCustomGradeUp
            CDataCraftCustomGradeUp gradeUpData = new CDataCraftCustomGradeUp();


            // Define local variables for calculations
            string gearUID = gradeUpData.GradeUpItemUID;
            uint gearupgradeID = gradeUpData.GradeUpItemID;
            uint goldRequired = gradeUpData.Gold;
            uint currentTotalEquipPoint = gradeUpData.TotalEquipPoint;
            uint addEquipPoint = gradeUpData.AddEquipPoint;
            uint nextGrade = gradeUpData.EquipGrade;

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
            List<CDataItemUpdateResult> RemoveItemResult = _itemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, STORAGE_TYPES, gearUID, 1);
            updateCharacterItemNtc.UpdateItemList.AddRange(AddItemResult);
            updateCharacterItemNtc.UpdateItemList.AddRange(RemoveItemResult);

            // TODO we need to implement Pawn craft levels since that affects the points that get added


            var res = new S2CCraftStartEquipGradeUpRes()
            {
                GradeUpItemUID = gearUID, // I assume this needs to be set? without it points don't get added (and it GradeUpItemUID is empty?)
                GradeUpItemID = gearupgradeID,
                TotalEquipPoint = currentTotalEquipPoint + addEquipPoint, // Dummy math just to make the bar slide up (HMMM HAPPY CHEMICALS)
                EquipGrade = nextGrade,
                IsGreatSuccess = true,
            };
            client.Send(updateCharacterItemNtc);
            client.Send(res);
        }
    }
}
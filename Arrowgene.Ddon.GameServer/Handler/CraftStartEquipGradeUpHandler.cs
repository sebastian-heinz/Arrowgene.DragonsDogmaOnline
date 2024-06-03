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

            // TODO we need to implement Pawn craft levels since that affects the points that get added
            // These are dummy values just to have the bar do something, definitely needs to be done differently.
            uint currentTotalEquipPoint = 200;
            uint addEquipPoint = 150;

            var res = new S2CCraftStartEquipGradeUpRes()
            {
                GradeUpItemUID = equipItemUID, // I assume this needs to be set? without it points don't get added (and it GradeUpItemUID is empty?)
                GradeUpItemID = 1674,
                TotalEquipPoint = currentTotalEquipPoint + addEquipPoint, // Dummy math just to make the bar slide up (HMMM HAPPY CHEMICALS)
                EquipGrade = 2,
                IsGreatSuccess = true,
            };
            client.Send(res);
        }
    }
}
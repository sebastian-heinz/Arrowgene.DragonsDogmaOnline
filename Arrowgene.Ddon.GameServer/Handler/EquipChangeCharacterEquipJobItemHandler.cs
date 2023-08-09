using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
#nullable enable
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Model;
using System;
using Arrowgene.Ddon.GameServer.Characters;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangeCharacterEquipJobItemHandler : GameStructurePacketHandler<C2SEquipChangeCharacterEquipJobItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangeCharacterEquipJobItemHandler));
        
        // TODO: Other storages (GG storage?)
        private static readonly StorageType[] JobItemPossibleStorageTypes = {StorageType.ItemBagJob, StorageType.StorageBoxNormal};

        public EquipChangeCharacterEquipJobItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SEquipChangeCharacterEquipJobItemReq> packet)
        {
            // TODO: Persist in DB
            // TODO: Move to EquipManager

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new();

            foreach (CDataChangeEquipJobItem changeEquipJobItem in packet.Structure.ChangeEquipJobItemList)
            {
                if(changeEquipJobItem.EquipJobItemUId.Length == 0)
                {
                    // UNEQUIP
                    // Remove from equipment
                    Item equippedJobItem = client.Character.Equipment.GetJobItem(client.Character.Job, changeEquipJobItem.EquipSlotNo) ?? throw new Exception("No job item equipped in this slot");
                    client.Character.Equipment.SetJobItem(null, client.Character.Job, changeEquipJobItem.EquipSlotNo);
                    Server.Database.DeleteEquipJobItem(client.Character.CommonId, client.Character.Job, changeEquipJobItem.EquipSlotNo);
                }
                else
                {
                    // EQUIP

                    // Check in item bag and storage box (Why in the world doesn't the packet send the storage and slot)
                    (ushort storageSlotNo, Item storageItem, uint storageItemNum) = JobItemPossibleStorageTypes
                        .Select(storageType => client.Character.Storage.getStorage(storageType).findItemByUId(changeEquipJobItem.EquipJobItemUId))
                        .Where(slotItemAndCount => slotItemAndCount != null)
                        .First()!;

                    // Remove previous equipped item from equipment
                    Item? equippedJobItem = client.Character.Equipment.GetJobItem(client.Character.Job, changeEquipJobItem.EquipSlotNo);
                    if (equippedJobItem != null)
                    {
                        client.Character.Equipment.SetJobItem(null, client.Character.Job, changeEquipJobItem.EquipSlotNo);
                        Server.Database.DeleteEquipJobItem(client.Character.CommonId, client.Character.Job, changeEquipJobItem.EquipSlotNo);
                    }

                    // Place storage item in equipment
                    client.Character.Equipment.SetJobItem(storageItem, client.Character.Job, changeEquipJobItem.EquipSlotNo);
                    Server.Database.InsertEquipJobItem(storageItem.UId, client.Character.CommonId, client.Character.Job, changeEquipJobItem.EquipSlotNo);
                }
            }

            List<CDataEquipJobItem> equippedJobItems = client.Character.Equipment.getJobItemsAsCDataEquipJobItem(client.Character.Job);
            client.Send(new S2CEquipChangeCharacterEquipJobItemRes() 
            {
                EquipJobItemList = equippedJobItems
            });

            client.Party.SendToAll(new S2CEquipChangeCharacterEquipJobItemNtc()
            {
                CharacterId = client.Character.CharacterId,
                EquipJobItemList = equippedJobItems
            });

            client.Send(updateCharacterItemNtc);
        }
    }
}
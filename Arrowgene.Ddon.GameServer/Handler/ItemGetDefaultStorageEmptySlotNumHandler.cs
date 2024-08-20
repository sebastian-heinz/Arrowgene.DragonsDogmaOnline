using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.GameServer.Characters;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemGetDefaultStorageEmptySlotNumHandler : GameRequestPacketHandler<C2SItemGetDefaultStorageEmptySlotNumReq, S2CItemGetDefaultStorageEmptySlotNumRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemGetDefaultStorageEmptySlotNumHandler));

        public ItemGetDefaultStorageEmptySlotNumHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CItemGetDefaultStorageEmptySlotNumRes Handle(GameClient client, C2SItemGetDefaultStorageEmptySlotNumReq request)
        {
            var result = new S2CItemGetDefaultStorageEmptySlotNumRes();

            var characterStorage = Server.Database.SelectAllStoragesByCharacterId(client.Character.NormalCharacterId);
            foreach (var storageType in ItemManager.BbmEmbodyStorages)
            {
                var storage = characterStorage.GetStorage(storageType);
                result.EmptySlotNumList.Add(new CDataStorageEmptySlotNum()
                {
                    StorageType = storageType,
                    Slots = storage.EmptySlots()
                });
            }

            return result;
        }
    }
}

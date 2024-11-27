using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangeCharacterStorageEquipHandler : GameRequestPacketQueueHandler<C2SEquipChangeCharacterStorageEquipReq, S2CEquipChangeCharacterStorageEquipRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangeCharacterStorageEquipHandler));

        private readonly EquipManager equipManager;

        public EquipChangeCharacterStorageEquipHandler(DdonGameServer server) : base(server)
        {
            equipManager = server.EquipManager;
        }

        public override PacketQueue Handle(GameClient client, C2SEquipChangeCharacterStorageEquipReq request)
        {
            PacketQueue queue = new();

            if (!Server.EquipManager.CanMeetStorageRequirements(Server, client, client.Character, request.ChangeCharacterEquipList, new List<StorageType>() { StorageType.StorageBoxNormal }))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_STORAGE_CAPACITY_OVER);
            }

            Server.Database.ExecuteInTransaction(connection =>
            {
            queue.AddRange(equipManager.HandleChangeEquipList(
                    Server, 
                    client, 
                    client.Character, 
                    request.ChangeCharacterEquipList, 
                    ItemNoticeType.ChangeStorageEquip, 
                    ItemManager.BoxStorageTypes,
                    connection));
            });

            client.Enqueue(new S2CEquipChangeCharacterStorageEquipRes()
            {
                CharacterEquipList = request.ChangeCharacterEquipList
                // TODO: Unk0
            }, queue);

            return queue;
        }
    }
}

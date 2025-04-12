using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangePawnStorageEquipHandler : GameRequestPacketQueueHandler<C2SEquipChangePawnStorageEquipReq, S2CEquipChangePawnStorageEquipRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangePawnStorageEquipHandler));
        
        private readonly EquipManager equipManager;

        public EquipChangePawnStorageEquipHandler(DdonGameServer server) : base(server)
        {
            equipManager = server.EquipManager;
        }

        public override PacketQueue Handle(GameClient client, C2SEquipChangePawnStorageEquipReq request)
        {
            PacketQueue queue = new();
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);

            if (!Server.EquipManager.CanMeetStorageRequirements(Server, client, pawn, request.ChangeCharacterEquipList, new List<StorageType>() { StorageType.StorageBoxNormal }))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_STORAGE_CAPACITY_OVER);
            }

            Server.Database.ExecuteInTransaction(connection =>
            {
                queue.AddRange(equipManager.HandleChangeEquipList(
                    Server,
                    client,
                    pawn,
                    request.ChangeCharacterEquipList,
                    ItemNoticeType.ChangeStoragePawnEquip,
                    ItemManager.BoxStorageTypes,
                    connection));
            });

            client.Enqueue(new S2CEquipChangePawnStorageEquipRes()
            {
                PawnId = request.PawnId,
                CharacterEquipList = request.ChangeCharacterEquipList
                // TODO: Unk0
            }, queue);

            return queue;
        }
    }
}

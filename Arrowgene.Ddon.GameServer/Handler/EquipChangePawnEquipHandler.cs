using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangePawnEquipHandler : GameRequestPacketQueueHandler<C2SEquipChangePawnEquipReq, S2CEquipChangePawnEquipRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangePawnEquipHandler));
        
        private readonly EquipManager equipManager;

        public EquipChangePawnEquipHandler(DdonGameServer server) : base(server)
        {
            equipManager = server.EquipManager;
        }

        public override PacketQueue Handle(GameClient client, C2SEquipChangePawnEquipReq request)
        {
            PacketQueue queue = new();
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);

            if (!Server.EquipManager.CanMeetStorageRequirements(Server, client, pawn, request.ChangeCharacterEquipList, new List<StorageType>() { StorageType.ItemBagEquipment }))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_BAG_CAPACITY_OVER);
            }

            Server.Database.ExecuteInTransaction(connection =>
            {
                queue.AddRange(equipManager.HandleChangeEquipList(
                    Server,
                    client,
                    pawn,
                    request.ChangeCharacterEquipList,
                    ItemNoticeType.ChangePawnEquip,
                    new List<StorageType>() { StorageType.ItemBagEquipment },
                    connection));
            });

            client.Enqueue(new S2CEquipChangePawnEquipRes()
            {
                PawnId = request.PawnId,
                CharacterEquipList = request.ChangeCharacterEquipList
                // TODO: Unk0
            }, queue);

            return queue;
        }
    }
}

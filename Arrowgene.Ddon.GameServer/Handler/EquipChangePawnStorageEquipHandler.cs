using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangePawnStorageEquipHandler : GameRequestPacketHandler<C2SEquipChangePawnStorageEquipReq, S2CEquipChangePawnStorageEquipRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangePawnStorageEquipHandler));
        
        private readonly EquipManager equipManager;

        public EquipChangePawnStorageEquipHandler(DdonGameServer server) : base(server)
        {
            equipManager = server.EquipManager;
        }

        public override S2CEquipChangePawnStorageEquipRes Handle(GameClient client, C2SEquipChangePawnStorageEquipReq request)
        {
            (S2CItemUpdateCharacterItemNtc itemNtc, S2CEquipChangePawnEquipNtc equipNtc) equipResult = (null, null);

            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == request.PawnId).Single();

            if (!Server.EquipManager.CanMeetStorageRequirements(Server, client, pawn, request.ChangeCharacterEquipList, new List<StorageType>() { StorageType.StorageBoxNormal }))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_STORAGE_CAPACITY_OVER);
            }

            Server.Database.ExecuteInTransaction(connection =>
            {
                equipResult = ((S2CItemUpdateCharacterItemNtc, S2CEquipChangePawnEquipNtc))
                equipManager.HandleChangeEquipList(
                    Server,
                    client,
                    pawn,
                    request.ChangeCharacterEquipList,
                    ItemNoticeType.ChangeStoragePawnEquip,
                    ItemManager.BoxStorageTypes,
                    connection);
            });

            client.Send(equipResult.itemNtc);

            //Only the party needs to be updated, because only they can see pawns.
            client.Party.SendToAllExcept(equipResult.equipNtc, client);

            return new S2CEquipChangePawnStorageEquipRes()
            {
                PawnId = request.PawnId,
                CharacterEquipList = request.ChangeCharacterEquipList
                // TODO: Unk0
            };
        }
    }
}

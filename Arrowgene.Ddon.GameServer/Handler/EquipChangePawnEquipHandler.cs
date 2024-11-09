using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangePawnEquipHandler : GameRequestPacketHandler<C2SEquipChangePawnEquipReq, S2CEquipChangePawnEquipRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangePawnEquipHandler));
        
        private readonly EquipManager equipManager;

        public EquipChangePawnEquipHandler(DdonGameServer server) : base(server)
        {
            equipManager = server.EquipManager;
        }

        public override S2CEquipChangePawnEquipRes Handle(GameClient client, C2SEquipChangePawnEquipReq request)
        {
            (S2CItemUpdateCharacterItemNtc itemNtc, S2CEquipChangePawnEquipNtc equipNtc) equipResult = (null, null);

            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == request.PawnId).Single();

            if (!Server.EquipManager.CanMeetStorageRequirements(Server, client, pawn, request.ChangeCharacterEquipList, new List<StorageType>() { StorageType.ItemBagEquipment }))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_BAG_CAPACITY_OVER);
            }

            Server.Database.ExecuteInTransaction(connection =>
            {
                equipResult = ((S2CItemUpdateCharacterItemNtc, S2CEquipChangePawnEquipNtc))
                equipManager.HandleChangeEquipList(
                    Server,
                    client,
                    pawn,
                    request.ChangeCharacterEquipList,
                    ItemNoticeType.ChangePawnEquip,
                    new List<StorageType>() { StorageType.ItemBagEquipment },
                    connection);
            });

            client.Send(equipResult.itemNtc);

            //Only the party needs to be updated, because only they can see pawns.
            client.Party.SendToAllExcept(equipResult.equipNtc, client);

            return new S2CEquipChangePawnEquipRes()
            {
                PawnId = request.PawnId,
                CharacterEquipList = request.ChangeCharacterEquipList
                // TODO: Unk0
            };
        }
    }
}

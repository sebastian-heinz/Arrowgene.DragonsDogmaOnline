using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangeCharacterStorageEquipHandler : GameRequestPacketHandler<C2SEquipChangeCharacterStorageEquipReq, S2CEquipChangeCharacterStorageEquipRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangeCharacterStorageEquipHandler));

        private readonly EquipManager equipManager;

        public EquipChangeCharacterStorageEquipHandler(DdonGameServer server) : base(server)
        {
            equipManager = server.EquipManager;
        }

        public override S2CEquipChangeCharacterStorageEquipRes Handle(GameClient client, C2SEquipChangeCharacterStorageEquipReq request)
        {
            (S2CItemUpdateCharacterItemNtc itemNtc, S2CEquipChangeCharacterEquipNtc equipNtc) equipResult = (null, null);

            if (!Server.EquipManager.CanMeetStorageRequirements(Server, client, client.Character, request.ChangeCharacterEquipList, new List<StorageType>() { StorageType.StorageBoxNormal }))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_STORAGE_CAPACITY_OVER);
            }

            Server.Database.ExecuteInTransaction(connection =>
            {
                equipResult = ((S2CItemUpdateCharacterItemNtc, S2CEquipChangeCharacterEquipNtc))
                equipManager.HandleChangeEquipList(
                    Server, 
                    client, 
                    client.Character, 
                    request.ChangeCharacterEquipList, 
                    ItemNoticeType.ChangeStorageEquip, 
                    ItemManager.BoxStorageTypes,
                    connection);
            });

            client.Send(equipResult.itemNtc);

            foreach (Client otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Send(equipResult.equipNtc);
            }

            return new S2CEquipChangeCharacterStorageEquipRes()
            {
                CharacterEquipList = request.ChangeCharacterEquipList
                // TODO: Unk0
            };
        }
    }
}

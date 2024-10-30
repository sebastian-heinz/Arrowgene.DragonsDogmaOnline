using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangeCharacterEquipHandler : GameRequestPacketHandler<C2SEquipChangeCharacterEquipReq, S2CEquipChangeCharacterEquipRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangeCharacterEquipHandler));

        private readonly EquipManager equipManager;

        public EquipChangeCharacterEquipHandler(DdonGameServer server) : base(server)
        {
            equipManager = server.EquipManager;
        }

        public override S2CEquipChangeCharacterEquipRes Handle(GameClient client, C2SEquipChangeCharacterEquipReq request)
        {
            (S2CItemUpdateCharacterItemNtc itemNtc, S2CEquipChangeCharacterEquipNtc equipNtc) equipResult = (null, null);

            if (!Server.EquipManager.CanMeetStorageRequirements(Server, client, client.Character, request.ChangeCharacterEquipList, new List<StorageType>() { StorageType.ItemBagEquipment }))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_BAG_CAPACITY_OVER);
            }

            Server.Database.ExecuteInTransaction(connection =>
            {
                equipResult = ((S2CItemUpdateCharacterItemNtc, S2CEquipChangeCharacterEquipNtc))equipManager.HandleChangeEquipList(
                    Server, client,
                    client.Character,
                    request.ChangeCharacterEquipList,
                    ItemNoticeType.ChangeEquip,
                    new List<StorageType>() { StorageType.ItemBagEquipment },
                    connection);
            });

            client.Send(equipResult.itemNtc);

            foreach (Client otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Send(equipResult.equipNtc); //TODO: Investigate if we need to send this to *everyone*.
            }

            return new S2CEquipChangeCharacterEquipRes()
            {
                CharacterEquipList = request.ChangeCharacterEquipList
                // TODO: Unk0
            };
        }
    }
}

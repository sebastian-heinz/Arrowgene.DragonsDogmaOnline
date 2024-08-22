using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangeCharacterStorageEquipHandler : GameStructurePacketHandler<C2SEquipChangeCharacterStorageEquipReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangeCharacterStorageEquipHandler));

        private readonly EquipManager equipManager;

        public EquipChangeCharacterStorageEquipHandler(DdonGameServer server) : base(server)
        {
            equipManager = server.EquipManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SEquipChangeCharacterStorageEquipReq> packet)
        {
            (S2CItemUpdateCharacterItemNtc itemNtc, S2CEquipChangeCharacterEquipNtc equipNtc) equipResult = (null, null);

            Server.Database.ExecuteInTransaction(connection =>
            {
                equipResult = ((S2CItemUpdateCharacterItemNtc, S2CEquipChangeCharacterEquipNtc))
                equipManager.HandleChangeEquipList(
                    Server, 
                    client, 
                    client.Character, 
                    packet.Structure.ChangeCharacterEquipList, 
                    ItemNoticeType.ChangeStorageEquip, 
                    ItemManager.BoxStorageTypes,
                    connection);
            });

            client.Send(equipResult.itemNtc);

            foreach (Client otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Send(equipResult.equipNtc);
            }

            client.Send(new S2CEquipChangeCharacterStorageEquipRes()
            {
                CharacterEquipList = packet.Structure.ChangeCharacterEquipList
                // TODO: Unk0
            });
        }
    }
}

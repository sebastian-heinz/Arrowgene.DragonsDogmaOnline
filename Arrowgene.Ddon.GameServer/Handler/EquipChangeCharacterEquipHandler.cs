using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangeCharacterEquipHandler : GameStructurePacketHandler<C2SEquipChangeCharacterEquipReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangeCharacterEquipHandler));

        private readonly EquipManager equipManager;

        public EquipChangeCharacterEquipHandler(DdonGameServer server) : base(server)
        {
            equipManager = server.EquipManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SEquipChangeCharacterEquipReq> packet)
        {
            (S2CItemUpdateCharacterItemNtc itemNtc, S2CEquipChangeCharacterEquipNtc equipNtc) equipResult = (null, null);

            Server.Database.ExecuteInTransaction(connection =>
            {
                equipResult = ((S2CItemUpdateCharacterItemNtc, S2CEquipChangeCharacterEquipNtc))equipManager.HandleChangeEquipList(
                    Server, client, 
                    client.Character, 
                    packet.Structure.ChangeCharacterEquipList, 
                    ItemNoticeType.ChangeEquip, 
                    new List<StorageType>() { StorageType.ItemBagEquipment }, 
                    connection);
            });

            client.Send(equipResult.itemNtc);

            foreach (Client otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Send(equipResult.equipNtc); //TODO: Investigate if we need to send this to *everyone*.
            }

            client.Send(new S2CEquipChangeCharacterEquipRes()
            {
                CharacterEquipList = packet.Structure.ChangeCharacterEquipList
                // TODO: Unk0
            });
        }
    }

}

using System.Collections.Generic;
using Arrowgene.Ddon.Database.Deferred;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
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
            Server.Database.ExecuteInTransaction(connection =>
            {
                equipManager.HandleChangeEquipList(
                    Server, client, 
                    client.Character, 
                    packet.Structure.ChangeCharacterEquipList, 
                    ItemNoticeType.ChangeEquip, 
                    new List<StorageType>() { StorageType.ItemBagEquipment }, () => {
                    client.Send(new S2CEquipChangeCharacterEquipRes()
                        {
                            CharacterEquipList = packet.Structure.ChangeCharacterEquipList
                            // TODO: Unk0
                        });
                    }, connection);
            });
        }
    }

}

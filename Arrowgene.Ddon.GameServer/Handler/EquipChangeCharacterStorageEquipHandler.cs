using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangeCharacterStorageEquipHandler : GameStructurePacketHandler<C2SEquipChangeCharacterStorageEquipReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangeCharacterStorageEquipHandler));

        public EquipChangeCharacterStorageEquipHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SEquipChangeCharacterStorageEquipReq> packet)
        {
            EquipChangeCharacterEquipHandler.HandleChangeCharacterEquipList(Server, client, packet.Structure.ChangeCharacterEquipList, 0x26, StorageType.StorageBoxNormal, () => {
                client.Send(new S2CEquipChangeCharacterStorageEquipRes()
                {
                    CharacterEquipList = packet.Structure.ChangeCharacterEquipList
                });
            });

        }
    }
}
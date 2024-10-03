using System.Linq;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipGetCharacterEquipListHandler : GameStructurePacketHandler<C2SEquipGetCharacterEquipListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipGetCharacterEquipListHandler));


        public EquipGetCharacterEquipListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SEquipGetCharacterEquipListReq> packet)
        {
            // TODO: Figure out if it should send all equips or just the ones for the current job
            client.Send(new S2CEquipGetCharacterEquipListRes()
            {
                CharacterEquipList = client.Character.Equipment.AsCDataCharacterEquipInfo(EquipType.Performance)
                    .Union(client.Character.Equipment.AsCDataCharacterEquipInfo(EquipType.Visual))
                    .ToList(),
                EquipJobItemList = client.Character.EquipmentTemplate.JobItemsAsCDataEquipJobItem(client.Character.Job),
            });
        }
    }
}

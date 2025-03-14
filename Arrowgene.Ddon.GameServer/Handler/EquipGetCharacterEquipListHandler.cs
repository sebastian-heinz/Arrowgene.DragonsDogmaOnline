using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipGetCharacterEquipListHandler : GameRequestPacketHandler<C2SEquipGetCharacterEquipListReq, S2CEquipGetCharacterEquipListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipGetCharacterEquipListHandler));

        public EquipGetCharacterEquipListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEquipGetCharacterEquipListRes Handle(GameClient client, C2SEquipGetCharacterEquipListReq request)
        {
            // TODO: Figure out if it should send all equips or just the ones for the current job
            return new S2CEquipGetCharacterEquipListRes()
            {
                CharacterEquipList = client.Character.Equipment.AsCDataCharacterEquipInfo(EquipType.Performance)
                    .Union(client.Character.Equipment.AsCDataCharacterEquipInfo(EquipType.Visual))
                    .ToList(),
                EquipJobItemList = client.Character.EquipmentTemplate.JobItemsAsCDataEquipJobItem(client.Character.Job),
            };
        }
    }
}

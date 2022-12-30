using System.Linq;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
                CharacterEquipList = client.Character.CharacterEquipViewItemListDictionary[client.Character.Job]
                    .Union(client.Character.CharacterEquipItemListDictionary[client.Character.Job])
                    .Select(x => new CDataCharacterEquipInfo()
                    {
                        EquipItemUId = x.EquipItemUId,
                        EquipCategory = (byte) x.EquipSlot,
                        EquipType = x.EquipType
                    })
                    .ToList(),
                EquipJobItemList = client.Character.CharacterEquipJobItemListDictionary[client.Character.Job],
                // TODO: PawnEquipItemList
            });
        }
    }
}

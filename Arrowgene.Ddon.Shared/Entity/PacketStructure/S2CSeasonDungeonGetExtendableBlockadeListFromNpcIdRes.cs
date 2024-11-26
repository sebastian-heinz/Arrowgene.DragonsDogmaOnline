using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonGetExtendableBlockadeListFromNpcIdRes : ServerResponse
    {
        public S2CSeasonDungeonGetExtendableBlockadeListFromNpcIdRes()
        {
            BlockadeList = new List<CDataSeasonDungeonBlockadeElement>();
        }

        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_GET_EXTENDABLE_BLOCKADE_LIST_FROM_NPC_ID_RES;

        public List<CDataSeasonDungeonBlockadeElement> BlockadeList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonGetExtendableBlockadeListFromNpcIdRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonGetExtendableBlockadeListFromNpcIdRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.BlockadeList);
            }

            public override S2CSeasonDungeonGetExtendableBlockadeListFromNpcIdRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonGetExtendableBlockadeListFromNpcIdRes obj = new S2CSeasonDungeonGetExtendableBlockadeListFromNpcIdRes();
                ReadServerResponse(buffer, obj);
                obj.BlockadeList = ReadEntityList<CDataSeasonDungeonBlockadeElement>(buffer);
                return obj;
            }
        }
    }
}

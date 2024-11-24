using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonGetBlockadeIdFromOmReq : IPacketStructure
    {
        public C2SSeasonDungeonGetBlockadeIdFromOmReq()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_OM_REQ;

        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonGetBlockadeIdFromOmReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonGetBlockadeIdFromOmReq obj)
            {
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
            }

            public override C2SSeasonDungeonGetBlockadeIdFromOmReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonGetBlockadeIdFromOmReq obj = new C2SSeasonDungeonGetBlockadeIdFromOmReq();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

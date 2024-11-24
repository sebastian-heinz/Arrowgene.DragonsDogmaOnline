using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonGetSoulOrdealListfromOmReq : IPacketStructure
    {
        public C2SSeasonDungeonGetSoulOrdealListfromOmReq()
        {
            StageLayoutId = new CDataStageLayoutId();
        }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_GET_SOUL_ORDEAL_LIST_FROM_OM_REQ;

        public CDataStageLayoutId StageLayoutId { get; set; }
        public uint PosId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonGetSoulOrdealListfromOmReq>
        {


            public override void Write(IBuffer buffer, C2SSeasonDungeonGetSoulOrdealListfromOmReq obj)
            {
                WriteEntity(buffer, obj.StageLayoutId);
                WriteUInt32(buffer, obj.PosId);
            }

            public override C2SSeasonDungeonGetSoulOrdealListfromOmReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonGetSoulOrdealListfromOmReq obj = new C2SSeasonDungeonGetSoulOrdealListfromOmReq();
                obj.StageLayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

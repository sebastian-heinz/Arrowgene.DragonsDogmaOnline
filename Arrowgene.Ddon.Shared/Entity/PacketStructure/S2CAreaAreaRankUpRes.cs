using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CAreaAreaRankUpRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_AREA_AREA_RANK_UP_RES;

        public S2CAreaAreaRankUpRes()
        {
            ReleaseList = new();
            ReleaseWarpList = new();
        }

        public uint AreaId { get; set; }
        public uint AreaRank { get; set; }
        public uint AreaPoint { get; set; }
        public uint NextAreaPoint { get; set; }
        public List<CDataCommonU32> ReleaseList { get; set; }
        public List<CDataCommonU32> ReleaseWarpList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CAreaAreaRankUpRes>
        {
            public override void Write(IBuffer buffer, S2CAreaAreaRankUpRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.AreaId);
                WriteUInt32(buffer, obj.AreaRank);
                WriteUInt32(buffer, obj.AreaPoint);
                WriteUInt32(buffer, obj.NextAreaPoint);
                WriteEntityList(buffer, obj.ReleaseList);
                WriteEntityList(buffer, obj.ReleaseWarpList);
            }

            public override S2CAreaAreaRankUpRes Read(IBuffer buffer)
            {
                S2CAreaAreaRankUpRes obj = new S2CAreaAreaRankUpRes();
                ReadServerResponse(buffer, obj);
                obj.AreaId = ReadUInt32(buffer);
                obj.AreaRank = ReadUInt32(buffer);
                obj.AreaPoint = ReadUInt32(buffer);
                obj.NextAreaPoint = ReadUInt32(buffer);
                obj.ReleaseList = ReadEntityList<CDataCommonU32>(buffer);
                obj.ReleaseWarpList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}

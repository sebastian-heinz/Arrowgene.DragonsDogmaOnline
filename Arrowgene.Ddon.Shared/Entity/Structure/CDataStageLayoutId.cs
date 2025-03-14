using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    /// <summary>
    /// CStageLayoutID
    /// </summary>
    public class CDataStageLayoutId : IEquatable<CDataStageLayoutId>
    {
        public uint StageId { get; set; }
        public byte LayerNo { get; set; }
        public uint GroupId { get; set; }

        public CDataStageLayoutId() {
            StageId = 0;
            LayerNo = 0;
            GroupId = 0;
        }

        public CDataStageLayoutId(uint stageId, byte layerNo, uint groupId)
        {
            StageId = stageId;
            LayerNo = layerNo;
            GroupId = groupId;
        }

        public StageLayoutId AsStageLayoutId()
        {
            return new StageLayoutId(this);
        }

        public class Serializer : EntitySerializer<CDataStageLayoutId>
        {
            public override void Write(IBuffer buffer, CDataStageLayoutId obj)
            {
                WriteUInt32(buffer, obj.StageId);
                WriteByte(buffer, obj.LayerNo);
                WriteUInt32(buffer, obj.GroupId);
            }

            public override CDataStageLayoutId Read(IBuffer buffer)
            {
                CDataStageLayoutId obj = new CDataStageLayoutId();
                obj.StageId = ReadUInt32(buffer);
                obj.LayerNo = ReadByte(buffer);
                obj.GroupId = ReadUInt32(buffer);
                return obj;
            }
        }

        public override string ToString() 
        { 
            return $"{StageId}.{LayerNo}.{GroupId}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StageId, LayerNo, GroupId);
        }

        public bool Equals(CDataStageLayoutId other)
        {
            return StageId == other.StageId && LayerNo == other.LayerNo && GroupId == other.GroupId;
        }
    }
}

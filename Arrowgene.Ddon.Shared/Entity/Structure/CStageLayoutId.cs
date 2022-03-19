using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    /// <summary>
    /// CStageLayoutID
    /// </summary>
    public class CStageLayoutId
    {
        public uint StageId { get; set; }
        public byte LayerNo { get; set; }
        public uint GroupId { get; set; }

        public CStageLayoutId() {
            StageId = 0;
            LayerNo = 0;
            GroupId = 0;
        }

        public class Serializer : EntitySerializer<CStageLayoutId>
        {
            public override void Write(IBuffer buffer, CStageLayoutId obj)
            {
                WriteUInt32(buffer, obj.StageId);
                WriteByte(buffer, obj.LayerNo);
                WriteUInt32(buffer, obj.GroupId);
            }

            public override CStageLayoutId Read(IBuffer buffer)
            {
                CStageLayoutId obj = new CStageLayoutId();
                obj.StageId = ReadUInt32(buffer);
                obj.LayerNo = ReadByte(buffer);
                obj.GroupId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

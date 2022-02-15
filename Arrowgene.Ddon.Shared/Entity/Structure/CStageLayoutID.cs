using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    /// <summary>
    /// CStageLayoutID
    /// </summary>
    public class CStageLayoutID
    {
        public uint StageId { get; set; }
        public byte LayerNo { get; set; }
        public uint GroupId { get; set; }

        public CStageLayoutID() {
            StageId = 0;
            LayerNo = 0;
            GroupId = 0;
        }

        public class Serializer : EntitySerializer<CStageLayoutID>
        {
            public override void Write(IBuffer buffer, CStageLayoutID obj)
            {
                WriteUInt32(buffer, obj.StageId);
                WriteByte(buffer, obj.LayerNo);
                WriteUInt32(buffer, obj.GroupId);
            }

            public override CStageLayoutID Read(IBuffer buffer)
            {
                CStageLayoutID obj = new CStageLayoutID();
                obj.StageId = ReadUInt32(buffer);
                obj.LayerNo = ReadByte(buffer);
                obj.GroupId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

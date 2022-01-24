using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CStageLayoutID
    {
        public uint StageID { get; set; }
        public byte LayerNo { get; set; }
        public uint GroupID { get; set; }

        public CStageLayoutID() {
            StageID = 0;
            LayerNo = 0;
            GroupID = 0;
        }

        public class Serializer : EntitySerializer<CStageLayoutID>
        {
            public override void Write(IBuffer buffer, CStageLayoutID obj)
            {
                WriteUInt32(buffer, obj.StageID);
                WriteByte(buffer, obj.LayerNo);
                WriteUInt32(buffer, obj.GroupID);
            }

            public override CStageLayoutID Read(IBuffer buffer)
            {
                CStageLayoutID obj = new CStageLayoutID();
                obj.StageID = ReadUInt32(buffer);
                obj.LayerNo = ReadByte(buffer);
                obj.GroupID = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
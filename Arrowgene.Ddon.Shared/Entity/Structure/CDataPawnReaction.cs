using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnReaction
    {
        public byte ReactionType { get; set; }
        public uint MotionNo { get; set; }

        public class Serializer : EntitySerializer<CDataPawnReaction>
        {
            public override void Write(IBuffer buffer, CDataPawnReaction obj)
            {
                WriteByte(buffer, obj.ReactionType);
                WriteUInt32(buffer, obj.MotionNo);
            }

            public override CDataPawnReaction Read(IBuffer buffer)
            {
                CDataPawnReaction obj = new CDataPawnReaction();
                obj.ReactionType = ReadByte(buffer);
                obj.MotionNo = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
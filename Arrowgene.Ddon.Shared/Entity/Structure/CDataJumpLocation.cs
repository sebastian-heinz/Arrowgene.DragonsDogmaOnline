using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJumpLocation
    {
        public CDataJumpLocation() {
            stageId = 0;
            startPos = 0;
        }

        public uint stageId;
        public uint startPos;

        public class Serializer : EntitySerializer<CDataJumpLocation>
        {
            public override void Write(IBuffer buffer, CDataJumpLocation obj)
            {
                WriteUInt32(buffer, obj.stageId);
                WriteUInt32(buffer, obj.startPos);
            }

            public override CDataJumpLocation Read(IBuffer buffer)
            {
                CDataJumpLocation obj = new CDataJumpLocation();
                obj.stageId = ReadUInt32(buffer);
                obj.startPos = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

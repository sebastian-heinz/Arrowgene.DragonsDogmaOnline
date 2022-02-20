using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SStageAreaChangeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_STAGE_AREA_CHANGE_REQ;

        public uint StageId { get; set; }
        public uint JumpType { get; set; }

        public C2SStageAreaChangeReq() {
            StageId=0;
            JumpType=0;
        }

        public class Serializer : PacketEntitySerializer<C2SStageAreaChangeReq>
        {
            public override void Write(IBuffer buffer, C2SStageAreaChangeReq obj)
            {
                WriteUInt32(buffer, obj.StageId);
                WriteUInt32(buffer, obj.JumpType);
            }

            public override C2SStageAreaChangeReq Read(IBuffer buffer)
            {
                C2SStageAreaChangeReq obj = new C2SStageAreaChangeReq();
                obj.StageId = ReadUInt32(buffer);
                obj.JumpType = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

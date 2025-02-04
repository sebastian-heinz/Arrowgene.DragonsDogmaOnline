using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SStageGetSpAreaChangeInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_STAGE_GET_SP_AREA_CHANGE_INFO_REQ;

        public C2SStageGetSpAreaChangeInfoReq()
        {
        }

        public uint StageId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SStageGetSpAreaChangeInfoReq>
        {
            public override void Write(IBuffer buffer, C2SStageGetSpAreaChangeInfoReq obj)
            {
                WriteUInt32(buffer, obj.StageId);
            }

            public override C2SStageGetSpAreaChangeInfoReq Read(IBuffer buffer)
            {
                C2SStageGetSpAreaChangeInfoReq obj = new C2SStageGetSpAreaChangeInfoReq();
                obj.StageId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

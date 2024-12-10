using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SStageGetSpAreaChangeIdFromNpcIdReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_STAGE_GET_SP_AREA_CHANGE_ID_FROM_NPC_ID_REQ;

        public C2SStageGetSpAreaChangeIdFromNpcIdReq()
        {
        }

        public NpcId NpcId {get; set;}

        public class Serializer : PacketEntitySerializer<C2SStageGetSpAreaChangeIdFromNpcIdReq>
        {
            public override void Write(IBuffer buffer, C2SStageGetSpAreaChangeIdFromNpcIdReq obj)
            {
                WriteUInt32(buffer, (uint) obj.NpcId);
            }

            public override C2SStageGetSpAreaChangeIdFromNpcIdReq Read(IBuffer buffer)
            {
                C2SStageGetSpAreaChangeIdFromNpcIdReq obj = new C2SStageGetSpAreaChangeIdFromNpcIdReq();
                obj.NpcId = (NpcId)ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

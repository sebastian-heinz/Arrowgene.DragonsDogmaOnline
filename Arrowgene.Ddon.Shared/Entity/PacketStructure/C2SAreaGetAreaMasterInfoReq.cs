using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SAreaGetAreaMasterInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_AREA_GET_AREA_MASTER_INFO_REQ;

        public QuestAreaId AreaId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SAreaGetAreaMasterInfoReq>
        {
            public override void Write(IBuffer buffer, C2SAreaGetAreaMasterInfoReq obj)
            {
                WriteUInt32(buffer, (uint) obj.AreaId);
            }

            public override C2SAreaGetAreaMasterInfoReq Read(IBuffer buffer)
            {
                C2SAreaGetAreaMasterInfoReq obj = new C2SAreaGetAreaMasterInfoReq();
                obj.AreaId = (QuestAreaId) ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

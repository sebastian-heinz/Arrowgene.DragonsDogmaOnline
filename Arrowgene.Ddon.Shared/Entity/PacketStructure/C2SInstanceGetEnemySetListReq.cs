using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceGetEnemySetListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_GET_ENEMY_SET_LIST_REQ;

        public C2SInstanceGetEnemySetListReq()
        {
            LayoutId = new CDataStageLayoutId();
            SubGroupId = 0;
        }

        public CDataStageLayoutId LayoutId { get; set; }
        public byte SubGroupId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SInstanceGetEnemySetListReq>
        {

            public override void Write(IBuffer buffer, C2SInstanceGetEnemySetListReq obj)
            {
                WriteEntity<CDataStageLayoutId>(buffer, obj.LayoutId);
                WriteByte(buffer, obj.SubGroupId);
            }

            public override C2SInstanceGetEnemySetListReq Read(IBuffer buffer)
            {
                C2SInstanceGetEnemySetListReq obj = new C2SInstanceGetEnemySetListReq();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.SubGroupId = ReadByte(buffer);
                return obj;
            }
        }
    }
}

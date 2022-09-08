using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceEnemyGroupLeaveNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_ENEMY_GROUP_LEAVE_NTC;

        public C2SInstanceEnemyGroupLeaveNtc()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public CDataStageLayoutId LayoutId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SInstanceEnemyGroupLeaveNtc>
        {

            public override void Write(IBuffer buffer, C2SInstanceEnemyGroupLeaveNtc obj)
            {
                WriteEntity<CDataStageLayoutId>(buffer, obj.LayoutId);
            }

            public override C2SInstanceEnemyGroupLeaveNtc Read(IBuffer buffer)
            {
                C2SInstanceEnemyGroupLeaveNtc obj = new C2SInstanceEnemyGroupLeaveNtc();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                return obj;
            }
        }
    }
}
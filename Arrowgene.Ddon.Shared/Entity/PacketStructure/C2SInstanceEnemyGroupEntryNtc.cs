using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceEnemyGroupEntryNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_ENEMY_GROUP_ENTRY_NTC;

        public C2SInstanceEnemyGroupEntryNtc()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public CDataStageLayoutId LayoutId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SInstanceEnemyGroupEntryNtc>
        {

            public override void Write(IBuffer buffer, C2SInstanceEnemyGroupEntryNtc obj)
            {
                WriteEntity<CDataStageLayoutId>(buffer, obj.LayoutId);
            }

            public override C2SInstanceEnemyGroupEntryNtc Read(IBuffer buffer)
            {
                C2SInstanceEnemyGroupEntryNtc obj = new C2SInstanceEnemyGroupEntryNtc();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                return obj;
            }
        }
    }
}
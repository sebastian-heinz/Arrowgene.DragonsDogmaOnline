using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceEnemyGroupResetNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INSTANCE_ENEMY_GROUP_RESET_NTC;

        public S2CInstanceEnemyGroupResetNtc()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public CDataStageLayoutId LayoutId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInstanceEnemyGroupResetNtc>
        {
            public override void Write(IBuffer buffer, S2CInstanceEnemyGroupResetNtc obj)
            {
                WriteEntity<CDataStageLayoutId>(buffer, obj.LayoutId);
            }

            public override S2CInstanceEnemyGroupResetNtc Read(IBuffer buffer)
            {
                S2CInstanceEnemyGroupResetNtc obj = new S2CInstanceEnemyGroupResetNtc();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                return obj;
            }
        }
    }
}

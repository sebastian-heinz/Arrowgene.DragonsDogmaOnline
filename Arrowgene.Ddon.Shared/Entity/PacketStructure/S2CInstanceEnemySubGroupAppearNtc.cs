using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceEnemySubGroupAppearNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INSTANCE_ENEMY_SUB_GROUP_APPEAR_NTC;

        public S2CInstanceEnemySubGroupAppearNtc()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public CDataStageLayoutId LayoutId { get; set; }
        public byte SubGroupId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInstanceEnemySubGroupAppearNtc>
        {
            public override void Write(IBuffer buffer, S2CInstanceEnemySubGroupAppearNtc obj)
            {
                WriteEntity<CDataStageLayoutId>(buffer, obj.LayoutId);
                WriteByte(buffer, obj.SubGroupId);
            }

            public override S2CInstanceEnemySubGroupAppearNtc Read(IBuffer buffer)
            {
                S2CInstanceEnemySubGroupAppearNtc obj = new S2CInstanceEnemySubGroupAppearNtc();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.SubGroupId = ReadByte(buffer);
                return obj;
            }
        }
    }
}
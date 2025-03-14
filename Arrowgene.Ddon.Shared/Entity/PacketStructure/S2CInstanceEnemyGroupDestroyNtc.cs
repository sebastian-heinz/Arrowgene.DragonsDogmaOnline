using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceEnemyGroupDestroyNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INSTANCE_ENEMY_GROUP_DESTROY_NTC;

        public CDataStageLayoutId LayoutId { get; set; } = new();
        public bool IsAreaBoss { get; set; }


        public class Serializer : PacketEntitySerializer<S2CInstanceEnemyGroupDestroyNtc>
        {
            public override void Write(IBuffer buffer, S2CInstanceEnemyGroupDestroyNtc obj)
            {
                WriteEntity<CDataStageLayoutId>(buffer, obj.LayoutId);
                WriteBool(buffer, obj.IsAreaBoss);
            }

            public override S2CInstanceEnemyGroupDestroyNtc Read(IBuffer buffer)
            {
                S2CInstanceEnemyGroupDestroyNtc obj = new S2CInstanceEnemyGroupDestroyNtc();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.IsAreaBoss = ReadBool(buffer);
                return obj;
            }
        }
    }
}

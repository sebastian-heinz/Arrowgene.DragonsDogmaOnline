using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceEnemyDieNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INSTANCE_ENEMY_DIE_NTC;

        public CDataStageLayoutId LayoutId { get; set; } = new();
        public uint SetId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInstanceEnemyDieNtc>
        {
            public override void Write(IBuffer buffer, S2CInstanceEnemyDieNtc obj)
            {
                WriteEntity<CDataStageLayoutId>(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.SetId);
            }

            public override S2CInstanceEnemyDieNtc Read(IBuffer buffer)
            {
                S2CInstanceEnemyDieNtc obj = new S2CInstanceEnemyDieNtc();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.SetId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}


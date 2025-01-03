using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceGatheringEnemyAppearNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INSTANCE_GATHERING_ENEMY_APPEAR_NTC;

        public S2CInstanceGatheringEnemyAppearNtc()
        {
            GatheringLayoutId = new CDataStageLayoutId();
            EnemyLayoutId = new CDataStageLayoutId();
        }

        public CDataStageLayoutId GatheringLayoutId { get; set; }
        public uint GatheringPosId { get; set; }
        public CDataStageLayoutId EnemyLayoutId { get; set; }
        public uint EnemyPosId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInstanceGatheringEnemyAppearNtc>
        {
            public override void Write(IBuffer buffer, S2CInstanceGatheringEnemyAppearNtc obj)
            {
                WriteEntity(buffer, obj.GatheringLayoutId);
                WriteUInt32(buffer, obj.GatheringPosId);
                WriteEntity(buffer, obj.EnemyLayoutId);
                WriteUInt32(buffer, obj.EnemyPosId);
            }

            public override S2CInstanceGatheringEnemyAppearNtc Read(IBuffer buffer)
            {
                S2CInstanceGatheringEnemyAppearNtc obj = new S2CInstanceGatheringEnemyAppearNtc();
                obj.GatheringLayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.GatheringPosId = ReadUInt32(buffer);
                obj.EnemyLayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.EnemyPosId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

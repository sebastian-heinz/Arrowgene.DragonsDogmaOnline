using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceEnemyStageBossAnnihilateNtc : IPacketStructure
    {
        public S2CInstanceEnemyStageBossAnnihilateNtc()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public CDataStageLayoutId LayoutId { get; set; }

        public PacketId Id => PacketId.S2C_INSTANCE_ENEMY_STAGE_BOSS_ANNIHILATE_NTC;

        public class Serializer : PacketEntitySerializer<S2CInstanceEnemyStageBossAnnihilateNtc> {
            public override void Write(IBuffer buffer, S2CInstanceEnemyStageBossAnnihilateNtc obj)
            {
                WriteEntity(buffer, obj.LayoutId);
            }

            public override S2CInstanceEnemyStageBossAnnihilateNtc Read(IBuffer buffer)
            {
                var obj = new S2CInstanceEnemyStageBossAnnihilateNtc();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                return obj;
            }
        }
    }
}

using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceEnemyBadStatusStartNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_ENEMY_BAD_STATUS_START_NTC;

        public C2SInstanceEnemyBadStatusStartNtc()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }
        public uint Unk4 { get; set; }
        public uint Unk5 { get; set; }


        public class Serializer : PacketEntitySerializer<C2SInstanceEnemyBadStatusStartNtc>
        {
            public override void Write(IBuffer buffer, C2SInstanceEnemyBadStatusStartNtc obj)
            {
                WriteEntity<CDataStageLayoutId>(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
                WriteUInt32(buffer, obj.Unk4);
                WriteUInt32(buffer, obj.Unk5);
            }

            public override C2SInstanceEnemyBadStatusStartNtc Read(IBuffer buffer)
            {
                C2SInstanceEnemyBadStatusStartNtc obj = new C2SInstanceEnemyBadStatusStartNtc();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                obj.Unk5 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

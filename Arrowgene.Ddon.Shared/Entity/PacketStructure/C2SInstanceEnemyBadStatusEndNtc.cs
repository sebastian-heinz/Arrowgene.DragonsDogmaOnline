using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceEnemyBadStatusEndNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_ENEMY_BAD_STATUS_END_NTC;

        public C2SInstanceEnemyBadStatusEndNtc()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }
        public uint Unk4 { get; set; }
        public uint Unk5 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SInstanceEnemyBadStatusEndNtc>
        {
            public override void Write(IBuffer buffer, C2SInstanceEnemyBadStatusEndNtc obj)
            {
                WriteEntity<CDataStageLayoutId>(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
                WriteUInt32(buffer, obj.Unk4);
                WriteUInt32(buffer, obj.Unk5);
            }

            public override C2SInstanceEnemyBadStatusEndNtc Read(IBuffer buffer)
            {
                C2SInstanceEnemyBadStatusEndNtc obj = new C2SInstanceEnemyBadStatusEndNtc();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                obj.Unk5 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

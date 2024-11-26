using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2S_SEASON_DUNGEON_62_12_16_NTC : IPacketStructure
    {
        public C2S_SEASON_DUNGEON_62_12_16_NTC()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_62_12_16_NTC;

        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }

        public class Serializer : PacketEntitySerializer<C2S_SEASON_DUNGEON_62_12_16_NTC>
        {
            public override void Write(IBuffer buffer, C2S_SEASON_DUNGEON_62_12_16_NTC obj)
            {
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
            }

            public override C2S_SEASON_DUNGEON_62_12_16_NTC Read(IBuffer buffer)
            {
                C2S_SEASON_DUNGEON_62_12_16_NTC obj = new C2S_SEASON_DUNGEON_62_12_16_NTC();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

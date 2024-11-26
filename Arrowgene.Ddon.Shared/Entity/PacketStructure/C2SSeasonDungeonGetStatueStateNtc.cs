using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonGetStatueStateNtc : IPacketStructure
    {
        public C2SSeasonDungeonGetStatueStateNtc()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_GET_STATUE_STATE_NTC;

        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }
        
        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonGetStatueStateNtc>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonGetStatueStateNtc obj)
            {
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
            }

            public override C2SSeasonDungeonGetStatueStateNtc Read(IBuffer buffer)
            {
                C2SSeasonDungeonGetStatueStateNtc obj = new C2SSeasonDungeonGetStatueStateNtc();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

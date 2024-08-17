using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftCraftRankUpNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CRAFT_CRAFT_RANK_UP_NTC;

        public S2CCraftCraftRankUpNtc()
        {
        }

        public uint PawnId { get; set; }
        public uint CraftRank { get; set; }
        public uint AddCraftPoints { get; set; }
        public uint TotalCraftPoint { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCraftCraftRankUpNtc>
        {
            public override void Write(IBuffer buffer, S2CCraftCraftRankUpNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.CraftRank);
                WriteUInt32(buffer, obj.AddCraftPoints);
                WriteUInt32(buffer, obj.TotalCraftPoint);
            }

            public override S2CCraftCraftRankUpNtc Read(IBuffer buffer)
            {
                S2CCraftCraftRankUpNtc obj = new S2CCraftCraftRankUpNtc();
                obj.PawnId = ReadUInt32(buffer);
                obj.CraftRank = ReadUInt32(buffer);
                obj.AddCraftPoints = ReadUInt32(buffer);
                obj.TotalCraftPoint = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

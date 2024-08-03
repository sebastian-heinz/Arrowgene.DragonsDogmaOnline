using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftCraftExpUpNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CRAFT_CRAFT_EXP_UP_NTC;

        public S2CCraftCraftExpUpNtc()
        {
        }

        public uint PawnId { get; set; }
        public uint AddExp {  get; set; }
        public uint ExtraBonusExp {  get; set; }
        public uint TotalExp { get; set; }
        public uint CraftRankLimit { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCraftCraftExpUpNtc>
        {
            public override void Write(IBuffer buffer, S2CCraftCraftExpUpNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.AddExp);
                WriteUInt32(buffer, obj.ExtraBonusExp);
                WriteUInt32(buffer, obj.TotalExp);
                WriteUInt32(buffer, obj.CraftRankLimit);
            }

            public override S2CCraftCraftExpUpNtc Read(IBuffer buffer)
            {
                S2CCraftCraftExpUpNtc obj = new S2CCraftCraftExpUpNtc();
                obj.PawnId = ReadUInt32(buffer);
                obj.AddExp = ReadUInt32(buffer);
                obj.ExtraBonusExp = ReadUInt32(buffer);
                obj.TotalExp = ReadUInt32(buffer);
                obj.CraftRankLimit = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}


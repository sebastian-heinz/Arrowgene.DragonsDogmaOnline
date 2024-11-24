using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSeasonDungeonRewardItemViewEntry
    {
        public CDataSeasonDungeonRewardItemViewEntry()
        {
        }

        public uint ItemId { get; set; }
        public uint MinAmount { get; set; }
        public uint MaxAmount { get; set; }

        public class Serializer : EntitySerializer<CDataSeasonDungeonRewardItemViewEntry>
        {
            public override void Write(IBuffer buffer, CDataSeasonDungeonRewardItemViewEntry obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.MinAmount);
                WriteUInt32(buffer, obj.MaxAmount);
            }

            public override CDataSeasonDungeonRewardItemViewEntry Read(IBuffer buffer)
            {
                CDataSeasonDungeonRewardItemViewEntry obj = new CDataSeasonDungeonRewardItemViewEntry();
                obj.ItemId = ReadUInt32(buffer);
                obj.MinAmount = ReadUInt32(buffer);
                obj.MaxAmount = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

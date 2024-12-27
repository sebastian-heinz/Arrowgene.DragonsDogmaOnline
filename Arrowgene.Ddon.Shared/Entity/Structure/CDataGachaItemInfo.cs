using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGachaItemInfo
    {
        public uint ItemId { get; set; }
        public uint ItemNum { get; set; }
        public uint Rank { get; set; }
        public uint Effect { get; set; }
        public double Probability { get; set; }

        public CDataGachaItemInfo()
        {
        }

        public class Serializer : EntitySerializer<CDataGachaItemInfo>
        {
            public override void Write(IBuffer buffer, CDataGachaItemInfo obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.ItemNum);
                WriteUInt32(buffer, obj.Rank);
                WriteUInt32(buffer, obj.Effect);
                WriteDouble(buffer, obj.Probability);
            }

            public override CDataGachaItemInfo Read(IBuffer buffer)
            {
                CDataGachaItemInfo obj = new CDataGachaItemInfo
                {
                    ItemId = ReadUInt32(buffer),
                    ItemNum = ReadUInt32(buffer),
                    Rank = ReadUInt32(buffer),
                    Effect = ReadUInt32(buffer),
                    Probability = ReadDouble(buffer)
                };

                return obj;
            }
        }
    }
}

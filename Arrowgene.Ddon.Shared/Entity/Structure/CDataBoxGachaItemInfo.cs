using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBoxGachaItemInfo
    {
        public uint ItemId { get; set; }
        public uint ItemNum { get; set; }
        public uint ItemStock { get; set; }
        public uint Rank { get; set; }
        public uint Effect { get; set; }
        public double Probability { get; set; }
        public ushort DrawNum { get; set; }

        public CDataBoxGachaItemInfo()
        {
        }

        public class Serializer : EntitySerializer<CDataBoxGachaItemInfo>
        {
            public override void Write(IBuffer buffer, CDataBoxGachaItemInfo obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.ItemNum);
                WriteUInt32(buffer, obj.ItemStock);
                WriteUInt32(buffer, obj.Rank);
                WriteUInt32(buffer, obj.Effect);
                WriteDouble(buffer, obj.Probability);
                WriteUInt16(buffer, obj.DrawNum);
            }

            public override CDataBoxGachaItemInfo Read(IBuffer buffer)
            {
                CDataBoxGachaItemInfo obj = new CDataBoxGachaItemInfo
                {
                    ItemId = ReadUInt32(buffer),
                    ItemNum = ReadUInt32(buffer),
                    ItemStock = ReadUInt32(buffer),
                    Rank = ReadUInt32(buffer),
                    Effect = ReadUInt32(buffer),
                    Probability = ReadDouble(buffer),
                    DrawNum = ReadUInt16(buffer)
                };

                return obj;
            }
        }
    }
}

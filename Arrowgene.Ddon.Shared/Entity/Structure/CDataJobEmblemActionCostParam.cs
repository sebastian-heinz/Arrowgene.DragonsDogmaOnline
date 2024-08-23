using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobEmblemActionCostParam
    {
        public uint Unk0 { get; set; } // 0 = Wallet, 3 = Exp Type
        public byte PointType { get; set; } // Wallet Type or Exp Type
        public uint Amount { get; set; }

        public class Serializer : EntitySerializer<CDataJobEmblemActionCostParam>
        {
            public override void Write(IBuffer buffer, CDataJobEmblemActionCostParam obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, (byte) obj.PointType);
                WriteUInt32(buffer, obj.Amount);
            }

            public override CDataJobEmblemActionCostParam Read(IBuffer buffer)
            {
                CDataJobEmblemActionCostParam obj = new CDataJobEmblemActionCostParam();
                obj.Unk0 = ReadUInt32(buffer);
                obj.PointType = ReadByte(buffer);
                obj.Amount = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}


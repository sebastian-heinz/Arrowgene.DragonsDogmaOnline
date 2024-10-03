using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStampBonus
    {
        /// <summary>
        /// The client uses this as some sort of offset. 
        /// Unk0 = 1 gets the intuitive behavior.
        /// </summary>
        public uint Unk0 { get; set; } = 1;
        /// <summary>
        /// BonusType 1-5 map to WalletType 1-5, but the others are not included.
        /// Alternatively, you can use an ItemID here.
        /// </summary>
        public uint BonusType { get; set; }
        public uint BonusValue { get; set; }

        public class Serializer : EntitySerializer<CDataStampBonus>
        {
            public override void Write(IBuffer buffer, CDataStampBonus obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.BonusType);
                WriteUInt32(buffer, obj.BonusValue);
            }

            public override CDataStampBonus Read(IBuffer buffer)
            {
                CDataStampBonus obj = new CDataStampBonus();
                obj.Unk0 = ReadUInt32(buffer);
                obj.BonusType = ReadUInt32(buffer);
                obj.BonusValue = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStampBonusAsset
    {
        public CDataStampBonusAsset()
        {
            StampBonus = new List<CDataStampBonus>();
        }

        public List<CDataStampBonus> StampBonus { get; set; }
        public ushort StampNum {  get; set; }
        public byte RecieveState { get; set; }

        public class Serializer : EntitySerializer<CDataStampBonusAsset>
        {
            public override void Write(IBuffer buffer, CDataStampBonusAsset obj)
            {
                WriteEntityList<CDataStampBonus>(buffer, obj.StampBonus);
                WriteUInt16(buffer, obj.StampNum);
                WriteByte(buffer, obj.RecieveState);
            }

            public override CDataStampBonusAsset Read(IBuffer buffer)
            {
                CDataStampBonusAsset obj = new CDataStampBonusAsset();
                obj.StampBonus = ReadEntityList<CDataStampBonus>(buffer);
                obj.StampNum = ReadUInt16(buffer);
                obj.RecieveState = ReadByte(buffer);
                return obj;
            }
        }
    }
}

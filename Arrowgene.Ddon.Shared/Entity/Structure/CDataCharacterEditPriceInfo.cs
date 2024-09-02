using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterEditPriceInfo
    {
        public CDataCharacterEditPriceInfo()
        {
            Prices = new List<CDataWalletPoint>();
        }

        public byte UpdateType { get; set; }
        public List<CDataWalletPoint> Prices { get; set; }

        public class Serializer : EntitySerializer<CDataCharacterEditPriceInfo>
        {
            public override void Write(IBuffer buffer, CDataCharacterEditPriceInfo obj)
            {
                WriteByte(buffer, obj.UpdateType);
                WriteEntityList(buffer, obj.Prices);
            }

            public override CDataCharacterEditPriceInfo Read(IBuffer buffer)
            {
                CDataCharacterEditPriceInfo obj = new CDataCharacterEditPriceInfo();
                obj.UpdateType = ReadByte(buffer);
                obj.Prices = ReadEntityList<CDataWalletPoint>(buffer);
                return obj;
            }
        }
    }
}


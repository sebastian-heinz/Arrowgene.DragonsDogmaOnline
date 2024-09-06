using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStampBonusTotal
    {
        public CDataStampBonusTotal()
        {
            AssetList = new List<CDataStampBonusAsset>();
        }

        public ushort TotalStampNum { get; set; }
        public List<CDataStampBonusAsset> AssetList { get; set; }

        public class Serializer : EntitySerializer<CDataStampBonusTotal>
        {
            public override void Write(IBuffer buffer, CDataStampBonusTotal obj)
            {
                WriteUInt16(buffer, obj.TotalStampNum);
                WriteEntityList<CDataStampBonusAsset>(buffer, obj.AssetList);
            }

            public override CDataStampBonusTotal Read(IBuffer buffer)
            {
                CDataStampBonusTotal obj = new CDataStampBonusTotal();
                obj.TotalStampNum = ReadUInt16(buffer);
                obj.AssetList = ReadEntityList<CDataStampBonusAsset>(buffer);
                return obj;
            }
        }
    }
}

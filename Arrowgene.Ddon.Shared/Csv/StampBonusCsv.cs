using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class StampBonusCsv : CsvReaderWriter<CDataStampBonusAsset>
    {
        protected override int NumExpectedItems => 3;

        protected override CDataStampBonusAsset CreateInstance(string[] properties)
        {
            if (!ushort.TryParse(properties[0], out ushort stampNum)) return null;
            if (!uint.TryParse(properties[1], out uint type)) return null;
            if (!uint.TryParse(properties[2], out uint value)) return null;

            var obj = new CDataStampBonusAsset()
            {
                RecieveState = (byte)StampRecieveState.Unearned,
                StampNum = stampNum
            };

            obj.StampBonus.Add(new CDataStampBonus()
            {
                Unk0 = 1,
                BonusType = type,
                BonusValue = value
            });

            return obj;
        }
    }
}

using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class StorageCsv : CsvReaderWriter<CDataCharacterItemSlotInfo>
    {
        protected override int NumExpectedItems => 2;

        protected override CDataCharacterItemSlotInfo CreateInstance(string[] properties)
        {
            if (!byte.TryParse(properties[0], out byte storageType)) return null;
            if (!ushort.TryParse(properties[1], out ushort slotMax)) return null;

            return new CDataCharacterItemSlotInfo()
            {
                StorageType = (StorageType) storageType,
                SlotMax = slotMax
            };
        }
    }
}

using System;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class StorageItemCsv : CsvReaderWriter<Tuple<StorageType, uint, Item>>
    {
        protected override int NumExpectedItems => 6;

        protected override Tuple<StorageType, uint, Item> CreateInstance(string[] properties)
        {
            if (!byte.TryParse(properties[0], out byte storageType)) return null;
            if (!uint.TryParse(properties[1], out uint itemNum)) return null;
            if (!uint.TryParse(properties[2], out uint itemId)) return null;
            if (!byte.TryParse(properties[3], out byte unk3)) return null;
            if (!byte.TryParse(properties[4], out byte color)) return null;
            if (!byte.TryParse(properties[5], out byte plusValue)) return null;

            return new Tuple<StorageType, uint, Item>(
                (StorageType) storageType, 
                itemNum, 
                new Item()
                {
                    ItemId = itemId,
                    SafetySetting = unk3,
                    Color = color,
                    PlusValue = plusValue
                });
        }
    }
}

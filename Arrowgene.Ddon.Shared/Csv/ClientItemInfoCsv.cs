using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class ClientItemInfoCsv : CsvReaderWriter<ClientItemInfo>
    {
        protected override int NumExpectedItems => 4;

        protected override ClientItemInfo CreateInstance(string[] properties)
        {
            if (!uint.TryParse(properties[0], out uint itemId)) return null;
            if (!byte.TryParse(properties[1], out byte storageType)) return null;
            if (!ushort.TryParse(properties[2], out ushort price)) return null;
            if (!byte.TryParse(properties[3], out byte stackLimit)) return null;
            return new ClientItemInfo
            {
                ItemId = itemId,
                StorageType = (StorageType) storageType,
                Price = price,
                StackLimit = stackLimit
            };
        }
    }
}
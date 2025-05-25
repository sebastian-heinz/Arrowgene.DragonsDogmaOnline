using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class ClientItemInfoCsv : CsvReaderWriter<ClientItemInfo>
    {
        protected override int NumExpectedItems => 12;

        protected override ClientItemInfo CreateInstance(string[] properties)
        {
            if (!uint.TryParse(properties[0], out uint itemId)) return null;
            if (!byte.TryParse(properties[1], out byte category)) return null;
            if (!ushort.TryParse(properties[2], out ushort price)) return null;
            if (!byte.TryParse(properties[3], out byte stackLimit)) return null;
            if (!byte.TryParse(properties[4], out byte rank)) return null;

            string name = properties[5];

            //Optional properties

            ushort? subcategory = ushort.TryParse(properties[6], out ushort tmp0) ? (ushort?)tmp0 : null;
            byte? level = byte.TryParse(properties[7], out byte tmp1) ? (byte?)tmp1 : null;
            byte? jobs = byte.TryParse(properties[8], out byte tmp2) ? (byte?)tmp2 : null;
            byte? crestSlots = byte.TryParse(properties[9], out byte tmp3) ? (byte?)tmp3 : null;
            byte? quality = byte.TryParse(properties[10], out byte tmp4) ? (byte?)tmp4 : null;
            byte? gender = byte.TryParse(properties[11], out byte tmp5) ? (byte?)tmp5 : null;

            if (subcategory != null && !Enum.IsDefined(typeof(ItemSubCategory), subcategory)) return null;

            return new ClientItemInfo
            {
                ItemId = itemId,
                Category = category,
                Price = price,
                StackLimit = stackLimit,
                Rank = rank,
                Name = name,
                SubCategory = subcategory != null ? (ItemSubCategory)subcategory : ItemSubCategory.None,
                Level = level,
                JobGroup = jobs != null ? (EquipJobList)jobs : null,
                CrestSlots = crestSlots,
                Quality = quality,
                Gender = gender != null ? (Gender)gender : null,
            };
        }
    }
}

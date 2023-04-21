using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public class ClientItemInfo
    {
        public uint ItemId;
        public StorageType StorageType;

        // TODO: Optimize this mess (Use a Set or something that doesn't require looping over it)
        public static StorageType GetStorageTypeForItemId(List<ClientItemInfo> clientItemInfos, uint itemId)
        {
            foreach (ClientItemInfo itemInfo in clientItemInfos)
            {
                if(itemInfo.ItemId == itemId)
                {
                    return itemInfo.StorageType;
                }
            }
            throw new Exception("No item found with ID "+itemId);
        }
    }
}
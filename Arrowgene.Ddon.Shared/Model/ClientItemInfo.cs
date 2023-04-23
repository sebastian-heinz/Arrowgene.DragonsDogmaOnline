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
        public ushort Price;
        public byte StackLimit;

        // TODO: Optimize this mess (Use a Set or something that doesn't require looping over it)
        public static ClientItemInfo GetInfoForItemId(List<ClientItemInfo> clientItemInfos, uint itemId)
        {
            foreach (ClientItemInfo itemInfo in clientItemInfos)
            {
                if(itemInfo.ItemId == itemId)
                {
                    return itemInfo;
                }
            }
            throw new Exception("No item found with ID "+itemId);
        }
    }
}
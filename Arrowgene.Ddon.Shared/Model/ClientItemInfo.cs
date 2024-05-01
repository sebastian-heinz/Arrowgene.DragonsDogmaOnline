using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model
{
    public class ClientItemInfo
    {
        public uint ItemId;
        public byte Category;
        public ushort Price;
        public byte StackLimit;

        public StorageType StorageType
        { 
            get
            {
                if (Category == 5)
                    // Job Items have a category of 5, but Job Items StorageType is 4
                    return StorageType.ItemBagJob;
                else
                    return (StorageType) Category;
            }
        }

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
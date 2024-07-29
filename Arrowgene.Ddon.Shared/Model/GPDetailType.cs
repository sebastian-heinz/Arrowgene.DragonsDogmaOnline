using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum GPDetailType : uint
    {
        None = 0,
        OnlineShopPurchase = 1,
        OnlineShopBonus = 2,
        LoginBonus = 3,
        ReceivedInMail = 4,
        PlayStationStorePurchase = 5,
        PlaystationStoreBonus = 6
    }
}

using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class IEquipmentRecycleMixin
    {
        /// <summary>
        /// This calculates what the items are that are actually returned and the currencies calcuated by the forcast.
        /// </summary>
        /// <param name="assetRepository"></param>
        /// <param name="itemInfo"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract RecycleRewards GetRecycleRewards(AssetRepository assetRepository, ClientItemInfo itemInfo, Item item);
    }
}

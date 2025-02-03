using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class IGameItem
    {
        public abstract ItemId ItemId { get; }

        /**
         * @brief Called when an item is used by a player.
         */
        public abstract void OnUse(GameClient client);

        /**
         * Called for items which have an impact to the player or pawn when they are
         * equipped when completing certain actions such as killing enemies or completing quests.
         */
        public abstract double GetBonusMultiplier(CharacterCommon characterCommon);
    }
}

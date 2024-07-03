using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.BattleContent;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class BitterblackMazeAsset
    {
        public BitterblackMazeAsset()
        {
            Stages = new Dictionary<StageId, BitterblackMazeConfig>();
        }

        public Dictionary<StageId, BitterblackMazeConfig> Stages { get; set; }
    }
}


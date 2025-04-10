using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public class AchievementBackgroundAsset
    {
        public HashSet<uint> DefaultBackgrounds { get; set; } = new();
        public HashSet<(uint Id, uint Required)> UnlockableBackgrounds { get; set; } = new();
    }
}

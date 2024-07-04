using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum ShopCategory : uint
    {
        Unknown = 0,
        Trinkets = 1, // Does this shop have a proper name (NPC Gregory)?
        // Trinkets 1 might need to be expanded if NPC has multiple options
        BitterblackMaze = 6,
        ExtremeMission = 27,
    }
}

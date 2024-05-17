using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum QuestBlockType : uint
    {
         None = 0,
         NpcOrder = 1,
         DiscoverEnemy= 2,
         KillGroup = 3,
         TalkToNpc = 4,
         DeliverItems = 5,
         SeekOutEnemiesAtMarkedLocation = 6,
         End = 7
    }
}

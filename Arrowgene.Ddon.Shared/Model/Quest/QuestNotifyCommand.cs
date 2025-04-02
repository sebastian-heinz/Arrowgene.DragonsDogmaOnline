using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    /**
     * The list seems to be similar to the check command list.
     * Values in this table were found in packet captures
     */
    public enum QuestNotifyCommand : uint
    {
        None = 0,
        FulfillDeliverItem = 5, // notifyFulfillDeliverItem(cQuestTask::cQuestProcess *this, u32 npcId);
        KilledEnemyLight = 6,
        SetQuestClearNum = 32, // Set Quest AKA world quest
        MakeCraft = 33,
        KilledTargetEnemySetGroup = 109, // notifyKilledTargetEnemySetGroup(cQuestTask::cQuestProcess *this, u32 flagNo, u32 stageNo, u32 groupNo);
        KilledTargetEmSetGrpNoMarker = 110, // this is a guess based on packet data and comes in pair with previous command (and almost same exact arguments besides command value)
        Craft = 159,
    }
}

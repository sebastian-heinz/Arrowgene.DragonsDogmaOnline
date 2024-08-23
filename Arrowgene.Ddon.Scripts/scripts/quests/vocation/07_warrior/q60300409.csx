/**
 * @brief Vocation Emblem Trial: Warrior
 */

#load "libs.csx"
#load "quests/vocation/EmblemTrial.csx"

public class ScriptedQuest : EmblemTrial
{
    public override QuestId QuestId => QuestId.VocationEmblemTrialWarrior;

    public override JobId QuestJobId => JobId.Warrior;
    public override uint GilstanMsgId0 => 23947;
    public override uint GilstanMsgId1 => 26307;
    public override uint SuraboQstLayoutFlag => 5653;
    public override uint SuraboMsgId0 => 23949;
    public override uint SuraboMsgId1 => 25615;
    public override uint RentonMsgId0 => 25616;
    public override ItemId EmblemItemId => ItemId.EmblemStoneWarrior;
    public override ContentsRelease JobEmblemContentsRelease => ContentsRelease.WarriorVocationEmblem;
}

return new ScriptedQuest();

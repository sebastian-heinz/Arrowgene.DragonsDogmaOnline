/**
 * @brief Vocation Emblem Trial: Hunter
 */

#load "libs.csx"
#load "quests/vocation/EmblemTrial.csx"

public class ScriptedQuest : EmblemTrial
{
    public override QuestId QuestId => QuestId.VocationEmblemTrialHunter;

    public override JobId QuestJobId => JobId.Hunter;
    public override uint GilstanMsgId0 => 23917;
    public override uint GilstanMsgId1 => 26292;
    public override uint SuraboQstLayoutFlag => 5648;
    public override uint SuraboMsgId0 => 23919;
    public override uint SuraboMsgId1 => 25580;
    public override uint RentonMsgId0 => 25581;
    public override ItemId EmblemItemId => ItemId.EmblemStoneHunter;
    public override ContentsRelease JobEmblemContentsRelease => ContentsRelease.HunterVocationEmblem;
}

return new ScriptedQuest();

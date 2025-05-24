/**
 * @brief Vocation Emblem Trial: High Scepter
 */

#load "libs.csx"
#load "quests/vocation/EmblemTrial.csx"

public class ScriptedQuest : EmblemTrial
{
    public override QuestId QuestId => QuestId.VocationEmblemTrialHighScepter;

    public override JobId QuestJobId => JobId.HighScepter;
    public override uint GilstanMsgId0 => 28315;
    public override uint GilstanMsgId1 => 28328;
    public override uint SuraboQstLayoutFlag => 7515;
    public override uint SuraboMsgId0 => 28317;
    public override uint SuraboMsgId1 => 28321;
    public override uint RentonMsgId0 => 28322;
    public override ItemId EmblemItemId => ItemId.EmblemStoneHighScepter;
    public override ContentsRelease JobEmblemContentsRelease => ContentsRelease.HighScepterVocationEmblem;
}

return new ScriptedQuest();

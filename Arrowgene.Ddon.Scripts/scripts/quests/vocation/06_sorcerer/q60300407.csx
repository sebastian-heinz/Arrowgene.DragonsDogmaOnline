/**
 * @brief Vocation Emblem Trial: Sorcerer
 */

#load "libs.csx"
#load "quests/vocation/EmblemTrial.csx"

public class ScriptedQuest : EmblemTrial
{
    public override QuestId QuestId => QuestId.VocationEmblemTrialSorcerer;

    public override JobId QuestJobId => JobId.Sorcerer;
    public override uint GilstanMsgId0 => 23937;
    public override uint GilstanMsgId1 => 26302;
    public override uint SuraboQstLayoutFlag => 5651;
    public override uint SuraboMsgId0 => 23939;
    public override uint SuraboMsgId1 => 25601;
    public override uint RentonMsgId0 => 25602;
    public override ItemId EmblemItemId => ItemId.EmblemStoneSorcerer;
    public override ContentsRelease JobEmblemContentsRelease => ContentsRelease.SorcererVocationEmblem;
}

return new ScriptedQuest();

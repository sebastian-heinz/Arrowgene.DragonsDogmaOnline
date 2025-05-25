/**
 * @brief Vocation Emblem Trial: Fighter
 */

#load "libs.csx"
#load "quests/vocation/EmblemTrial.csx"

public class ScriptedQuest : EmblemTrial
{
    public override QuestId QuestId => QuestId.VocationEmblemTrialFighter;

    public override JobId QuestJobId => JobId.Fighter;
    public override uint GilstanMsgId0 => 23907;
    public override uint GilstanMsgId1 => 26286;
    public override uint SuraboQstLayoutFlag => 5646;
    public override uint SuraboMsgId0 => 23909;
    public override uint SuraboMsgId1 => 25566;
    public override uint RentonMsgId0 => 25567;
    public override ItemId EmblemItemId => ItemId.EmblemStoneFighter;
    public override ContentsRelease JobEmblemContentsRelease => ContentsRelease.FighterVocationEmblem;
}

return new ScriptedQuest();

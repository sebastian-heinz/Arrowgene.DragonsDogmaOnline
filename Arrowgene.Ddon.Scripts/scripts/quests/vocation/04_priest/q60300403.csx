/**
 * @brief Vocation Emblem Trial: Priest
 */

#load "libs.csx"
#load "quests/vocation/EmblemTrial.csx"

public class ScriptedQuest : EmblemTrial
{
    public override QuestId QuestId => QuestId.VocationEmblemTrialPriest;

    public override JobId QuestJobId => JobId.Priest;
    public override uint GilstanMsgId0 => 23912;
    public override uint GilstanMsgId1 => 26289;
    public override uint SuraboQstLayoutFlag => 5647;
    public override uint SuraboMsgId0 => 23914;
    public override uint SuraboMsgId1 => 25573;
    public override uint RentonMsgId0 => 25574;
    public override ItemId EmblemItemId => ItemId.EmblemStonePriest;
    public override ContentsRelease JobEmblemContentsRelease => ContentsRelease.PriestVocationEmblem;
}

return new ScriptedQuest();

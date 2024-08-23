/**
 * @brief Vocation Emblem Trial: Spirit Lancer
 */

#load "libs.csx"
#load "quests/vocation/EmblemTrial.csx"

public class ScriptedQuest : EmblemTrial
{
    public override QuestId QuestId => QuestId.VocationEmblemTrialSpiritLancer;

    public override JobId QuestJobId => JobId.SpiritLancer;
    public override uint GilstanMsgId0 => 23957;
    public override uint GilstanMsgId1 => 26313;
    public override uint SuraboQstLayoutFlag => 5655;
    public override uint SuraboMsgId0 => 23959;
    public override uint SuraboMsgId1 => 25629;
    public override uint RentonMsgId0 => 25630;
    public override ItemId EmblemItemId => ItemId.EmblemStoneSpiritLancer;
    public override ContentsRelease JobEmblemContentsRelease => ContentsRelease.SpiritLancerVocationEmblem;
}

return new ScriptedQuest();

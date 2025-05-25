/**
 * @brief Vocation Emblem Trial: Element Archer
 */

#load "libs.csx"
#load "quests/vocation/EmblemTrial.csx"

public class ScriptedQuest : EmblemTrial
{
    public override QuestId QuestId => QuestId.VocationEmblemTrialElementArcher;

    public override JobId QuestJobId => JobId.ElementArcher;
    public override uint GilstanMsgId0 => 23942;
    public override uint GilstanMsgId1 => 26305;
    public override uint SuraboQstLayoutFlag => 5652;
    public override uint SuraboMsgId0 => 23944;
    public override uint SuraboMsgId1 => 26304;
    public override uint RentonMsgId0 => 25609;
    public override ItemId EmblemItemId => ItemId.EmblemStoneElementArcher;
    public override ContentsRelease JobEmblemContentsRelease => ContentsRelease.ElementArcherVocationEmblem;
}

return new ScriptedQuest();

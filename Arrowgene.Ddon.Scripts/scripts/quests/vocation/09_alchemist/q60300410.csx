/**
 * @brief Vocation Emblem Trial: Alchemist
 */

#load "libs.csx"
#load "quests/vocation/EmblemTrial.csx"

public class ScriptedQuest : EmblemTrial
{
    public override QuestId QuestId => QuestId.VocationEmblemTrialAlchemist;

    public override JobId QuestJobId => JobId.Alchemist;
    public override uint GilstanMsgId0 => 23952;
    public override uint GilstanMsgId1 => 26311;
    public override uint SuraboQstLayoutFlag => 5654;
    public override uint SuraboMsgId0 => 23954;
    public override uint SuraboMsgId1 => 25622;
    public override uint RentonMsgId0 => 25623;
    public override ItemId EmblemItemId => ItemId.EmblemStoneAlchemist;
    public override ContentsRelease JobEmblemContentsRelease => ContentsRelease.AlchemistVocationEmblem;
}

return new ScriptedQuest();

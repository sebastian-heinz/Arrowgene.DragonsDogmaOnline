/**
 * @brief Vocation Emblem Trial: Shield Sage
 */

#load "libs.csx"
#load "quests/vocation/EmblemTrial.csx"

public class ScriptedQuest : EmblemTrial
{
    public override QuestId QuestId => QuestId.VocationEmblemTrialShieldSage;

    public override JobId QuestJobId => JobId.ShieldSage;
    public override uint GilstanMsgId0 => 23922;
    public override uint GilstanMsgId1 => 26295;
    public override uint SuraboQstLayoutFlag => 5649;
    public override uint SuraboMsgId0 => 23924;
    public override uint SuraboMsgId1 => 26294;
    public override uint RentonMsgId0 => 25588;
    public override ItemId EmblemItemId => ItemId.EmblemStoneShieldSage;
    public override ContentsRelease JobEmblemContentsRelease => ContentsRelease.ShieldSageVocationEmblem;
}

return new ScriptedQuest();

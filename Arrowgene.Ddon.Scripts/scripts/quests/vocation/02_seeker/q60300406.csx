/**
 * @brief Vocation Emblem Trial: Seeker
 */

#load "libs.csx"
#load "quests/vocation/EmblemTrial.csx"

public class ScriptedQuest : EmblemTrial
{
    public override QuestId QuestId => QuestId.VocationEmblemTrialSeeker;

    public override JobId QuestJobId => JobId.Seeker;
    public override uint GilstanMsgId0 => 23932;
    public override uint GilstanMsgId1 => 26298;
    public override uint SuraboQstLayoutFlag => 5650;
    public override uint SuraboMsgId0 => 23934;
    public override uint SuraboMsgId1 => 26297;
    public override uint RentonMsgId0 => 25595;
    public override ItemId EmblemItemId => ItemId.EmblemStoneSeeker;
    public override ContentsRelease JobEmblemContentsRelease => ContentsRelease.SeekerVocationEmblem;
}

return new ScriptedQuest();

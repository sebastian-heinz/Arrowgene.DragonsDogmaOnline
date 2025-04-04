/**
 * @brief Quest used to generate announce messages for generic enemies
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.WorldManage;
    public override QuestId QuestId => QuestId.WorldManageMonsterCaution;
    public override ushort RecommendedLevel => 0;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNoProgressBlock();
        process0.AddNoProgressBlock();
        process0.AddProcessEndBlock(false);

        ushort processIndex = 1;
        foreach (var spot in LibDdon.GetCautionSpots())
        {
            if (!spot.CautionPlayer)
            {
                continue;
            }

            var stageInfo = Stage.StageInfoFromStageLayoutId(spot.StageLayoutId);
            var groupId = spot.StageLayoutId.GroupId;

            var process = AddNewProcess(processIndex++);
            process.AddRawBlock(QuestAnnounceType.None)
                .AddCheckCmdIsEnemyFound(stageInfo, groupId, -1, false)
                .AddCheckCmdCheckAreaRank(spot.AreaId, spot.RequiredAreaRank)
                .AddCheckCmdIsStageNo(stageInfo, false);
            process.AddRawBlock(QuestAnnounceType.Caution)
                .AddCheckCmdIsResetInstanceArea();
            process.AddReturnCheckPointBlock(process.ProcessNo, 1);
            process.AddNoProgressBlock();
            process.AddProcessEndBlock(false);
        }
    }
}

return new ScriptedQuest();

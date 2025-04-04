/**
 * @brief Quest used to test out flags without reloading the server or changing quest files
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.WorldManage;
    public override QuestId QuestId => QuestId.WorldManageDebug;
    public override ushort RecommendedLevel => 0;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNoProgressBlock();
        process0.AddNoProgressBlock();
        process0.AddProcessEndBlock(false);
    }
}

return new ScriptedQuest();

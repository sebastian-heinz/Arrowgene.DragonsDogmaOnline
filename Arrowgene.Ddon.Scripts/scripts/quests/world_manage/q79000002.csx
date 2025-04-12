/**
 * @brief Quest used to generate tutorial message for jobs when they are selected for the first time.
 */

#load "libs.csx"

public class ScriptedQuest : IQuest
{
    public override QuestType QuestType => QuestType.WorldManage;
    public override QuestId QuestId => QuestId.WorldManageJobTutorial;
    public override ushort RecommendedLevel => 0;
    public override byte MinimumItemRank => 0;
    public override bool IsDiscoverable => false;

    private List<(JobId JobId, TutorialId TutorialId)> BasicJobTutorials = new List<(JobId JobId, TutorialId TutorialId)>()
    {
        (JobId.Fighter, TutorialId.BasicTacticsFighter),
        (JobId.Seeker, TutorialId.BasicTacticsSeeker),
        (JobId.Hunter, TutorialId.BasicTacticsHunter),
        (JobId.Priest, TutorialId.BasicTacticsPriest),
        (JobId.ShieldSage, TutorialId.BasicTacticsShieldSage),
        (JobId.Sorcerer, TutorialId.BasicTacticsSorcerer),
        (JobId.Warrior, TutorialId.BasicTacticsWarrior),
        (JobId.ElementArcher, TutorialId.BasicTacticsElementArcher),
        (JobId.Alchemist, TutorialId.BasicTacticsAlchemist),
        (JobId.SpiritLancer, TutorialId.BasicTacticsSpiritLancer),
        (JobId.HighScepter, TutorialId.BasicTacticsHighScepter),
    };

    protected override void InitializeBlocks()
    {
        var process0 = AddNewProcess(0);
        process0.AddNoProgressBlock();
        process0.AddNoProgressBlock();
        process0.AddProcessEndBlock(false);

        ushort processIndex = 1;
        foreach (var tutorial in BasicJobTutorials)
        {
            var process = AddNewProcess(processIndex++);
            process.AddRawBlock(QuestAnnounceType.None)
                .AddCheckCmdPlJobEq(tutorial.JobId);
            process.AddRawBlock(QuestAnnounceType.None)
                .AddResultCmdTutorialDialog(tutorial.TutorialId);
            process.AddProcessEndBlock(false);
        }
    }
}

return new ScriptedQuest();

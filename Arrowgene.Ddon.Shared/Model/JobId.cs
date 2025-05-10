using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum JobId : byte
    {
        None = 0,
        Fighter = 1,
        Seeker = 2,
        Hunter = 3,
        Priest = 4,
        ShieldSage = 5,
        Sorcerer = 6,
        Warrior = 7,
        ElementArcher = 8,
        Alchemist = 9,
        SpiritLancer = 10,
        HighScepter = 11,
    }

    public static class JobIdExtensions
    {
        public static readonly ReadOnlyCollection<JobId> GreenJobs = new List<JobId>()
        {
            JobId.Priest,
            JobId.ElementArcher,
            JobId.Seeker,
            JobId.SpiritLancer,
        }.AsReadOnly();

        public static readonly ReadOnlyCollection<JobId> RedJobs = new List<JobId>()
        {
            JobId.Fighter,
            JobId.Hunter,
            JobId.Seeker,
            JobId.Sorcerer,
            JobId.Warrior,
            JobId.HighScepter
        }.AsReadOnly();

        public static readonly ReadOnlyCollection<JobId> BlueJobs = new List<JobId>()
        {
            JobId.ShieldSage,
            JobId.Alchemist
        }.AsReadOnly();

        public static bool IsGreenJob(this JobId jobId)
        {
            return GreenJobs.Contains(jobId);
        }

        public static bool IsRedJob(this JobId jobId)
        {
            return RedJobs.Contains(jobId);
        }

        public static bool IsBlueJob(this JobId jobId)
        {
            return BlueJobs.Contains(jobId);
        }

        private static Dictionary<JobId, ContentsRelease> JobTrainingReleaseIds = new Dictionary<JobId, ContentsRelease>()
        {
            [JobId.Fighter] = ContentsRelease.FighterJobTraining,
            [JobId.Seeker] = ContentsRelease.SeekerJobTraining,
            [JobId.Hunter] = ContentsRelease.HunterJobTraining,
            [JobId.Priest] = ContentsRelease.PriestJobTraining,
            [JobId.ShieldSage] = ContentsRelease.ShieldSageJobTraining,
            [JobId.Sorcerer] = ContentsRelease.SorcererJobTraining,
            [JobId.Warrior] = ContentsRelease.WarriorJobTraining,
            [JobId.ElementArcher] = ContentsRelease.ElementArcherJobTraining,
            [JobId.Alchemist] = ContentsRelease.AlchemistJobTraining,
            [JobId.SpiritLancer] = ContentsRelease.SpiritLancerJobTraining,
            [JobId.HighScepter] = ContentsRelease.HighScepterJobTraining,
        };

        public static ContentsRelease JobTrainingReleaseId(this JobId jobId)
        {
            return JobTrainingReleaseIds[jobId];
        }

        private static Dictionary<JobId, ContentsRelease> SkillAugmentationReleaseIds = new Dictionary<JobId, ContentsRelease>()
        {
            [JobId.Fighter] = ContentsRelease.FighterWarSkillAugmentation,
            [JobId.Seeker] = ContentsRelease.SeekerWarSkillAugmentation,
            [JobId.Hunter] = ContentsRelease.HunterWarSkillAugmentation,
            [JobId.Priest] = ContentsRelease.PriestWarSkillAugmentation,
            [JobId.ShieldSage] = ContentsRelease.ShieldSageWarSkillAugmentation,
            [JobId.Sorcerer] = ContentsRelease.SorcererWarSkillAugmentation,
            [JobId.Warrior] = ContentsRelease.WarriorWarSkillAugmentation,
            [JobId.ElementArcher] = ContentsRelease.ElementArcherWarSkillAugmentation,
            [JobId.Alchemist] = ContentsRelease.AlchemistWarSkillAugmentation,
            [JobId.SpiritLancer] = ContentsRelease.SpiritLancerWarSkillAugmentation,
            [JobId.HighScepter] = ContentsRelease.HighScepterWarSkillAugmentation,
        };

        public static ContentsRelease SkillAugmentationReleaseId(this JobId jobId)
        {
            return SkillAugmentationReleaseIds[jobId];
        }
    }
}

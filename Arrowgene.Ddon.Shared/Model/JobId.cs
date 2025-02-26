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
    }
}

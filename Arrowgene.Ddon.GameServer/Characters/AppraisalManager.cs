using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Appraisal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class AppraisalManager
    {
        public static uint RollDragonTrinketsAlpha(JobId jobId)
        {
            return DragonTrinketAlphaRewards.Rolls[jobId][Random.Shared.Next(0, DragonTrinketAlphaRewards.Rolls[jobId].Count)];
        }

        public static List<uint> GetDragonTrinketAlphaRolls(JobId jobId)
        {
            return DragonTrinketAlphaRewards.Rolls[jobId];
        }

        public static uint RollDragonTrinketsBeta(JobId jobId)
        {
            return DragonTrinketBetaRewards.Rolls[jobId][Random.Shared.Next(0, DragonTrinketBetaRewards.Rolls[jobId].Count)];
        }

        public static List<uint> GetDragonTrinketBetaRolls(JobId jobId)
        {
            return DragonTrinketBetaRewards.Rolls[jobId];
        }

        public static uint RollCrestLottery(List<uint> rolls)
        {
            return rolls[Random.Shared.Next(0, rolls.Count)];
        }

        public static uint RollBitterBlackMazeEarringCrest(JobId jobId)
        {
            var jobCrests = BitterBlackMazeRewards.EarringRolls[jobId];
            return jobCrests[Random.Shared.Next(0, jobCrests.Count)];
        }

        public static ushort RollBitterBlackMazeEarringPercent(JobId job)
        {
            /**
             * Based on research in discord, Warrior and Shield sage earrings can roll a
             * higher % range 8-20% when being appraised. The rest of the jobs can roll
             * 1-13% bonus on their equipment. The percentage values are incoded as ushorts.
             * For example 2 == 2% in the UI.
             */
            if (job == JobId.Warrior || job == JobId.ShieldSage)
            {
                // [8, 20]
                return (ushort)Random.Shared.Next(8, 20 + 1);
            }
            // [1, 13]
            return (ushort)Random.Shared.Next(1, 13 + 1);
        }
    }
}

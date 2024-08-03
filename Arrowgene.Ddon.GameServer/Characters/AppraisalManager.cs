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

    }
}

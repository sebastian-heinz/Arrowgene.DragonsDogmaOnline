using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Appraisal;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class AppraisalManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AppraisalManager));

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

        public static uint RollBitterBlackMazeEarringCrest(HashSet<uint> seals, JobId jobId)
        {
            List<uint> rolls = [.. BitterblackMazeRewards.AppraisalData.Where(x => x.BaseItem == ItemId.BitterblackEarring
                && x.SpecificJob == jobId
                && !seals.Contains(x.SealIndex))
                .Select(x => (uint)x.CrestId)];

            if (rolls.Count != 0)
            {
                return rolls[Random.Shared.Next(0, rolls.Count)];
            }
            else
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_DISPEL_NO_OPTIONS);
            }
        }

        public static uint RollBitterBlackMazeBraceletCrest(HashSet<uint> seals)
        {
            List<uint> rolls = [.. BitterblackMazeRewards.AppraisalData.Where(x => x.BaseItem == ItemId.BitterblackBracelet
                && !seals.Contains(x.SealIndex))
                .Select(x => (uint)x.CrestId)];
            if (rolls.Count != 0)
            {
                return rolls[Random.Shared.Next(0, rolls.Count)];
            }
            else
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_DISPEL_NO_OPTIONS);
            }
        }

        public static ushort RollBitterBlackMazeEarringPercent(JobId jobId)
        {
            return 0;
        }
    }
}

using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model
{
    public static class EnemyIdExtension
    {
        private static readonly Dictionary<EnemyId, HmPresetId> HmPresetMap = new Dictionary<EnemyId, HmPresetId>()
        {
            [EnemyId.RogueFighter] = HmPresetId.RogueFighter,
            [EnemyId.RogueSeeker] = HmPresetId.RogueSeeker,
            [EnemyId.RogueHunter] = HmPresetId.RogueHunter,
            [EnemyId.RogueHealer] = HmPresetId.RogueHealer,
            [EnemyId.RogueDefender] = HmPresetId.RogueDefender,
            [EnemyId.RogueMage] = HmPresetId.RogueMage,
            [EnemyId.RogueWarrior] = HmPresetId.RogueWarrior,
            [EnemyId.BandedFighter] = HmPresetId.BandedFighter,
            [EnemyId.BandedSeeker] = HmPresetId.BandedSeeker,
            [EnemyId.BandedHunter] = HmPresetId.BandedHunter,
            [EnemyId.BandedHealer] = HmPresetId.BandedHealer,
            [EnemyId.BandedDefender] = HmPresetId.BandedDefender,
            [EnemyId.BandedMage] = HmPresetId.BandedMage,
            [EnemyId.BandedWarrior] = HmPresetId.BandedWarrior,
            [EnemyId.MerganFighter] = HmPresetId.MerganFighter,
            [EnemyId.MerganSeeker] = HmPresetId.MerganSeeker,
            [EnemyId.MerganHunter] = HmPresetId.MerganHunter,
            [EnemyId.MerganHealer] = HmPresetId.MerganHealer,
            [EnemyId.MerganDefender] = HmPresetId.MerganDefender,
            [EnemyId.MerganMage] = HmPresetId.MerganMage,
            [EnemyId.MerganWarrior] = HmPresetId.MerganWarrior,
            [EnemyId.MerganElementArcher] = HmPresetId.MerganElementalArcher,
            [EnemyId.MistFighter] = HmPresetId.MistFighter,
            [EnemyId.MistHunter] = HmPresetId.MistHunter,
            [EnemyId.MistPriest] = HmPresetId.MistPriest,
            [EnemyId.MistSorcerer] = HmPresetId.MistSorcerer,
            [EnemyId.MistWarrior] = HmPresetId.MistWarrior,
            [EnemyId.Iris] = HmPresetId.Iris,
            [EnemyId.DiamantesHuman] = HmPresetId.DiamantesHuman,
            [EnemyId.Leo] = HmPresetId.LeoS1,
            [EnemyId.HoodedIris] = HmPresetId.HoodedIris,
            [EnemyId.FormerCommanderLeoFinalBoss] = HmPresetId.LeoS3,
            [EnemyId.PawnFighter] = HmPresetId.PawnFighter,
            [EnemyId.PawnSeeker] = HmPresetId.PawnSeeker,
            [EnemyId.PawnHunter] = HmPresetId.PawnHunter,
            [EnemyId.PawnHealer] = HmPresetId.PawnHealer,
            [EnemyId.PawnDefender] = HmPresetId.PawnDefender,
            [EnemyId.PawnMage] = HmPresetId.PawnMage,
            [EnemyId.PawnWarrior] = HmPresetId.PawnWarrior,
            [EnemyId.PawnElementArcher] = HmPresetId.PawnElementalArcher,
            [EnemyId.PhindymianFighter] = HmPresetId.PhindymianFighter,
            [EnemyId.PhindymianSeeker] = HmPresetId.PhindymianSeeker,
            [EnemyId.PhindymianHunter] = HmPresetId.PhindymianHunter,
            [EnemyId.PhindymianPriest] = HmPresetId.PhindymianPriest,
            [EnemyId.PhindymianDefender] = HmPresetId.PhindymianDefender,
            [EnemyId.PhindymianSorcerer] = HmPresetId.PhindymianSorcerer,
            [EnemyId.PhindymianWarrior] = HmPresetId.PhindymianWarrior,
            [EnemyId.PhindymianElementArcher] = HmPresetId.PhindymianElementalArcher,
            [EnemyId.ScarletFighter] = HmPresetId.ScarletFighter,
            [EnemyId.ScarletSeeker] = HmPresetId.ScarletSeeker,
            [EnemyId.ScarletHunter] = HmPresetId.ScarletHunter,
            [EnemyId.ScarletHealer] = HmPresetId.ScarletHealer,
            [EnemyId.ScarletDefender] = HmPresetId.ScarletDefender,
            [EnemyId.ScarletMage] = HmPresetId.ScarletMage,
            [EnemyId.ScarletWarrior] = HmPresetId.ScarletWarrior,
            [EnemyId.MegadoGuardFighter] = HmPresetId.MegadoGuardFighter,
            [EnemyId.MegadoGuardSeeker] = HmPresetId.MegadoGuardSeeker,
            [EnemyId.MegadoGuardHunter] = HmPresetId.MegadoGuardHunter,
            [EnemyId.MegadoGuardWarrior] = HmPresetId.MegadoGuardWarrior,
        };

        public static HmPresetId GetHmPresetId(this EnemyId enemyId)
        {
            if (HmPresetMap.ContainsKey(enemyId))
            {
                return HmPresetMap[enemyId];
            }
            return HmPresetId.None;
        }
    }
}

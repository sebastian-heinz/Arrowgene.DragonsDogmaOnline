using Arrowgene.Ddon.Server.Scripting.utils;
using System.Collections.Generic;
using System.ComponentModel;

namespace Arrowgene.Ddon.Server.Settings
{
    public class PointModifierSettings : IGameSettings
    {
        public PointModifierSettings(ScriptableSettings settingsData) : base(settingsData, typeof(PointModifierSettings).Name)
        {
        }

        /// <summary>
        /// Handles EXP penalties for the party based on the
        /// difference between the lowest leveled member and highest
        /// leveled member of the party.If the range is larger than
        /// the last entry in AdjustPartyEnemyExpTiers, a 0% exp rate
        /// is automatically applied.
        /// </summary>
        [DefaultValue(_EnableAdjustPartyEnemyExp)]
        public bool EnableAdjustPartyEnemyExp
        {
            set
            {
                SetSetting("EnableAdjustPartyEnemyExp", value);
            }
            get
            {
                return TryGetSetting("EnableAdjustPartyEnemyExp", _EnableAdjustPartyEnemyExp);
            }
        }
        private const bool _EnableAdjustPartyEnemyExp = true;

        /// <summary>
        /// The ranges used when EnableAdjustPartyEnemyExp is true.
        /// </summary>
        [DefaultValue("new List<(uint MinLv, uint MaxLv, double ExpMultiplier)>()\n" +
            "{\n" +
            "    // MinLv and MaxLv define the relative level difference between the levels of the lowest and\n" +
            "    // highest members in the party.\n" +
            "    // The ExpMultiplier value can be a value between [0.0, 1.0] (1.0 = 100%, 0.0 = 0%)\n" +
            "    //\n" +
            "    // MinLv, MaxLv, ExpMultiplier\n" +
            "    (      0,     2,           1.0),\n" +
            "    (      3,     4,           0.9),\n" +
            "    (      5,     6,           0.8),\n" +
            "    (      7,     8,           0.6),\n" +
            "    (      9,    10,           0.5),\n" +
            "}")]
        public List<(uint MinLv, uint MaxLv, double ExpMultiplier)> AdjustPartyEnemyExpTiers
        {
            set
            {
                SetSetting("AdjustPartyEnemyExpTiers", value);
            }
            get
            {
                return TryGetSetting("AdjustPartyEnemyExpTiers", new List<(uint MinLv, uint MaxLv, double ExpMultiplier)>()
                {
                    (0,  2, 1.0),
                    (3,  4, 0.9),
                    (5,  6, 0.8),
                    (7,  8, 0.6),
                    (9, 10, 0.5),
                });
            }
        }

        /// <summary>
        /// Handles EXP penalties based on the highest leveled member
        /// in the party and the level of the target enemy.If the range is
        /// larger than the last entry in AdjustTargetLvEnemyExpTiers, a 0%
        /// exp rate is automatically applied.
        /// </summary>
        [DefaultValue(_EnableAdjustTargetLvEnemyExp)]
        public bool EnableAdjustTargetLvEnemyExp
        {
            set
            {
                SetSetting("EnableAdjustTargetLvEnemyExp", value);
            }
            get
            {
                return TryGetSetting("EnableAdjustTargetLvEnemyExp", _EnableAdjustTargetLvEnemyExp);
            }
        }
        private const bool _EnableAdjustTargetLvEnemyExp = false;

        /// <summary>
        /// The ranges used when EnableAdjustTargetLvEnemyExp is true.
        /// </summary>
        [DefaultValue("new List<(uint MinLv, uint MaxLv, double ExpMultiplier)>()\n" +
            "{\n" +
            "    // MinLv and MaxLv define the relative level difference between the target and highest member in the party.\n" +
            "    // The ExpMultiplier value can be a value between [0.0, 1.0] (1.0 = 100%, 0.0 = 0%)\n" +
            "    //\n" +
            "    // MinLv, MaxLv, ExpMultiplier\n" +
            "    (      0,     2,           1.0),\n" +
            "    (      3,     4,           0.9),\n" +
            "    (      5,     6,           0.8),\n" +
            "    (      7,     8,           0.6),\n" +
            "    (      9,    10,           0.5),\n" +
            "}")]
        public List<(uint MinLv, uint MaxLv, double ExpMultiplier)> AdjustTargetLvEnemyExpTiers
        {
            set
            {
                SetSetting("AdjustTargetLvEnemyExpTiers", value);
            }
            get
            {
                return TryGetSetting("AdjustTargetLvEnemyExpTiers", new List<(uint MinLv, uint MaxLv, double ExpMultiplier)>()
                {
                    (0,  2, 1.0),
                    (3,  4, 0.9),
                    (5,  6, 0.8),
                    (7,  8, 0.6),
                    (9, 10, 0.5),
                });
            }
        }

        /// <summary>
        /// If set to true, pawns owned by the player will not be included in exp penalty calculations.
        /// </summary>
        [DefaultValue(_DisableExpCorrectionForMyPawn)]
        public bool DisableExpCorrectionForMyPawn
        {
            set
            {
                SetSetting("DisableExpCorrectionForMyPawn", value);
            }
            get
            {
                return TryGetSetting("DisableExpCorrectionForMyPawn", _DisableExpCorrectionForMyPawn);
            }
        }
        private const bool _DisableExpCorrectionForMyPawn = true;

        /// <summary>
        /// If set to true, if the pawn is PawnCatchupLvDiff or more levels behind the players current level, an exp multiplers of PawnCatchupMultiplier is applied.
        /// </summary>
        [DefaultValue(_EnablePawnCatchup)]
        public bool EnablePawnCatchup
        {
            set
            {
                SetSetting("EnablePawnCatchup", value);
            }
            get
            {
                return TryGetSetting("EnablePawnCatchup", _EnablePawnCatchup);
            }
        }
        private const bool _EnablePawnCatchup = true;

        /// <summary>
        /// The exp bonus applied when the pawn catchup mechanic takes place if EnablePawnCatchup is set to true and the level difference requirements are met.
        /// </summary>
        [DefaultValue(_PawnCatchupMultiplier)]
        public double PawnCatchupMultiplier
        {
            set
            {
                SetSetting("PawnCatchupMultiplier", value);
            }
            get
            {
                return TryGetSetting("PawnCatchupMultiplier", _PawnCatchupMultiplier);
            }
        }
        private const double _PawnCatchupMultiplier = 1.5;

        /// <summary>
        /// The minimum level difference required for the catchup mechanic to be active if EnablePawnCatchup is set to true.
        /// </summary>
        [DefaultValue(_PawnCatchupLvDiff)]
        public uint PawnCatchupLvDiff
        {
            set
            {
                SetSetting("PawnCatchupLvDiff", value);
            }
            get
            {
                return TryGetSetting("PawnCatchupLvDiff", _PawnCatchupLvDiff);
            }
        }
        private const uint _PawnCatchupLvDiff = 5;
    }
}

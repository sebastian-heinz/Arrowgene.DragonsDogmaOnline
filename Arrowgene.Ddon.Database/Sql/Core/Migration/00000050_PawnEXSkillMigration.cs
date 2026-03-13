using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class PawnExSkillMigration(DatabaseSetting databaseSetting) : IMigrationStrategy
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(PawnExSkillMigration));

        public uint From => 49;
        public uint To => 50;

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            Logger.Info("Collecting updated records ...");
            var rows = new List<(uint CharacterId, OrbTreeType OrbTreeType, JobId JobId, uint ReleaseId)>();
            db.ExecuteReader(conn, "SELECT * FROM ddon_skill_augmentation_released_elements WHERE orb_tree_type = 2;",
               command => { },
               reader =>
               {
                   while (reader.Read())
                   {
                       var characterId = db.GetUInt32(reader, "character_id");
                       var orbTreeType = (OrbTreeType)db.GetUInt32(reader, "orb_tree_type");
                       var jobId = (JobId)db.GetUInt32(reader, "job_id");
                       var releaseId = db.GetUInt32(reader, "release_id");

                       if (OrbSkills.TryGetValue((orbTreeType, jobId, releaseId), out var _))
                       {
                           rows.Add((characterId, orbTreeType, jobId, releaseId));
                       }
                   }
               }
           );

            var rowDict = rows
                .GroupBy(x => x.CharacterId)
                .ToDictionary(key => key.Key, value => value.Select(x => (x.OrbTreeType, x.JobId, x.ReleaseId)).ToList());

            foreach(var (characterId, orbs) in rowDict)
            {
                List<uint> pawns = [];
                db.ExecuteReader(conn, "SELECT character_common_id FROM ddon_pawn WHERE character_id = @character_id;",
                    command => { db.AddParameter(command, "@character_id", characterId); },
                    reader =>
                    {
                        while (reader.Read())
                        {
                            pawns.Add(db.GetUInt32(reader, "character_common_id"));
                        }
                    }
                );

                var grantingSkills = orbs
                    .Where(x => OrbSkills.ContainsKey(x))
                    .Select(x => OrbSkills[x]);

                foreach (var pawnCommonId in pawns)
                {
                    foreach (var skill in grantingSkills)
                    {
                        db.ExecuteNonQuery(conn,
                            "INSERT INTO ddon_learned_custom_skill VALUES (@character_common_id, @job, @skill_id, @skill_lv) ON CONFLICT DO NOTHING;",
                            command =>
                            {
                                db.AddParameter(command, "@character_common_id", pawnCommonId);
                                db.AddParameter(command, "@job", (byte)skill.JobId());
                                db.AddParameter(command, "@skill_id", skill.ReleaseId());
                                db.AddParameter(command, "@skill_lv", 1);
                            }
                        );
                    }
                }
            }

            return true;
        }

        private static uint JobUniqueId(uint elementId, OrbTreeType orbTreeType, JobId jobId)
        {
            return elementId | ((uint)jobId << 28) | ((uint)orbTreeType << 24);
        }

        private static readonly Dictionary<(OrbTreeType TreeType, JobId Job, uint ElementId), CustomSkillId> OrbSkills = 
            new List<(OrbTreeType TreeType, JobId Job, uint ElementId, CustomSkillId CustomSkill)>() {
                (OrbTreeType.Season3, JobId.Alchemist, 8, CustomSkillId.PileBinderT),
                (OrbTreeType.Season3, JobId.Alchemist, 25, CustomSkillId.PileBinderP),
                (OrbTreeType.Season3, JobId.Alchemist, 34, CustomSkillId.AlmaPillarT),
                (OrbTreeType.Season3, JobId.Alchemist, 50, CustomSkillId.AlmaPillarP),
                (OrbTreeType.Season3, JobId.ElementArcher, 8, CustomSkillId.FlamingBowT),
                (OrbTreeType.Season3, JobId.ElementArcher, 25, CustomSkillId.FlamingBowP),
                (OrbTreeType.Season3, JobId.ElementArcher, 34, CustomSkillId.ExhaustingBowT),
                (OrbTreeType.Season3, JobId.ElementArcher, 50, CustomSkillId.ExhaustingBowP),
                (OrbTreeType.Season3, JobId.Fighter, 8, CustomSkillId.TuskTossT),
                (OrbTreeType.Season3, JobId.Fighter, 25, CustomSkillId.TuskTossP),
                (OrbTreeType.Season3, JobId.Fighter, 34, CustomSkillId.CymbalAttackT),
                (OrbTreeType.Season3, JobId.Fighter, 50, CustomSkillId.CymbalAttackP),
                (OrbTreeType.Season3, JobId.Hunter, 8, CustomSkillId.ThreefoldArrowT),
                (OrbTreeType.Season3, JobId.Hunter, 25, CustomSkillId.ThreefoldArrowP),
                (OrbTreeType.Season3, JobId.Hunter, 34, CustomSkillId.WhirlingArrowT),
                (OrbTreeType.Season3, JobId.Hunter, 50, CustomSkillId.WhirlingArrowP),
                (OrbTreeType.Season3, JobId.Priest, 8, CustomSkillId.SolaceRiserT),
                (OrbTreeType.Season3, JobId.Priest, 25, CustomSkillId.SolaceRiserP),
                (OrbTreeType.Season3, JobId.Priest, 34, CustomSkillId.SeraphimFlapT),
                (OrbTreeType.Season3, JobId.Priest, 50, CustomSkillId.SeraphimFlapP),
                (OrbTreeType.Season3, JobId.Seeker, 8, CustomSkillId.EasyKillT),
                (OrbTreeType.Season3, JobId.Seeker, 25, CustomSkillId.EasyKillP),
                (OrbTreeType.Season3, JobId.Seeker, 34, CustomSkillId.ExplosiveFlameBladeT),
                (OrbTreeType.Season3, JobId.Seeker, 50, CustomSkillId.ExplosiveFlameBladeP),
                (OrbTreeType.Season3, JobId.ShieldSage, 8, CustomSkillId.EarthShakeT),
                (OrbTreeType.Season3, JobId.ShieldSage, 25, CustomSkillId.EarthShakeP),
                (OrbTreeType.Season3, JobId.ShieldSage, 34, CustomSkillId.ForceShieldT),
                (OrbTreeType.Season3, JobId.ShieldSage, 50, CustomSkillId.ForceShieldP),
                (OrbTreeType.Season3, JobId.Sorcerer, 8, CustomSkillId.DarknessMistT),
                (OrbTreeType.Season3, JobId.Sorcerer, 25, CustomSkillId.DarknessMistP),
                (OrbTreeType.Season3, JobId.Sorcerer, 34, CustomSkillId.FulminationT),
                (OrbTreeType.Season3, JobId.Sorcerer, 50, CustomSkillId.FulminationP),
                (OrbTreeType.Season3, JobId.SpiritLancer, 8, CustomSkillId.WallGlastaT),
                (OrbTreeType.Season3, JobId.SpiritLancer, 25, CustomSkillId.WallGlastaP),
                (OrbTreeType.Season3, JobId.SpiritLancer, 34, CustomSkillId.AuromFangT),
                (OrbTreeType.Season3, JobId.SpiritLancer, 50, CustomSkillId.AuromFangP),
                (OrbTreeType.Season3, JobId.Warrior, 8, CustomSkillId.SavageLungeT),
                (OrbTreeType.Season3, JobId.Warrior, 25, CustomSkillId.SavageLungeP),
                (OrbTreeType.Season3, JobId.Warrior, 34, CustomSkillId.PommelStrikeT),
                (OrbTreeType.Season3, JobId.Warrior, 50, CustomSkillId.PommelStrikeP)
            }
            .ToDictionary(
                key => (key.TreeType, key.Job, JobUniqueId(key.ElementId, key.TreeType, key.Job)), 
                value => value.CustomSkill
            );
    }
}

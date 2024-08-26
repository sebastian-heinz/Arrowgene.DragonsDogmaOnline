using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class JobOrbUnlockManager
    {
        private DdonGameServer Server;
        public JobOrbUnlockManager(DdonGameServer server)
        {
            Server = server;
        }

        public List<CDataJobOrbDevoteElement> GetUpgradeList(JobId jobId)
        {
            var jobOrbUpgrades = Server.ScriptManager.SkillAugmentationModule.SkillAugmentations;

            if (!jobOrbUpgrades.ContainsKey(jobId))
            {
                return new List<CDataJobOrbDevoteElement>();
            }

            return jobOrbUpgrades[jobId].Select(x => x.ToCDataJobOrbDevoteElement()).ToList();
        }

#if false
        private readonly Dictionary<JobId, List<JobOrbUpgrade>> gJobOrbUpgrades = new Dictionary<JobId, List<JobOrbUpgrade>>()
        {
            #region Fighter
            [JobId.Fighter] = new List<JobOrbUpgrade>()
            {
                // 1st Tier
                new JobOrbUpgrade()
                    .Id(0, JobId.Fighter)
                    .Location(4, 1)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobHpMax, 30),
                // 2nd Tier
                new JobOrbUpgrade()
                    .Id(1, JobId.Fighter)
                    .Location(4, 2)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
                    .HasUnlockDependency(0),
                // 3rd Tier
                new JobOrbUpgrade()
                    .Id(2, JobId.Fighter)
                    .Location(3, 3)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
                    .HasUnlockDependency(1),
                new JobOrbUpgrade()
                    .Id(3, JobId.Fighter)
                    .Location(5, 3)
                    .BloodOrbCost(500)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
                    .HasUnlockDependency(1),
                // 4th Tier
                new JobOrbUpgrade()
                    .Id(4, JobId.Fighter)
                    .Location(4, 4)
                    .BloodOrbCost(1200)
                    .Unlocks(CustomSkillId.PierceSlash)
                    .HasUnlockDependencies(new List<uint>() {2, 3}),
                // 5th Tier
                new JobOrbUpgrade()
                    .Id(5, JobId.Fighter)
                    .Location(3, 5)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
                    .HasUnlockDependency(4),
                new JobOrbUpgrade()
                    .Id(6, JobId.Fighter)
                    .Location(5, 5)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
                    .HasUnlockDependency(4),
                // 6th Tier
                new JobOrbUpgrade()
                    .Id(7, JobId.Fighter)
                    .Location(2, 6)
                    .BloodOrbCost(1200)
                    .Unlocks(OrbGainParamType.JobHpMax, 30)
                    .HasUnlockDependency(5),
                new JobOrbUpgrade()
                    .Id(8, JobId.Fighter)
                    .Location(3, 6)
                    .BloodOrbCost(1200)
                    .Unlocks(OrbGainParamType.JobHpMax, 35)
                    .HasUnlockDependency(5),
                new JobOrbUpgrade()
                    .Id(9, JobId.Fighter)
                    .Location(5, 6)
                    .BloodOrbCost(1200)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 15)
                    .HasUnlockDependency(6),
                new JobOrbUpgrade()
                    .Id(10, JobId.Fighter)
                    .Location(6, 6)
                    .BloodOrbCost(1200)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
                    .HasUnlockDependency(6),
                // 7th Tier
                new JobOrbUpgrade()
                    .Id(11, JobId.Fighter)
                    .Location(1, 7)
                    .BloodOrbCost(1600)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
                    .HasUnlockDependency(7),
                new JobOrbUpgrade()
                    .Id(12, JobId.Fighter)
                    .Location(2, 7)
                    .BloodOrbCost(1200)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
                    .HasUnlockDependency(7),
                new JobOrbUpgrade()
                    .Id(13, JobId.Fighter)
                    .Location(3, 7)
                    .BloodOrbCost(1200)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
                    .HasUnlockDependency(8),
                new JobOrbUpgrade()
                    .Id(14, JobId.Fighter)
                    .Location(5, 7)
                    .BloodOrbCost(1600)
                    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
                    .HasUnlockDependency(9),
                new JobOrbUpgrade()
                    .Id(15, JobId.Fighter)
                    .Location(7, 7)
                    .BloodOrbCost(1200)
                    .Unlocks(OrbGainParamType.JobHpMax, 30)
                    .HasUnlockDependency(10),
                // 8th Tier
                new JobOrbUpgrade()
                    .Id(16, JobId.Fighter)
                    .Location(1, 8)
                    .BloodOrbCost(1200)
                    .Unlocks(SecretAbility.OnslaughtSlayer)
                    .HasUnlockDependency(11),
                new JobOrbUpgrade()
                    .Id(17, JobId.Fighter)
                    .Location(2, 8)
                    .BloodOrbCost(1600)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
                    .HasUnlockDependency(12),
                new JobOrbUpgrade()
                    .Id(18, JobId.Fighter)
                    .Location(3, 8)
                    .BloodOrbCost(1600)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
                    .HasUnlockDependency(13),
                new JobOrbUpgrade()
                    .Id(19, JobId.Fighter)
                    .Location(5, 8)
                    .BloodOrbCost(1600)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
                    .HasUnlockDependency(14),
                new JobOrbUpgrade()
                    .Id(20, JobId.Fighter)
                    .Location(6, 8)
                    .BloodOrbCost(1600)
                    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
                    .HasUnlockDependency(14),
                new JobOrbUpgrade()
                    .Id(21, JobId.Fighter)
                    .Location(7, 8)
                    .BloodOrbCost(1200)
                    .Unlocks(SecretAbility.DemolishingStrikeSlayer)
                    .HasUnlockDependency(15),
                // 9th Tier
                new JobOrbUpgrade()
                    .Id(22, JobId.Fighter)
                    .Location(2, 9)
                    .BloodOrbCost(1700)
                    .Unlocks(SecretAbility.GougeEradicate)
                    .HasUnlockDependency(17),
                new JobOrbUpgrade()
                    .Id(23, JobId.Fighter)
                    .Location(3, 9)
                    .BloodOrbCost(1500)
                    .Unlocks(OrbGainParamType.AllJobsHpMax, 30)
                    .HasUnlockDependency(18),
                new JobOrbUpgrade()
                    .Id(24, JobId.Fighter)
                    .Location(5, 9)
                    .BloodOrbCost(2000)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 15)
                    .HasUnlockDependency(19),
                new JobOrbUpgrade()
                    .Id(25, JobId.Fighter)
                    .Location(6, 9)
                    .BloodOrbCost(1400)
                    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1)
                    .HasUnlockDependency(20),
                // 10th Tier
                new JobOrbUpgrade()
                    .Id(26, JobId.Fighter)
                    .Location(4, 10)
                    .BloodOrbCost(1800)
                    .Unlocks(SecretAbility.CrushingBlow)
                    .HasUnlockDependencies(new List<uint> { 23, 24}),
                // 11th Tier
                new JobOrbUpgrade()
                    .Id(27, JobId.Fighter)
                    .Location(4, 11)
                    .BloodOrbCost(500)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack , 1)
                    .HasUnlockDependency(26),
                // 12th Tier
                new JobOrbUpgrade()
                    .Id(28, JobId.Fighter)
                    .Location(3, 12)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobStaminaMax , 10)
                    .HasUnlockDependency(27),
                new JobOrbUpgrade()
                    .Id(29, JobId.Fighter)
                    .Location(4, 12)
                    .BloodOrbCost(500)
                    .Unlocks(OrbGainParamType.JobHpMax , 25)
                    .HasUnlockDependency(27),
                new JobOrbUpgrade()
                    .Id(30, JobId.Fighter)
                    .Location(5, 12)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobMagicalDefence , 1)
                    .HasUnlockDependency(27),
                // 13th Tier
                new JobOrbUpgrade()
                    .Id(31, JobId.Fighter)
                    .Location(4, 13)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack , 1)
                    .HasUnlockDependency(29),
                // 14th Tier
                new JobOrbUpgrade()
                    .Id(32, JobId.Fighter)
                    .Location(1, 14)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobHpMax , 25)
                    .HasUnlockDependency(28),
                new JobOrbUpgrade()
                    .Id(33, JobId.Fighter)
                    .Location(2, 14)
                    .BloodOrbCost(1000)
                    .Unlocks(OrbGainParamType.JobStaminaMax , 10)
                    .HasUnlockDependency(28),
                new JobOrbUpgrade()
                    .Id(34, JobId.Fighter)
                    .Location(3, 14)
                    .BloodOrbCost(1000)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence , 1)
                    .HasUnlockDependency(28),
                new JobOrbUpgrade()
                    .Id(35, JobId.Fighter)
                    .Location(5, 14)
                    .BloodOrbCost(1000)
                    .Unlocks(OrbGainParamType.JobMagicalDefence , 25)
                    .HasUnlockDependency(30),
                new JobOrbUpgrade()
                    .Id(36, JobId.Fighter)
                    .Location(6, 14)
                    .BloodOrbCost(1000)
                    .Unlocks(OrbGainParamType.JobMagicalAttack , 1)
                    .HasUnlockDependency(30),
                new JobOrbUpgrade()
                    .Id(37, JobId.Fighter)
                    .Location(7, 14)
                    .BloodOrbCost(1000)
                    .Unlocks(OrbGainParamType.JobHpMax , 25)
                    .HasUnlockDependency(30),
                // 15th Tier
                new JobOrbUpgrade()
                    .Id(38, JobId.Fighter)
                    .Location(1, 15)
                    .BloodOrbCost(1500)
                    .Unlocks(OrbGainParamType.JobStaminaMax , 10)
                    .HasUnlockDependency(32),
                new JobOrbUpgrade()
                    .Id(39, JobId.Fighter)
                    .Location(2, 15)
                    .BloodOrbCost(1000)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack , 1)
                    .HasUnlockDependency(33),
                new JobOrbUpgrade()
                    .Id(40, JobId.Fighter)
                    .Location(3, 15)
                    .BloodOrbCost(1600)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence , 1)
                    .HasUnlockDependency(34),
                new JobOrbUpgrade()
                    .Id(41, JobId.Fighter)
                    .Location(5, 15)
                    .BloodOrbCost(1800)
                    .Unlocks(OrbGainParamType.JobMagicalDefence , 1)
                    .HasUnlockDependency(35),
                new JobOrbUpgrade()
                    .Id(42, JobId.Fighter)
                    .Location(6, 15)
                    .BloodOrbCost(1800)
                    .Unlocks(OrbGainParamType.JobMagicalAttack , 1)
                    .HasUnlockDependency(36),
                new JobOrbUpgrade()
                    .Id(43, JobId.Fighter)
                    .Location(7, 15)
                    .BloodOrbCost(1000)
                    .Unlocks(OrbGainParamType.JobHpMax , 25)
                    .HasUnlockDependency(37),
                // 16th Tier
                new JobOrbUpgrade()
                    .Id(43, JobId.Fighter)
                    .Location(2, 16)
                    .BloodOrbCost(1700)
                    .Unlocks(SecretAbility.DireOnslaughtSlayer)
                    .HasUnlockDependency(39),
                new JobOrbUpgrade()
                    .Id(44, JobId.Fighter)
                    .Location(3, 16)
                    .BloodOrbCost(1800)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
                    .HasUnlockDependency(40),
                new JobOrbUpgrade()
                    .Id(45, JobId.Fighter)
                    .Location(5, 16)
                    .BloodOrbCost(1000)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
                    .HasUnlockDependency(41),
                new JobOrbUpgrade()
                    .Id(46, JobId.Fighter)
                    .Location(6, 16)
                    .BloodOrbCost(1700)
                    .Unlocks(SecretAbility.FirmShield)
                    .HasUnlockDependency(42),
                // 17th Tier
                new JobOrbUpgrade()
                    .Id(47, JobId.Fighter)
                    .Location(3, 17)
                    .BloodOrbCost(1600)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
                    .HasUnlockDependency(44),
                new JobOrbUpgrade()
                    .Id(48, JobId.Fighter)
                    .Location(5, 17)
                    .BloodOrbCost(1400)
                    .Unlocks(OrbGainParamType.JobHpMax, 25)
                    .HasUnlockDependency(45),
                // 18th Tier
                new JobOrbUpgrade()
                    .Id(49, JobId.Fighter)
                    .Location(3, 18)
                    .BloodOrbCost(1600)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
                    .HasUnlockDependency(47),
                new JobOrbUpgrade()
                    .Id(50, JobId.Fighter)
                    .Location(5, 18)
                    .BloodOrbCost(1600)
                    .Unlocks(OrbGainParamType.JobHpMax, 25)
                    .HasUnlockDependency(48),
                // 19th Tier
                new JobOrbUpgrade()
                    .Id(51, JobId.Fighter)
                    .Location(3, 19)
                    .BloodOrbCost(1700)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
                    .HasUnlockDependency(49),
                new JobOrbUpgrade()
                    .Id(52, JobId.Fighter)
                    .Location(5, 19)
                    .BloodOrbCost(1600)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
                    .HasUnlockDependency(50),
                // 20th Tier
                new JobOrbUpgrade()
                    .Id(53, JobId.Fighter)
                    .Location(4, 20)
                    .BloodOrbCost(1400)
                    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1)
                    .HasUnlockDependencies(new List<uint>() {51, 52 }),
            },
            #endregion
            #region SEEKER 
            [JobId.Seeker] = new List<JobOrbUpgrade>()
            {
                
            },
            #endregion
            [JobId.Hunter] = new List<JobOrbUpgrade>()
            {
                // 1st Tier
                new JobOrbUpgrade()
                    .Id(0, JobId.Hunter)
                    .Location(4, 1)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 10),
                // 2nd Tier
                new JobOrbUpgrade()
                    .Id(1, JobId.Hunter)
                    .Location(3, 2)
                    .BloodOrbCost(500)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
                    .HasUnlockDependency(0),
                new JobOrbUpgrade()
                    .Id(2, JobId.Hunter)
                    .Location(5, 2)
                    .BloodOrbCost(500)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
                    .HasUnlockDependency(0),
                // 3rd Tier
                new JobOrbUpgrade()
                    .Id(3, JobId.Hunter)
                    .Location(2, 3)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
                    .HasUnlockDependencies(new List<uint>{1, 2}),
                new JobOrbUpgrade()
                    .Id(4, JobId.Hunter)
                    .Location(4, 3)
                    .BloodOrbCost(1200)
                    .Unlocks(CustomSkillId.SkyBurstShot)
                    .HasUnlockDependencies(new List<uint>{1, 2}),
                new JobOrbUpgrade()
                    .Id(5, JobId.Hunter)
                    .Location(6, 3)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
                    .HasUnlockDependencies(new List<uint>{1, 2}),
                // 4th Tier
                new JobOrbUpgrade()
                    .Id(6, JobId.Hunter)
                    .Location(2, 4)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobHpMax, 30)
                    .HasUnlockDependency(3),
                new JobOrbUpgrade()
                    .Id(7, JobId.Hunter)
                    .Location(4, 4)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
                    .HasUnlockDependency(4),
                new JobOrbUpgrade()
                    .Id(8, JobId.Hunter)
                    .Location(6, 4)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
                    .HasUnlockDependency(5),
                // 5th Tier
                new JobOrbUpgrade()
                    .Id(9, JobId.Hunter)
                    .Location(1, 5)
                    .BloodOrbCost(1200)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
                    .HasUnlockDependency(6),
                new JobOrbUpgrade()
                    .Id(10, JobId.Hunter)
                    .Location(2, 5)
                    .BloodOrbCost(1200)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
                    .HasUnlockDependency(6),
                new JobOrbUpgrade()
                    .Id(11, JobId.Hunter)
                    .Location(4, 5)
                    .BloodOrbCost(1200)
                    .Unlocks(SecretAbility.ArcherySlayer)
                    .HasUnlockDependency(7),
                new JobOrbUpgrade()
                    .Id(12, JobId.Hunter)
                    .Location(6, 5)
                    .BloodOrbCost(1200)
                    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
                    .HasUnlockDependency(8),
                new JobOrbUpgrade()
                    .Id(13, JobId.Hunter)
                    .Location(7, 5)
                    .BloodOrbCost(1400)
                    .Unlocks(OrbGainParamType.JobHpMax, 30)
                    .HasUnlockDependency(8),
                // 6th Tier
                new JobOrbUpgrade()
                    .Id(14, JobId.Hunter)
                    .Location(1, 6)
                    .BloodOrbCost(1200)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 15)
                    .HasUnlockDependency(9),
                new JobOrbUpgrade()
                    .Id(15, JobId.Hunter)
                    .Location(2, 6)
                    .BloodOrbCost(1200)
                    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1)
                    .HasUnlockDependency(10),
                new JobOrbUpgrade()
                    .Id(16, JobId.Hunter)
                    .Location(4, 6)
                    .BloodOrbCost(1800)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
                    .HasUnlockDependency(11),
                new JobOrbUpgrade()
                    .Id(17, JobId.Hunter)
                    .Location(6, 6)
                    .BloodOrbCost(1800)
                    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 10)
                    .HasUnlockDependency(12),
                new JobOrbUpgrade()
                    .Id(18, JobId.Hunter)
                    .Location(7, 6)
                    .BloodOrbCost(1800)
                    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
                    .HasUnlockDependency(13),
                // 7th Tier
                new JobOrbUpgrade()
                    .Id(19, JobId.Hunter)
                    .Location(1, 7)
                    .BloodOrbCost(1600)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 15)
                    .HasUnlockDependency(14),
                new JobOrbUpgrade()
                    .Id(20, JobId.Hunter)
                    .Location(4, 7)
                    .BloodOrbCost(1800)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
                    .HasUnlockDependencies(new List<uint>() {15, 17}),
                new JobOrbUpgrade()
                    .Id(21, JobId.Hunter)
                    .Location(7, 7)
                    .BloodOrbCost(1700)
                    .Unlocks(OrbGainParamType.JobHpMax, 30)
                    .HasUnlockDependency(18),
                // 8th Tier
                new JobOrbUpgrade()
                    .Id(22, JobId.Hunter)
                    .Location(1, 8)
                    .BloodOrbCost(1700)
                    .Unlocks(SecretAbility.ExplodingArrowFury)
                    .HasUnlockDependency(19),
                new JobOrbUpgrade()
                    .Id(23, JobId.Hunter)
                    .Location(3, 8)
                    .BloodOrbCost(1800)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
                    .HasUnlockDependency(20),
                new JobOrbUpgrade()
                    .Id(24, JobId.Hunter)
                    .Location(5, 8)
                    .BloodOrbCost(1600)
                    .Unlocks(OrbGainParamType.JobHpMax, 35)
                    .HasUnlockDependency(20),
                new JobOrbUpgrade()
                    .Id(25, JobId.Hunter)
                    .Location(7, 8)
                    .BloodOrbCost(2000)
                    .Unlocks(SecretAbility.ArrowheadStrikeFury)
                    .HasUnlockDependency(21),
                // 9th Tier
                new JobOrbUpgrade()
                    .Id(25, JobId.Hunter)
                    .Location(4, 9)
                    .BloodOrbCost(1800)
                    .Unlocks(SecretAbility.RigidStance)
                    .HasUnlockDependencies(new List<uint> {23, 24}),
            },
            [JobId.Priest] = new List<JobOrbUpgrade>() { },
            [JobId.ShieldSage] = new List<JobOrbUpgrade>() { },
            [JobId.Sorcerer] = new List<JobOrbUpgrade>() { },

            #region WARRIOR
            [JobId.Warrior] = new List<JobOrbUpgrade>()
            {
                // First Layer
                new JobOrbUpgrade()
                    .Id(0, JobId.Warrior)
                    .Location(4, 1)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 10),
                // Second Layer
                new JobOrbUpgrade()
                    .Id(1, JobId.Warrior)
                    .Location(3, 2)
                    .BloodOrbCost(500)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
                    .HasUnlockDependency(0),
                new JobOrbUpgrade()
                    .Id(2, JobId.Warrior)
                    .Location(4, 2)
                    .BloodOrbCost(500)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
                    .HasUnlockDependency(0),
                new JobOrbUpgrade()
                    .Id(3, JobId.Warrior)
                    .Location(5, 2)
                    .BloodOrbCost(500)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
                    .HasUnlockDependency(0),
                // Third Layer
                new JobOrbUpgrade()
                    .Id(5, JobId.Warrior)
                    .Location(2, 3)
                    .BloodOrbCost(1200)
                    .HasUnlockDependency(1),
                new JobOrbUpgrade()
                    .Id(6, JobId.Warrior)
                    .Location(3, 3)
                    .BloodOrbCost(800)
                    .HasUnlockDependency(1),
                new JobOrbUpgrade()
                    .Id(7, JobId.Warrior)
                    .Location(4, 3)
                    .BloodOrbCost(800)
                    .Unlocks(CustomSkillId.GreatGougingFang)
                    .HasUnlockDependency(2),
                new JobOrbUpgrade()
                    .Id(8, JobId.Warrior)
                    .Location(5, 3)
                    .BloodOrbCost(800)
                    .HasUnlockDependency(3),
                new JobOrbUpgrade()
                    .Id(9, JobId.Warrior)
                    .Location(6, 3)
                    .BloodOrbCost(1200)
                    .HasUnlockDependency(3),
            },
            #endregion

            [JobId.ElementArcher] = new List<JobOrbUpgrade>() { },
            [JobId.Alchemist] = new List<JobOrbUpgrade>() { },
            [JobId.SpiritLancer] = new List<JobOrbUpgrade>() { },

            #region HIGHSCEPTER
            [JobId.HighScepter] = new List<JobOrbUpgrade>()
            {
                // 1st Layer
                new JobOrbUpgrade()
                    .Id(0, JobId.HighScepter)
                    .Location(4, 1)
                    .BloodOrbCost(500)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
                    .HasQuestDependency(60300401),
                // 2nd Layer
                new JobOrbUpgrade()
                    .Id(1, JobId.HighScepter)
                    .Location(3, 2)
                    .BloodOrbCost(700)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
                    .HasUnlockDependency(0),
                new JobOrbUpgrade()
                    .Id(2, JobId.HighScepter)
                    .Location(5, 2)
                    .BloodOrbCost(700)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
                    .HasUnlockDependency(0),
                // 3rd Layer
                new JobOrbUpgrade()
                    .Id(3, JobId.HighScepter)
                    .Location(2, 3)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobHpMax, 50)
                    .HasUnlockDependency(1),
                new JobOrbUpgrade()
                    .Id(4, JobId.HighScepter)
                    .Location(3, 3)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
                    .HasUnlockDependency(1),
                new JobOrbUpgrade()
                    .Id(5, JobId.HighScepter)
                    .Location(5, 3)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
                    .HasUnlockDependency(2),
                new JobOrbUpgrade()
                    .Id(6, JobId.HighScepter)
                    .Location(6, 3)
                    .BloodOrbCost(800)
                    .Unlocks(OrbGainParamType.JobHpMax, 50)
                    .HasUnlockDependency(2),
                // 4th Layer
                new JobOrbUpgrade()
                    .Id(7, JobId.HighScepter)
                    .Location(2, 4)
                    .BloodOrbCost(1000)
                    .Unlocks(OrbGainParamType.JobHpMax, 50)
                    .HasUnlockDependency(3),
                new JobOrbUpgrade()
                    .Id(8, JobId.HighScepter)
                    .Location(4, 4)
                    .BloodOrbCost(1000)
                    .Unlocks(CustomSkillId.EclipseBright)
                    .HasUnlockDependencies(new List<uint>() {4, 5}),
                new JobOrbUpgrade()
                    .Id(9, JobId.HighScepter)
                    .Location(6, 4)
                    .BloodOrbCost(1000)
                    .Unlocks(OrbGainParamType.JobHpMax, 50)
                    .HasUnlockDependency(6),
                // 5th Layer
                new JobOrbUpgrade()
                    .Id(10, JobId.HighScepter)
                    .Location(2, 5)
                    .BloodOrbCost(1100)
                    .Unlocks(OrbGainParamType.JobHpMax, 50)
                    .HasUnlockDependency(7),
                new JobOrbUpgrade()
                    .Id(11, JobId.HighScepter)
                    .Location(3, 5)
                    .BloodOrbCost(1100)
                    .Unlocks(OrbGainParamType.JobMagicalDefence, 2)
                    .HasUnlockDependency(7),
                new JobOrbUpgrade()
                    .Id(12, JobId.HighScepter)
                    .Location(5, 5)
                    .BloodOrbCost(1100)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence, 2)
                    .HasUnlockDependency(9),
                new JobOrbUpgrade()
                    .Id(13, JobId.HighScepter)
                    .Location(6, 5)
                    .BloodOrbCost(1100)
                    .Unlocks(OrbGainParamType.JobHpMax, 50)
                    .HasUnlockDependency(9),
                // 6th Layer
                new JobOrbUpgrade()
                    .Id(14, JobId.HighScepter)
                    .Location(3, 6)
                    .BloodOrbCost(1200)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
                    .HasUnlockDependency(11),
                new JobOrbUpgrade()
                    .Id(15, JobId.HighScepter)
                    .Location(5, 6)
                    .BloodOrbCost(1200)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
                    .HasUnlockDependency(12),
                // 7th Layer
                new JobOrbUpgrade()
                    .Id(16, JobId.HighScepter)
                    .Location(1, 7)
                    .BloodOrbCost(1300)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 20)
                    .HasUnlockDependency(14),
                new JobOrbUpgrade()
                    .Id(17, JobId.HighScepter)
                    .Location(3, 7)
                    .BloodOrbCost(1500)
                    .Unlocks(SecretAbility.ArcSlashSlayer)
                    .HasUnlockDependency(14),
                new JobOrbUpgrade()
                    .Id(18, JobId.HighScepter)
                    .Location(5, 7)
                    .BloodOrbCost(1500)
                    .Unlocks(SecretAbility.SkySlashSlayer)
                    .HasUnlockDependency(15),
                new JobOrbUpgrade()
                    .Id(19, JobId.HighScepter)
                    .Location(7, 7)
                    .BloodOrbCost(1300)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 20)
                    .HasUnlockDependency(15),
                // 8th Layer
                new JobOrbUpgrade()
                    .Id(20, JobId.HighScepter)
                    .Location(1, 8)
                    .BloodOrbCost(1500)
                    .Unlocks(OrbGainParamType.JobHpMax, 50)
                    .HasUnlockDependency(16),
                new JobOrbUpgrade()
                    .Id(21, JobId.HighScepter)
                    .Location(2, 8)
                    .BloodOrbCost(1300)
                    .Unlocks(OrbGainParamType.JobMagicalDefence, 2)
                    .HasUnlockDependency(16),
                new JobOrbUpgrade()
                    .Id(22, JobId.HighScepter)
                    .Location(4, 8)
                    .BloodOrbCost(1500)
                    .Unlocks(SecretAbility.QuadrupleSlash)
                    .HasUnlockDependencies(new List<uint>() {17, 18}),
                new JobOrbUpgrade()
                    .Id(23, JobId.HighScepter)
                    .Location(6, 8)
                    .BloodOrbCost(1300)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence, 2)
                    .HasUnlockDependency(19),
                new JobOrbUpgrade()
                    .Id(24, JobId.HighScepter)
                    .Location(7, 8)
                    .BloodOrbCost(1500)
                    .Unlocks(OrbGainParamType.JobHpMax, 50)
                    .HasUnlockDependency(19),
                // 9th Layer
                new JobOrbUpgrade()
                    .Id(25, JobId.HighScepter)
                    .Location(2, 9)
                    .BloodOrbCost(1500)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
                    .HasUnlockDependency(21),
                new JobOrbUpgrade()
                    .Id(26, JobId.HighScepter)
                    .Location(6, 9)
                    .BloodOrbCost(1500)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
                    .HasUnlockDependency(23),
                // 10th Layer
                new JobOrbUpgrade()
                    .Id(27, JobId.HighScepter)
                    .Location(1, 10)
                    .BloodOrbCost(1800)
                    .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2)
                    .HasUnlockDependency(20),
                new JobOrbUpgrade()
                    .Id(28, JobId.HighScepter)
                    .Location(4, 10)
                    .BloodOrbCost(1800)
                    .Unlocks(SecretAbility.FallingSlashSlayer)
                    .HasUnlockDependencies(new List<uint> {25, 26}),
                new JobOrbUpgrade()
                    .Id(29, JobId.HighScepter)
                    .Location(7, 10)
                    .BloodOrbCost(1800)
                    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2)
                    .HasUnlockDependency(24),
                // 11th Layer
                new JobOrbUpgrade()
                    .Id(30, JobId.HighScepter)
                    .Location(4, 11)
                    .BloodOrbCost(2000)
                    .Unlocks(OrbGainParamType.JobHpMax, 50)
                    .HasUnlockDependency(28),
                // 12th Layer
                new JobOrbUpgrade()
                    .Id(31, JobId.HighScepter)
                    .Location(1, 12)
                    .BloodOrbCost(2200)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
                    .HasUnlockDependency(30),
                new JobOrbUpgrade()
                    .Id(32, JobId.HighScepter)
                    .Location(7, 12)
                    .BloodOrbCost(2200)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
                    .HasUnlockDependency(30),
                // 13th Layer
                new JobOrbUpgrade()
                    .Id(33, JobId.HighScepter)
                    .Location(1, 13)
                    .BloodOrbCost(2400)
                    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
                    .HasUnlockDependency(31),
                new JobOrbUpgrade()
                    .Id(34, JobId.HighScepter)
                    .Location(2, 13)
                    .BloodOrbCost(2400)
                    .Unlocks(OrbGainParamType.JobHpMax, 50)
                    .HasUnlockDependency(31),
                new JobOrbUpgrade()
                    .Id(35, JobId.HighScepter)
                    .Location(3, 13)
                    .BloodOrbCost(2400)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 15)
                    .HasUnlockDependency(31),
                new JobOrbUpgrade()
                    .Id(36, JobId.HighScepter)
                    .Location(5, 13)
                    .BloodOrbCost(2400)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 15)
                    .HasUnlockDependency(32),
                new JobOrbUpgrade()
                    .Id(37, JobId.HighScepter)
                    .Location(6, 13)
                    .BloodOrbCost(2400)
                    .Unlocks(OrbGainParamType.JobHpMax, 50)
                    .HasUnlockDependency(32),
                new JobOrbUpgrade()
                    .Id(38, JobId.HighScepter)
                    .Location(7, 13)
                    .BloodOrbCost(2400)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
                    .HasUnlockDependency(32),
                // 14th Layer
                new JobOrbUpgrade()
                    .Id(39, JobId.HighScepter)
                    .Location(1, 14)
                    .BloodOrbCost(2600)
                    .Unlocks(OrbGainParamType.JobMagicalDefence, 2)
                    .HasUnlockDependency(33),
                new JobOrbUpgrade()
                    .Id(40, JobId.HighScepter)
                    .Location(2, 14)
                    .BloodOrbCost(2600)
                    .Unlocks(OrbGainParamType.JobHpMax, 50)
                    .HasUnlockDependency(34),
                new JobOrbUpgrade()
                    .Id(41, JobId.HighScepter)
                    .Location(3, 14)
                    .BloodOrbCost(2600)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
                    .HasUnlockDependency(35),
                new JobOrbUpgrade()
                    .Id(42, JobId.HighScepter)
                    .Location(5, 14)
                    .BloodOrbCost(2600)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
                    .HasUnlockDependency(36),
                new JobOrbUpgrade()
                    .Id(43, JobId.HighScepter)
                    .Location(6, 14)
                    .BloodOrbCost(2600)
                    .Unlocks(OrbGainParamType.JobHpMax, 50)
                    .HasUnlockDependency(37),
                new JobOrbUpgrade()
                    .Id(44, JobId.HighScepter)
                    .Location(7, 14)
                    .BloodOrbCost(2600)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence, 2)
                    .HasUnlockDependency(38),
                // 15th Layer
                new JobOrbUpgrade()
                    .Id(45, JobId.HighScepter)
                    .Location(1, 15)
                    .BloodOrbCost(2800)
                    .Unlocks(OrbGainParamType.JobMagicalDefence, 2)
                    .HasUnlockDependency(39),
                new JobOrbUpgrade()
                    .Id(46, JobId.HighScepter)
                    .Location(2, 15)
                    .BloodOrbCost(2800)
                    .Unlocks(OrbGainParamType.JobHpMax, 50)
                    .HasUnlockDependency(40),
                new JobOrbUpgrade()
                    .Id(47, JobId.HighScepter)
                    .Location(6, 15)
                    .BloodOrbCost(2800)
                    .Unlocks(OrbGainParamType.JobHpMax, 50)
                    .HasUnlockDependency(43),
                new JobOrbUpgrade()
                    .Id(48, JobId.HighScepter)
                    .Location(7, 15)
                    .BloodOrbCost(2800)
                    .Unlocks(OrbGainParamType.JobPhysicalDefence, 2)
                    .HasUnlockDependency(44),
                // 16th Layer
                new JobOrbUpgrade()
                    .Id(49, JobId.HighScepter)
                    .Location(1, 16)
                    .BloodOrbCost(3000)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
                    .HasUnlockDependency(45),
                new JobOrbUpgrade()
                    .Id(50, JobId.HighScepter)
                    .Location(2, 16)
                    .BloodOrbCost(3000)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
                    .HasUnlockDependency(46),
                new JobOrbUpgrade()
                    .Id(51, JobId.HighScepter)
                    .Location(6, 16)
                    .BloodOrbCost(3000)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
                    .HasUnlockDependency(47),
                new JobOrbUpgrade()
                    .Id(52, JobId.HighScepter)
                    .Location(7, 16)
                    .BloodOrbCost(3000)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
                    .HasUnlockDependency(48),
                // 17th Layer
                new JobOrbUpgrade()
                    .Id(53, JobId.HighScepter)
                    .Location(1, 17)
                    .BloodOrbCost(3300)
                    .Unlocks(SecretAbility.OrdinaryAttack)
                    .HasUnlockDependency(49),
                new JobOrbUpgrade()
                    .Id(54, JobId.HighScepter)
                    .Location(7, 17)
                    .BloodOrbCost(3300)
                    .Unlocks(SecretAbility.Respiration)
                    .HasUnlockDependency(52),
                // 18th Layer
                new JobOrbUpgrade()
                    .Id(55, JobId.HighScepter)
                    .Location(2, 18)
                    .BloodOrbCost(3300)
                    .Unlocks(OrbGainParamType.JobMagicalAttack, 3)
                    .HasUnlockDependency(50),
                new JobOrbUpgrade()
                    .Id(56, JobId.HighScepter)
                    .Location(6, 18)
                    .BloodOrbCost(3300)
                    .Unlocks(OrbGainParamType.JobPhysicalAttack, 3)
                    .HasUnlockDependency(51),
                // 19th Layer
                new JobOrbUpgrade()
                    .Id(57, JobId.HighScepter)
                    .Location(4, 19)
                    .BloodOrbCost(3300)
                    .Unlocks(OrbGainParamType.JobStaminaMax, 20)
                    .HasUnlockDependencies(new List<uint>() {55, 56}),
                // 20th Layer
                new JobOrbUpgrade()
                    .Id(58, JobId.HighScepter)
                    .Location(3, 20)
                    .BloodOrbCost(4000)
                    .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2)
                    .HasUnlockDependency(57),
                new JobOrbUpgrade()
                    .Id(59, JobId.HighScepter)
                    .Location(5, 20)
                    .BloodOrbCost(4000)
                    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2)
                    .HasUnlockDependency(57),
            },
            #endregion
        };
#endif
    }
}

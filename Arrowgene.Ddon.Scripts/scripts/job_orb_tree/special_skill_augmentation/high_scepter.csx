#load "libs.csx"

public class SkillAugmentation : ISkillAugmentation
{
    public override JobId JobId => JobId.HighScepter;
    public override OrbTreeType OrbTreeType => OrbTreeType.Season3;
}

var skillAugmentation = new SkillAugmentation();

#region TIER1
// Row 1
skillAugmentation.AddNode(1)
    .Location(4, 1)
    .BloodOrbCost(3700)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10);
// Row 2
skillAugmentation.AddNode(2)
    .Location(3, 2)
    .BloodOrbCost(3900)
    .HasUnlockDependencies(1)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
skillAugmentation.AddNode(3)
    .Location(5, 2)
    .BloodOrbCost(3900)
    .HasUnlockDependencies(1)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
// Row 3
skillAugmentation.AddNode(4)
    .Location(2, 3)
    .BloodOrbCost(4000)
    .HasUnlockDependencies(2)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
skillAugmentation.AddNode(5)
    .Location(6, 3)
    .BloodOrbCost(4000)
    .HasUnlockDependencies(3)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
// Row 4
skillAugmentation.AddNode(6)
    .Location(1, 4)
    .BloodOrbCost(4200)
    .HasUnlockDependencies(4)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2);
skillAugmentation.AddNode(7)
    .Location(7, 4)
    .BloodOrbCost(4200)
    .HasUnlockDependencies(5)
    .HasSpecialConditionDependencies(1)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2);
// Row 5
skillAugmentation.AddNode(8)
    .Location(1, 5)
    .HighOrbCost(400)
    .HasUnlockDependencies(6)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 2);
skillAugmentation.AddNode(9)
    .Location(2, 5)
    .BloodOrbCost(4400)
    .HasUnlockDependencies(6)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
skillAugmentation.AddNode(10)
    .Location(6, 5)
    .BloodOrbCost(4400)
    .HasUnlockDependencies(7)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
skillAugmentation.AddNode(11)
    .Location(7, 5)
    .HighOrbCost(400)
    .HasUnlockDependencies(7)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 2);
// Row 6
skillAugmentation.AddNode(12)
    .Location(1, 6)
    .HighOrbCost(400)
    .HasUnlockDependencies(8)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 2);
skillAugmentation.AddNode(13)
    .Location(3, 6)
    .BloodOrbCost(4600)
    .HasUnlockDependencies(9)
    .Unlocks(OrbGainParamType.JobStaminaMax, 20);
skillAugmentation.AddNode(14)
    .Location(5, 6)
    .BloodOrbCost(4600)
    .HasUnlockDependencies(10)
    .Unlocks(OrbGainParamType.JobStaminaMax, 20);
skillAugmentation.AddNode(15)
    .Location(7, 6)
    .HighOrbCost(400)
    .HasUnlockDependencies(11)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 2);
// Row 7
skillAugmentation.AddNode(16)
    .Location(1, 7)
    .HighOrbCost(500)
    .HasUnlockDependencies(12)
    .Unlocks(AbilityId.QuadrupleSlashAbsorption);
skillAugmentation.AddNode(17)
    .Location(4, 7)
    .BloodOrbCost(4800)
    .HasUnlockDependencies(13, 14)
    .HasSpecialConditionDependencies(2)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
skillAugmentation.AddNode(18)
    .Location(7, 7)
    .HighOrbCost(500)
    .HasUnlockDependencies(15)
    .Unlocks(AbilityId.ArcSlashDestroyer);
// Row 8
skillAugmentation.AddNode(19)
    .Location(3, 8)
    .HighOrbCost(400)
    .HasUnlockDependencies(17)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
skillAugmentation.AddNode(20)
    .Location(4, 8)
    .BloodOrbCost(5000)
    .HasUnlockDependencies(17)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
skillAugmentation.AddNode(21)
    .Location(5, 8)
    .HighOrbCost(400)
    .HasUnlockDependencies(17)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
// Row 9
skillAugmentation.AddNode(22)
    .Location(2, 9)
    .HighOrbCost(400)
    .HasUnlockDependencies(19)
    .HasSpecialConditionDependencies(3)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2);
skillAugmentation.AddNode(23)
    .Location(4, 9)
    .BloodOrbCost(5200)
    .HasUnlockDependencies(20)
    .HasSpecialConditionDependencies(3)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
skillAugmentation.AddNode(24)
    .Location(6, 9)
    .HighOrbCost(400)
    .HasUnlockDependencies(21)
    .HasSpecialConditionDependencies(3)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2);
// Row 10
skillAugmentation.AddNode(25)
    .Location(1, 10)
    .HighOrbCost(500)
    .HasUnlockDependencies(22)
    .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2);
skillAugmentation.AddNode(26)
    .Location(4, 10)
    .BloodOrbCost(5500)
    .HasUnlockDependencies(23)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
skillAugmentation.AddNode(27)
    .Location(7, 10)
    .HighOrbCost(500)
    .HasUnlockDependencies(24)
    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2);
#endregion

return skillAugmentation;

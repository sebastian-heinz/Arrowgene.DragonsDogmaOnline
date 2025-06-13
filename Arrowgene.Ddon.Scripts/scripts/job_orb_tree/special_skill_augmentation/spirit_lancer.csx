#load "libs.csx"

public class SkillAugmentation : ISkillAugmentation
{
    public override JobId JobId => JobId.SpiritLancer;
    public override OrbTreeType OrbTreeType => OrbTreeType.Season3;
}

var skillAugmentation = new SkillAugmentation();

#region TIER1
// Row 1
skillAugmentation.AddNode(1)
    .Location(2, 1)
    .BloodOrbCost(3100)
    .Unlocks(OrbGainParamType.JobHpMax, 30);
skillAugmentation.AddNode(2)
    .Location(4, 1)
    .BloodOrbCost(3300)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
skillAugmentation.AddNode(3)
    .Location(6, 1)
    .BloodOrbCost(3100)
    .Unlocks(OrbGainParamType.JobHpMax, 30);
// Row 2
skillAugmentation.AddNode(4)
    .Location(3, 2)
    .BloodOrbCost(3500)
    .HasUnlockDependencies(1)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
skillAugmentation.AddNode(5)
    .Location(4, 2)
    .BloodOrbCost(3500)
    .HasUnlockDependencies(2)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
skillAugmentation.AddNode(6)
    .Location(5, 2)
    .BloodOrbCost(3500)
    .HasUnlockDependencies(3)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
// Row 3
skillAugmentation.AddNode(7)
    .Location(2, 3)
    .HighOrbCost(200)
    .HasUnlockDependencies(4)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
skillAugmentation.AddNode(8)
    .Location(4, 3)
    .BloodOrbCost(4000)
    .Unlocks(CustomSkillId.WallGlastaT)
    .HasUnlockDependencies(4, 5, 6);
skillAugmentation.AddNode(9)
    .Location(6, 3)
    .HighOrbCost(200)
    .HasUnlockDependencies(6)
    .HasSpecialConditionDependencies(1)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
// Row 4
skillAugmentation.AddNode(10)
    .Location(1, 4)
    .BloodOrbCost(3500)
    .HasUnlockDependencies(7)
    .Unlocks(OrbGainParamType.JobHpMax, 35);
skillAugmentation.AddNode(11)
    .Location(3, 4)
    .HighOrbCost(400)
    .HasUnlockDependencies(8)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
skillAugmentation.AddNode(12)
    .Location(5, 4)
    .HighOrbCost(400)
    .HasUnlockDependencies(8)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
skillAugmentation.AddNode(13)
    .Location(7, 4)
    .BloodOrbCost(3500)
    .HasUnlockDependencies(9)
    .Unlocks(OrbGainParamType.JobHpMax, 35);
// Row 5
skillAugmentation.AddNode(14)
    .Location(1, 5)
    .BloodOrbCost(4000)
    .HasUnlockDependencies(10)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
skillAugmentation.AddNode(15)
    .Location(3, 5)
    .HighOrbCost(400)
    .HasUnlockDependencies(11)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
skillAugmentation.AddNode(16)
    .Location(5, 5)
    .HighOrbCost(400)
    .HasUnlockDependencies(12)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
skillAugmentation.AddNode(17)
    .Location(7, 5)
    .BloodOrbCost(4000)
    .HasUnlockDependencies(13)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
// Row 6
skillAugmentation.AddNode(18)
    .Location(1, 6)
    .BloodOrbCost(5000)
    .HasUnlockDependencies(14)
    .Unlocks(OrbGainParamType.JobHpMax, 40);
skillAugmentation.AddNode(19)
    .Location(4, 6)
    .HighOrbCost(400)
    .HasUnlockDependencies(15, 16)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
skillAugmentation.AddNode(20)
    .Location(7, 6)
    .BloodOrbCost(5000)
    .HasUnlockDependencies(17)
    .Unlocks(OrbGainParamType.JobHpMax, 40);
// Row 7
skillAugmentation.AddNode(21)
    .Location(1, 7)
    .BloodOrbCost(5500)
    .HasUnlockDependencies(18)
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15);
skillAugmentation.AddNode(22)
    .Location(4, 7)
    .HighOrbCost(200)
    .HasUnlockDependencies(19)
    .HasSpecialConditionDependencies(2)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
skillAugmentation.AddNode(23)
    .Location(7, 7)
    .BloodOrbCost(5500)
    .HasUnlockDependencies(20)
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 15);
// Row 8
skillAugmentation.AddNode(24)
    .Location(4, 8)
    .HighOrbCost(600)
    .HasUnlockDependencies(22)
    .Unlocks(OrbGainParamType.JobHpMax, 40);
// Row 9
skillAugmentation.AddNode(25)
    .Location(4, 9)
    .HighOrbCost(800)
    .HasUnlockDependencies(24)
    .HasSpecialConditionDependencies(3)
    .Unlocks(CustomSkillId.WallGlastaP);
#endregion

return skillAugmentation;

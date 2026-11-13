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

#region TIER2
// Row 11
skillAugmentation.AddNode(28)
    .Location(4, 11)
    .BloodOrbCost(5500)
    .HasUnlockDependencies(26)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasQuestDependency(QuestId.HerosRestFeryanaRegion);
// Row 12
skillAugmentation.AddNode(29)
    .Location(2, 12)
    .BloodOrbCost(5600)
    .HasUnlockDependencies(28)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
skillAugmentation.AddNode(30)
    .Location(6, 12)
    .BloodOrbCost(5600)
    .HasUnlockDependencies(28)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
// Row 13
skillAugmentation.AddNode(31)
    .Location(2, 13)
    .BloodOrbCost(5800)
    .HasUnlockDependencies(29)
    .Unlocks(OrbGainParamType.JobStaminaMax, 15);
skillAugmentation.AddNode(32)
    .Location(6, 13)
    .BloodOrbCost(5800)
    .HasUnlockDependencies(30)
    .Unlocks(OrbGainParamType.JobStaminaMax, 15);
// Row 14
skillAugmentation.AddNode(33)
    .Location(2, 14)
    .BloodOrbCost(6000)
    .HasUnlockDependencies(31)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
skillAugmentation.AddNode(34)
    .Location(3, 14)
    .HighOrbCost(500)
    .HasUnlockDependencies(31)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2);
skillAugmentation.AddNode(35)
    .Location(5, 14)
    .HighOrbCost(500)
    .HasUnlockDependencies(32)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2);
skillAugmentation.AddNode(36)
    .Location(6, 14)
    .BloodOrbCost(6000)
    .HasUnlockDependencies(32)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
// Row 15
skillAugmentation.AddNode(37)
    .Location(2, 15)
    .BloodOrbCost(6200)
    .HasUnlockDependencies(33)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
skillAugmentation.AddNode(38)
    .Location(6, 15)
    .BloodOrbCost(6200)
    .HasUnlockDependencies(36)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
// Row 16
skillAugmentation.AddNode(39)
    .Location(2, 16)
    .BloodOrbCost(6400)
    .HasUnlockDependencies(37)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 2);
skillAugmentation.AddNode(40)
    .Location(3, 16)
    .HighOrbCost(500)
    .HasUnlockDependencies(37)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2);
skillAugmentation.AddNode(41)
    .Location(5, 16)
    .HighOrbCost(500)
    .HasUnlockDependencies(38)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2);
skillAugmentation.AddNode(42)
    .Location(6, 16)
    .BloodOrbCost(6400)
    .HasUnlockDependencies(38)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 2);
// Row 17
skillAugmentation.AddNode(43)
    .Location(2, 17)
    .BloodOrbCost(6600)
    .HasUnlockDependencies(39)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 2);
skillAugmentation.AddNode(44)
    .Location(6, 17)
    .BloodOrbCost(6600)
    .HasUnlockDependencies(42)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 2);
// Row 18
skillAugmentation.AddNode(45)
    .Location(2, 18)
    .HighOrbCost(600)
    .HasUnlockDependencies(43)
    .HasSpecialConditionDependencies(4)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
skillAugmentation.AddNode(46)
    .Location(3, 18)
    .BloodOrbCost(6800)
    .HasUnlockDependencies(43)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2);
skillAugmentation.AddNode(47)
    .Location(5, 18)
    .BloodOrbCost(6800)
    .HasUnlockDependencies(44)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2);
skillAugmentation.AddNode(48)
    .Location(6, 18)
    .HighOrbCost(600)
    .HasUnlockDependencies(44)
    .HasSpecialConditionDependencies(5)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
// Row 19
skillAugmentation.AddNode(49)
    .Location(1, 19)
    .HighOrbCost(600)
    .HasUnlockDependencies(45)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 3);
skillAugmentation.AddNode(50)
    .Location(3, 19)
    .BloodOrbCost(7000)
    .HasUnlockDependencies(46)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
skillAugmentation.AddNode(51)
    .Location(5, 19)
    .BloodOrbCost(7000)
    .HasUnlockDependencies(47)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
skillAugmentation.AddNode(52)
    .Location(7, 19)
    .HighOrbCost(600)
    .HasUnlockDependencies(48)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 3);
// Row 20
skillAugmentation.AddNode(53)
    .Location(1, 20)
    .HighOrbCost(700)
    .HasUnlockDependencies(49)
    .Unlocks(AbilityId.SkySlashAbsorption);
skillAugmentation.AddNode(54)
    .Location(4, 20)
    .HighOrbCost(600)
    .HasUnlockDependencies(50, 51)
    .HasSpecialConditionDependencies(6)
    .Unlocks(OrbGainParamType.JobStaminaMax, 20);
skillAugmentation.AddNode(55)
    .Location(7, 20)
    .HighOrbCost(700)
    .HasUnlockDependencies(52)
    .Unlocks(AbilityId.FallingSlashAbsorption);
// Row 21
skillAugmentation.AddNode(56)
    .Location(3, 21)
    .HighOrbCost(700)
    .HasUnlockDependencies(54)
    .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2);
skillAugmentation.AddNode(57)
    .Location(5, 21)
    .HighOrbCost(700)
    .HasUnlockDependencies(54)
    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2);
#endregion

#region TIER4
// Row 22
skillAugmentation.AddNode(58)
    .Location(4, 22)
    .BloodOrbCost(5100)
    .HasUnlockDependencies(56, 57)
    .HasQuestDependency((QuestId)60300023)
    .Unlocks(OrbGainParamType.JobHpMax, 30);
// Row 23
skillAugmentation.AddNode(59)
    .Location(3, 23)
    .HighOrbCost(300)
    .HasUnlockDependencies(58)
	.HasSpecialConditionDependencies(12)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
skillAugmentation.AddNode(60)
    .Location(4, 23)
    .BloodOrbCost(5300)
    .HasUnlockDependencies(58)
    .Unlocks(OrbGainParamType.JobHpMax, 30);
skillAugmentation.AddNode(61)
    .Location(5, 23)
    .HighOrbCost(300)
    .HasUnlockDependencies(58)
	.HasSpecialConditionDependencies(13)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
// Row 24	
skillAugmentation.AddNode(62)
    .Location(2, 24)
    .HighOrbCost(300)
    .HasUnlockDependencies(59)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
skillAugmentation.AddNode(63)
    .Location(4, 24)
    .BloodOrbCost(5400)
    .HasUnlockDependencies(60)
    .Unlocks(OrbGainParamType.JobHpMax, 30);
skillAugmentation.AddNode(64)
    .Location(6, 24)
    .HighOrbCost(300)
    .HasUnlockDependencies(61)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
// Row 25	
skillAugmentation.AddNode(65)
    .Location(1, 25)
    .HighOrbCost(300)
    .HasUnlockDependencies(62)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
skillAugmentation.AddNode(66)
    .Location(3, 25)
    .BloodOrbCost(5600)
    .HasUnlockDependencies(63)
    .HasSpecialConditionDependencies(9)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
skillAugmentation.AddNode(67)
    .Location(4, 25)
    .BloodOrbCost(5600)
    .HasUnlockDependencies(63)
    .HasSpecialConditionDependencies(7)
    .Unlocks(OrbGainParamType.JobHpMax, 30);
skillAugmentation.AddNode(68)
    .Location(5, 25)
    .BloodOrbCost(5600)
    .HasUnlockDependencies(63)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
skillAugmentation.AddNode(69)
    .Location(7, 25)
    .HighOrbCost(300)
    .HasUnlockDependencies(64)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
// Row 26	
skillAugmentation.AddNode(70)
    .Location(2, 26)
    .BloodOrbCost(5800)
    .HasUnlockDependencies(66)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
skillAugmentation.AddNode(71)
    .Location(4, 26)
    .BloodOrbCost(5800)
    .HasUnlockDependencies(63)
    .Unlocks(OrbGainParamType.JobHpMax, 30);
skillAugmentation.AddNode(72)
    .Location(6, 26)
    .BloodOrbCost(5800)
    .HasUnlockDependencies(68)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
// Row 27
skillAugmentation.AddNode(73)
    .Location(3, 27)
    .BloodOrbCost(6000)
    .HasUnlockDependencies(71)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
skillAugmentation.AddNode(74)
    .Location(4, 27)
    .HighOrbCost(500)
    .HasUnlockDependencies(71)
	.HasSpecialConditionDependencies(11)
    .Unlocks(CustomSkillId.PhantomEdge);
skillAugmentation.AddNode(75)
    .Location(5, 27)
    .BloodOrbCost(6000)
    .HasUnlockDependencies(71)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
// Row 28
skillAugmentation.AddNode(76)
    .Location(4, 28)
    .HighOrbCost(500)
    .HasUnlockDependencies(74)
	.HasSpecialConditionDependencies(14)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
// Row 29
skillAugmentation.AddNode(77)
    .Location(3, 29)
    .BloodOrbCost(6500)
    .HasUnlockDependencies(76)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
skillAugmentation.AddNode(78)
    .Location(4, 29)
    .HighOrbCost(500)
    .HasUnlockDependencies(76)
    .Unlocks(OrbGainParamType.JobHpMax, 50);
skillAugmentation.AddNode(79)
    .Location(5, 29)
    .BloodOrbCost(6500)
    .HasUnlockDependencies(76)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
// Row 30
skillAugmentation.AddNode(80)
    .Location(2, 30)
    .HighOrbCost(600)
    .HasUnlockDependencies(77)
    .Unlocks(AbilityId.RushAttack);
skillAugmentation.AddNode(81)
    .Location(6, 30)
    .HighOrbCost(600)
    .HasUnlockDependencies(79)
    .Unlocks(AbilityId.FlowAttack);
#endregion

return skillAugmentation;

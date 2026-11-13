#load "libs.csx"

public class SkillAugmentation : ISkillAugmentation
{
    public override JobId JobId => JobId.Hunter;
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
    .Unlocks(CustomSkillId.ThreefoldArrowT)
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
    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1);
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
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 10);
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
    .Unlocks(CustomSkillId.ThreefoldArrowP);
#endregion

#region TIER2
// Row 10
skillAugmentation.AddNode(26)
    .Location(4, 10)
    .BloodOrbCost(3500)
    .HasUnlockDependencies(25)
    .HasQuestDependency(QuestId.HerosRestFeryanaRegion)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
// Row 11
skillAugmentation.AddNode(27)
    .Location(3, 11)
    .BloodOrbCost(3600)
    .HasUnlockDependencies(26)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
skillAugmentation.AddNode(28)
    .Location(4, 11)
    .BloodOrbCost(3600)
    .HasUnlockDependencies(26)
    .Unlocks(OrbGainParamType.JobHpMax, 30);
skillAugmentation.AddNode(29)
    .Location(5, 11)
    .BloodOrbCost(3600)
    .HasUnlockDependencies(26)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
// Row 12
skillAugmentation.AddNode(30)
    .Location(2, 12)
    .HighOrbCost(250)
    .HasUnlockDependencies(27)
    .HasSpecialConditionDependencies(4)
    .Unlocks(OrbGainParamType.JobHpMax, 35);
skillAugmentation.AddNode(31)
    .Location(4, 12)
    .BloodOrbCost(4000)
    .HasUnlockDependencies(27, 28, 29)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
skillAugmentation.AddNode(32)
    .Location(6, 12)
    .HighOrbCost(250)
    .HasUnlockDependencies(29)
    .HasSpecialConditionDependencies(5)
    .Unlocks(OrbGainParamType.JobHpMax, 35);
// Row 13
skillAugmentation.AddNode(33)
    .Location(1, 13)
    .BloodOrbCost(3600)
    .HasUnlockDependencies(30)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
skillAugmentation.AddNode(34)
    .Location(4, 13)
    .BloodOrbCost(5500)
    .HasUnlockDependencies(31)
    .Unlocks(CustomSkillId.WhirlingArrowT);
skillAugmentation.AddNode(35)
    .Location(7, 13)
    .BloodOrbCost(3600)
    .HasUnlockDependencies(32)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
// Row 14
skillAugmentation.AddNode(36)
    .Location(1, 14)
    .BloodOrbCost(3700)
    .HasUnlockDependencies(33)
    .Unlocks(OrbGainParamType.JobHpMax, 35);
skillAugmentation.AddNode(37)
    .Location(3, 14)
    .HighOrbCost(400)
    .HasUnlockDependencies(30, 34)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
skillAugmentation.AddNode(38)
    .Location(5, 14)
    .HighOrbCost(400)
    .HasUnlockDependencies(32, 34)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
skillAugmentation.AddNode(39)
    .Location(7, 14)
    .BloodOrbCost(3700)
    .HasUnlockDependencies(35)
    .Unlocks(OrbGainParamType.JobHpMax, 35);
// Row 15
skillAugmentation.AddNode(40)
    .Location(4, 15)
    .HighOrbCost(650)
    .HasUnlockDependencies(37, 38)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
// Row 16
skillAugmentation.AddNode(41)
    .Location(1, 16)
    .BloodOrbCost(3800)
    .HasUnlockDependencies(36)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
skillAugmentation.AddNode(42)
    .Location(4, 16)
    .HighOrbCost(400)
    .HasUnlockDependencies(40)
    .HasSpecialConditionDependencies(6)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
skillAugmentation.AddNode(43)
    .Location(7, 16)
    .BloodOrbCost(3800)
    .HasUnlockDependencies(39)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
// Row 17
skillAugmentation.AddNode(44)
    .Location(1, 17)
    .BloodOrbCost(4000)
    .HasUnlockDependencies(41)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
skillAugmentation.AddNode(45)
    .Location(2, 17)
    .BloodOrbCost(5500)
    .HasUnlockDependencies(41)
    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1);
skillAugmentation.AddNode(46)
    .Location(3, 17)
    .HighOrbCost(650)
    .HasUnlockDependencies(42)
    .Unlocks(OrbGainParamType.JobHpMax, 40);
skillAugmentation.AddNode(47)
    .Location(5, 17)
    .HighOrbCost(650)
    .HasUnlockDependencies(42)
    .Unlocks(OrbGainParamType.JobHpMax, 40);
skillAugmentation.AddNode(48)
    .Location(6, 17)
    .BloodOrbCost(5500)
    .HasUnlockDependencies(43)
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 10);
skillAugmentation.AddNode(49)
    .Location(7, 17)
    .BloodOrbCost(4000)
    .HasUnlockDependencies(43)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
// Row 18
skillAugmentation.AddNode(50)
    .Location(4, 18)
    .HighOrbCost(850)
    .HasUnlockDependencies(46, 47)
    .Unlocks(CustomSkillId.WhirlingArrowP);
#endregion

#region TIER3
// Row 19
skillAugmentation.AddNode(51)
    .Location(4, 19)
    .BloodOrbCost(4000)
    .HasUnlockDependencies(50)
    .HasQuestDependency((QuestId)60300022)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
// Row 20
skillAugmentation.AddNode(52)
    .Location(2, 20)
    .BloodOrbCost(4200)
    .HasUnlockDependencies(51)
    .Unlocks(OrbGainParamType.JobHpMax, 30);
skillAugmentation.AddNode(53)
    .Location(6, 20)
    .BloodOrbCost(3600)
    .HasUnlockDependencies(51)
    .Unlocks(OrbGainParamType.JobHpMax, 30);
// Row 21
skillAugmentation.AddNode(54)
    .Location(1, 21)
    .BloodOrbCost(4200)
    .HasUnlockDependencies(52)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
skillAugmentation.AddNode(55)
    .Location(7, 21)
    .BloodOrbCost(4200)
    .HasUnlockDependencies(53)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
// Row 22
skillAugmentation.AddNode(56)
    .Location(1, 22)
    .HighOrbCost(250)
    .HasUnlockDependencies(54)
    .HasSpecialConditionDependencies(8)
    .Unlocks(OrbGainParamType.JobHpMax, 35);
skillAugmentation.AddNode(57)
    .Location(2, 22)
    .BloodOrbCost(4500)
    .HasUnlockDependencies(54)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
skillAugmentation.AddNode(58)
    .Location(6, 22)
    .BloodOrbCost(4500)
    .HasUnlockDependencies(55)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
skillAugmentation.AddNode(59)
    .Location(7, 22)
    .HighOrbCost(250)
    .HasUnlockDependencies(55)
    .HasSpecialConditionDependencies(9)
    .Unlocks(OrbGainParamType.JobHpMax, 35);
// Row 23
skillAugmentation.AddNode(60)
    .Location(4, 23)
    .HighOrbCost(100)
    .HasUnlockDependencies(57, 58)
    .HasSpecialConditionDependencies(7)
    .Unlocks(CustomSkillId.FullBendT);
// Row 24
skillAugmentation.AddNode(61)
    .Location(2, 24)
    .HighOrbCost(400)
    .HasUnlockDependencies(56)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
skillAugmentation.AddNode(62)
    .Location(6, 24)
    .HighOrbCost(400)
    .HasUnlockDependencies(59)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
// Row 25
skillAugmentation.AddNode(63)
    .Location(1, 25)
    .BloodOrbCost(4800)
    .HasUnlockDependencies(56)
    .Unlocks(OrbGainParamType.JobHpMax, 40);
skillAugmentation.AddNode(64)
    .Location(7, 25)
    .BloodOrbCost(4800)
    .HasUnlockDependencies(59)
    .Unlocks(OrbGainParamType.JobHpMax, 40);
// Row 26
skillAugmentation.AddNode(65)
    .Location(1, 26)
    .BloodOrbCost(4800)
    .HasUnlockDependencies(63)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
skillAugmentation.AddNode(66)
    .Location(7, 26)
    .BloodOrbCost(4800)
    .HasUnlockDependencies(64)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
// Row 27
skillAugmentation.AddNode(67)
    .Location(1, 27)
    .BloodOrbCost(5000)
    .HasUnlockDependencies(61, 65)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
skillAugmentation.AddNode(68)
    .Location(2, 27)
    .HighOrbCost(500)
    .HasUnlockDependencies(61, 65)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
skillAugmentation.AddNode(69)
    .Location(6, 27)
    .HighOrbCost(500)
    .HasUnlockDependencies(62, 66)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
skillAugmentation.AddNode(70)
    .Location(7, 27)
    .BloodOrbCost(5000)
    .HasUnlockDependencies(62, 66)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
// Row 28
skillAugmentation.AddNode(71)
    .Location(1, 28)
    .BloodOrbCost(5500)
    .HasUnlockDependencies(67)
    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1);
skillAugmentation.AddNode(72)
    .Location(4, 28)
    .HighOrbCost(800)
    .HasUnlockDependencies(68, 69)
    .HasSpecialConditionDependencies(10)
    .Unlocks(OrbGainParamType.JobHpMax, 40);
skillAugmentation.AddNode(73)
    .Location(7, 28)
    .BloodOrbCost(5500)
    .HasUnlockDependencies(70)
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 10);
// Row 29
skillAugmentation.AddNode(74)
    .Location(4, 29)
    .HighOrbCost(800)
    .HasUnlockDependencies(72)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
// Row 30
skillAugmentation.AddNode(75)
    .Location(4, 30)
    .HighOrbCost(1000)
    .HasUnlockDependencies(74)
    .Unlocks(CustomSkillId.FullBendP);
#endregion

#region TIER4
// Row 31
skillAugmentation.AddNode(76)
    .Location(4, 31)
    .BloodOrbCost(5100)
    .HasUnlockDependencies(75)
    .HasQuestDependency((QuestId)60300023)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
// Row 32
skillAugmentation.AddNode(77)
    .Location(3, 32)
    .HighOrbCost(300)
    .HasUnlockDependencies(76)
	.HasSpecialConditionDependencies(12)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
skillAugmentation.AddNode(78)
    .Location(4, 32)
    .BloodOrbCost(5300)
    .HasUnlockDependencies(76)
    .Unlocks(OrbGainParamType.JobHpMax, 30);
skillAugmentation.AddNode(79)
    .Location(5, 32)
    .HighOrbCost(300)
    .HasUnlockDependencies(76)
	.HasSpecialConditionDependencies(13)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
// Row 33	
skillAugmentation.AddNode(80)
    .Location(2, 33)
    .HighOrbCost(300)
    .HasUnlockDependencies(77)
    .Unlocks(OrbGainParamType.JobHpMax, 40);
skillAugmentation.AddNode(81)
    .Location(4, 33)
    .BloodOrbCost(5400)
    .HasUnlockDependencies(78)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
skillAugmentation.AddNode(82)
    .Location(6, 33)
    .HighOrbCost(300)
    .HasUnlockDependencies(79)
    .Unlocks(OrbGainParamType.JobHpMax, 40);
// Row 34	
skillAugmentation.AddNode(83)
    .Location(1, 34)
    .HighOrbCost(300)
    .HasUnlockDependencies(80)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
skillAugmentation.AddNode(84)
    .Location(3, 34)
    .BloodOrbCost(5600)
    .HasUnlockDependencies(81)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
skillAugmentation.AddNode(85)
    .Location(4, 34)
    .BloodOrbCost(5600)
    .HasUnlockDependencies(81)
    .Unlocks(OrbGainParamType.JobHpMax, 30);
skillAugmentation.AddNode(86)
    .Location(5, 34)
    .BloodOrbCost(5600)
    .HasUnlockDependencies(81)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
skillAugmentation.AddNode(87)
    .Location(7, 34)
    .HighOrbCost(300)
    .HasUnlockDependencies(82)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
// Row 35	
skillAugmentation.AddNode(88)
    .Location(2, 35)
    .BloodOrbCost(5800)
    .HasUnlockDependencies(84)
    .Unlocks(OrbGainParamType.JobHpMax, 35);
skillAugmentation.AddNode(89)
    .Location(4, 35)
    .BloodOrbCost(5800)
    .HasUnlockDependencies(85)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
skillAugmentation.AddNode(90)
    .Location(6, 35)
    .BloodOrbCost(5800)
    .HasUnlockDependencies(86)
    .Unlocks(OrbGainParamType.JobHpMax, 35);
// Row 36
skillAugmentation.AddNode(91)
    .Location(3, 36)
    .BloodOrbCost(6000)
    .HasUnlockDependencies(89)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1);
skillAugmentation.AddNode(92)
    .Location(4, 36)
    .HighOrbCost(500)
    .HasUnlockDependencies(89)
	.HasSpecialConditionDependencies(11)
    .Unlocks(CustomSkillId.ExplosiveArrowVolleyT);
skillAugmentation.AddNode(93)
    .Location(5, 36)
    .BloodOrbCost(6000)
    .HasUnlockDependencies(89)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1);
// Row 37
skillAugmentation.AddNode(94)
    .Location(4, 37)
    .HighOrbCost(500)
    .HasUnlockDependencies(92)
	.HasSpecialConditionDependencies(14)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
// Row 38
skillAugmentation.AddNode(95)
    .Location(3, 38)
    .BloodOrbCost(6500)
    .HasUnlockDependencies(94)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1);
skillAugmentation.AddNode(96)
    .Location(4, 38)
    .HighOrbCost(500)
    .HasUnlockDependencies(94)
    .Unlocks(OrbGainParamType.JobHpMax, 40);
skillAugmentation.AddNode(97)
    .Location(5, 38)
    .BloodOrbCost(6500)
    .HasUnlockDependencies(94)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1);
// Row 39
skillAugmentation.AddNode(98)
    .Location(2, 39)
    .HighOrbCost(600)
    .HasUnlockDependencies(95)
    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1);
skillAugmentation.AddNode(99)
    .Location(6, 39)
    .HighOrbCost(600)
    .HasUnlockDependencies(97)
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 10);
// Row 40
skillAugmentation.AddNode(100)
    .Location(4, 40)
    .HighOrbCost(1000)
    .HasUnlockDependencies(96)
    .Unlocks(CustomSkillId.ExplosiveArrowVolleyP);
#endregion

return skillAugmentation;

#load "libs.csx"

public class SkillAugmentation : ISkillAugmentation
{
    public override JobId JobId => JobId.Hunter;
}

var skillAugmentation = new SkillAugmentation();

#region PAGE1
// 1st tier
skillAugmentation.AddNode(1)
    .Location(4, 1)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasQuestDependency(QuestId.SkillAugmentationHunterTrialI);

// 2nd Tier
skillAugmentation.AddNode(2)
    .Location(3, 2)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependency(1);

skillAugmentation.AddNode(3)
    .Location(5, 2)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependency(1);

// 3rd Tier
skillAugmentation.AddNode(4)
    .Location(2, 3)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(2, 3);

skillAugmentation.AddNode(5)
    .Location(4, 3)
    .BloodOrbCost(1200)
    .Unlocks(CustomSkillId.SkyBurstShot)
    .HasUnlockDependencies(2, 3);

skillAugmentation.AddNode(6)
    .Location(6, 3)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(2, 3);

// 4th Tier
skillAugmentation.AddNode(7)
    .Location(2, 4)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 30)
    .HasUnlockDependencies(4);

skillAugmentation.AddNode(8)
    .Location(4, 4)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(5);

skillAugmentation.AddNode(9)
    .Location(6, 4)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(6);

// 5th Tier
skillAugmentation.AddNode(10)
    .Location(1, 5)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(7);

skillAugmentation.AddNode(11)
    .Location(2, 5)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
    .HasUnlockDependencies(7);

skillAugmentation.AddNode(12)
    .Location(4, 5)
    .BloodOrbCost(1200)
    .Unlocks(AbilityId.ArcherySlayer)
    .HasUnlockDependencies(8);

skillAugmentation.AddNode(13)
    .Location(6, 5)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(9);

skillAugmentation.AddNode(14)
    .Location(7, 5)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(9);

// 6th Tier
skillAugmentation.AddNode(15)
    .Location(1, 6)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobStaminaMax, 15)
    .HasUnlockDependencies(10);

skillAugmentation.AddNode(16)
    .Location(2, 6)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1)
    .HasUnlockDependencies(11);

skillAugmentation.AddNode(17)
    .Location(4, 6)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(12);

skillAugmentation.AddNode(18)
    .Location(6, 6)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 10)
    .HasUnlockDependencies(13);

skillAugmentation.AddNode(19)
    .Location(7, 6)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(14);

// 7th tier
skillAugmentation.AddNode(20)
    .Location(1, 7)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 15)
    .HasUnlockDependencies(15);

skillAugmentation.AddNode(21)
    .Location(4, 7)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(16, 17, 18);

skillAugmentation.AddNode(22)
    .Location(7, 7)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(19);

// 8th tier
skillAugmentation.AddNode(23)
    .Location(1, 8)
    .BloodOrbCost(1700)
    .Unlocks(AbilityId.ExplodingArrowFury)
    .HasUnlockDependencies(20);

skillAugmentation.AddNode(24)
    .Location(3, 8)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
    .HasUnlockDependencies(21);

skillAugmentation.AddNode(25)
    .Location(5, 8)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependencies(21);

skillAugmentation.AddNode(26)
    .Location(7, 8)
    .BloodOrbCost(2000)
    .Unlocks(AbilityId.ArrowheadStrikeFury)
    .HasUnlockDependencies(22);

// 9th Tier
skillAugmentation.AddNode(27)
    .Location(4, 9)
    .BloodOrbCost(1800)
    .Unlocks(AbilityId.RigidStance)
    .HasUnlockDependencies(24, 25);
#endregion

#region PAGE2
// 10th Tier
skillAugmentation.AddNode(28)
    .Location(4, 10)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 15)
    .HasUnlockDependencies(27)
    .HasQuestDependency(QuestId.SkillAugmentationHunterTrialII);

// 11th Tier
skillAugmentation.AddNode(29)
    .Location(2, 11)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(28);

skillAugmentation.AddNode(30)
    .Location(6, 11)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(28);

// 12th Tier
skillAugmentation.AddNode(31)
    .Location(1, 12)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobHpMax, 20)
    .HasUnlockDependencies(29);

skillAugmentation.AddNode(32)
    .Location(3, 12)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(29);

skillAugmentation.AddNode(33)
    .Location(5, 12)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(30);

skillAugmentation.AddNode(34)
    .Location(7, 12)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobHpMax, 15)
    .HasUnlockDependencies(30);

// 13th Tier
skillAugmentation.AddNode(35)
    .Location(1, 13)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(31);

skillAugmentation.AddNode(36)
    .Location(3, 13)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(32);

skillAugmentation.AddNode(37)
    .Location(5, 13)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(33);

skillAugmentation.AddNode(38)
    .Location(7, 13)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(34);

// 14th Tier
skillAugmentation.AddNode(39)
    .Location(1, 14)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(35);

skillAugmentation.AddNode(40)
    .Location(3, 14)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(36);

skillAugmentation.AddNode(41)
    .Location(5, 14)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(37);

skillAugmentation.AddNode(42)
    .Location(7, 14)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(38);

// 15th Tier
skillAugmentation.AddNode(43)
    .Location(2, 15)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobHpMax, 15)
    .HasUnlockDependencies(39, 40);

skillAugmentation.AddNode(44)
    .Location(6, 15)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(41, 42);

// 16th Tier
skillAugmentation.AddNode(45)
    .Location(2, 16)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 10)
    .HasUnlockDependencies(43);

skillAugmentation.AddNode(46)
    .Location(4, 16)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(43, 44);

skillAugmentation.AddNode(47)
    .Location(6, 16)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1)
    .HasUnlockDependencies(44);

// 17th Tier
skillAugmentation.AddNode(48)
    .Location(1, 17)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(45);

skillAugmentation.AddNode(49)
    .Location(2, 17)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(45);

skillAugmentation.AddNode(50)
    .Location(3, 17)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(45);

skillAugmentation.AddNode(51)
    .Location(4, 17)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependencies(46);

skillAugmentation.AddNode(52)
    .Location(5, 17)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobHpMax, 20)
    .HasUnlockDependencies(47);

skillAugmentation.AddNode(53)
    .Location(6, 17)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(47);

skillAugmentation.AddNode(54)
    .Location(7, 17)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(47);

// 18th Tier
skillAugmentation.AddNode(55)
    .Location(2, 18)
    .BloodOrbCost(1700)
    .Unlocks(AbilityId.KeensightShotSlayer)
    .HasUnlockDependencies(49);

skillAugmentation.AddNode(56)
    .Location(4, 18)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobStaminaMax, 15)
    .HasUnlockDependencies(51);

skillAugmentation.AddNode(57)
    .Location(6, 18)
    .BloodOrbCost(1700)
    .Unlocks(AbilityId.RescueAssistance)
    .HasUnlockDependencies(53);
#endregion

#region PAGE3
// 19th Tier
skillAugmentation.AddNode(58)
    .Location(4, 19)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(56)
    .HasQuestDependency(QuestId.SkillAugmentationHunterTrialIII);

// 20th Tier
skillAugmentation.AddNode(59)
    .Location(3, 20)
    .BloodOrbCost(1500)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(58);

skillAugmentation.AddNode(60)
    .Location(5, 20)
    .BloodOrbCost(1500)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(58);

// 21st Tier
skillAugmentation.AddNode(61)
    .Location(2, 21)
    .BloodOrbCost(1750)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(59);

skillAugmentation.AddNode(62)
    .Location(3, 21)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(59);

skillAugmentation.AddNode(63)
    .Location(5, 21)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(60);

skillAugmentation.AddNode(64)
    .Location(6, 21)
    .BloodOrbCost(1750)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(60);

// 22nd Tier
skillAugmentation.AddNode(65)
    .Location(1, 22)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(61);

skillAugmentation.AddNode(66)
    .Location(2, 22)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(61);

skillAugmentation.AddNode(67)
    .Location(4, 22)
    .BloodOrbCost(2300)
    .Unlocks(CustomSkillId.CombinedPierceShot)
    .HasUnlockDependencies(62, 63);

skillAugmentation.AddNode(68)
    .Location(6, 22)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(64);

skillAugmentation.AddNode(69)
    .Location(7, 22)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(64);

// 23rd tier
skillAugmentation.AddNode(70)
    .Location(2, 23)
    .BloodOrbCost(2050)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(65, 66);

skillAugmentation.AddNode(71)
    .Location(3, 23)
    .BloodOrbCost(2050)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(67);

skillAugmentation.AddNode(72)
    .Location(5, 23)
    .BloodOrbCost(2050)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependencies(67);

skillAugmentation.AddNode(73)
    .Location(6, 23)
    .BloodOrbCost(2050)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(68, 69);

// 24th tier
skillAugmentation.AddNode(74)
    .Location(2, 24)
    .BloodOrbCost(2600)
    .Unlocks(AbilityId.ArcheryCrusher)
    .HasUnlockDependencies(70, 71);

skillAugmentation.AddNode(75)
    .Location(6, 24)
    .BloodOrbCost(2600)
    .Unlocks(AbilityId.ExplodingArrowCrusher)
    .HasUnlockDependencies(72, 73);

// 25th tier
skillAugmentation.AddNode(76)
    .Location(2, 25)
    .BloodOrbCost(2700)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
    .HasUnlockDependencies(74);

skillAugmentation.AddNode(77)
    .Location(3, 25)
    .BloodOrbCost(2200)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(74);

skillAugmentation.AddNode(78)
    .Location(5, 25)
    .BloodOrbCost(2200)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(75);

skillAugmentation.AddNode(79)
    .Location(6, 25)
    .BloodOrbCost(2700)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
    .HasUnlockDependencies(75);

// 26th tier
skillAugmentation.AddNode(80)
    .Location(2, 26)
    .BloodOrbCost(2300)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(76);

skillAugmentation.AddNode(81)
    .Location(4, 26)
    .BloodOrbCost(2800)
    .Unlocks(AbilityId.ArrowheadStrikeCrusher)
    .HasUnlockDependencies(77, 78);

skillAugmentation.AddNode(82)
    .Location(6, 26)
    .BloodOrbCost(2300)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(79);

// 27th tier
skillAugmentation.AddNode(83)
    .Location(1, 27)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 10)
    .HasUnlockDependencies(80);

skillAugmentation.AddNode(84)
    .Location(7, 27)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 10)
    .HasUnlockDependencies(82);

// 28th tier
skillAugmentation.AddNode(85)
    .Location(4, 28)
    .BloodOrbCost(3200)
    .Unlocks(AbilityId.AugmentedSpirit)
    .HasUnlockDependencies(83, 84);
#endregion

#region PAGE4
// 29th Tier
skillAugmentation.AddNode(86)
    .Location(2, 29)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(85)
    .HasQuestDependency(QuestId.SkillAugmentationHunterTrialIV);

skillAugmentation.AddNode(87)
    .Location(6, 29)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(85)
    .HasQuestDependency(QuestId.SkillAugmentationHunterTrialIV);

// 30th Tier
skillAugmentation.AddNode(88)
    .Location(1, 30)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(86);

skillAugmentation.AddNode(89)
    .Location(3, 30)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(86);

skillAugmentation.AddNode(90)
    .Location(5, 30)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(87);

skillAugmentation.AddNode(91)
    .Location(7, 30)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(87);

// 31st Tier
skillAugmentation.AddNode(92)
    .Location(1, 31)
    .BloodOrbCost(1950)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(88);

skillAugmentation.AddNode(93)
    .Location(2, 31)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(88);

skillAugmentation.AddNode(94)
    .Location(6, 31)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(91);

skillAugmentation.AddNode(95)
    .Location(7, 31)
    .BloodOrbCost(1950)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(91);

// 32st Tier
skillAugmentation.AddNode(96)
    .Location(3, 32)
    .BloodOrbCost(1950)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(89);

skillAugmentation.AddNode(97)
    .Location(4, 32)
    .BloodOrbCost(1950)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(89, 90);

skillAugmentation.AddNode(98)
    .Location(5, 32)
    .BloodOrbCost(2100)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(90);

// 33rd Tier
skillAugmentation.AddNode(99)
    .Location(2, 33)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(92, 93);

skillAugmentation.AddNode(100)
    .Location(3, 33)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(96);

skillAugmentation.AddNode(101)
    .Location(4, 33)
    .BloodOrbCost(2250)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(97);

skillAugmentation.AddNode(102)
    .Location(5, 33)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(98);

skillAugmentation.AddNode(103)
    .Location(6, 33)
    .BloodOrbCost(2550)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependencies(94, 95);

// 34th tier
skillAugmentation.AddNode(104)
    .Location(3, 34)
    .BloodOrbCost(2500)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(100);

skillAugmentation.AddNode(105)
    .Location(5, 34)
    .BloodOrbCost(2500)
    .Unlocks(OrbGainParamType.JobStaminaMax, 30)
    .HasUnlockDependencies(102);

// 35th tier
skillAugmentation.AddNode(106)
    .Location(2, 35)
    .BloodOrbCost(2700)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(99);

skillAugmentation.AddNode(107)
    .Location(3, 35)
    .BloodOrbCost(2800)
    .Unlocks(AbilityId.KeensightCrusher)
    .HasUnlockDependencies(104);

skillAugmentation.AddNode(108)
    .Location(5, 35)
    .BloodOrbCost(2800)
    .Unlocks(AbilityId.ClimaxBow)
    .HasUnlockDependencies(105);

skillAugmentation.AddNode(109)
    .Location(6, 35)
    .BloodOrbCost(2700)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(103);

// 36th tier
skillAugmentation.AddNode(110)
    .Location(4, 36)
    .BloodOrbCost(2900)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(101, 107, 108);

// 37th tier
skillAugmentation.AddNode(111)
    .Location(3, 37)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 10)
    .HasUnlockDependencies(106, 110);

skillAugmentation.AddNode(112)
    .Location(5, 37)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 10)
    .HasUnlockDependencies(109, 110);
#endregion

return skillAugmentation;

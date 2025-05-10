#load "libs.csx"

public class SkillAugmentation : ISkillAugmentation
{
    public override JobId JobId => JobId.ElementArcher;
}

var skillAugmentation = new SkillAugmentation();

#region PAGE1
skillAugmentation.AddNode(1)
    .Location(4, 1)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasQuestDependency(QuestId.SkillAugmentationElementArcherTrialI);

skillAugmentation.AddNode(2)
    .Location(3, 2)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(1);
skillAugmentation.AddNode(3)
    .Location(4, 2)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
    .HasUnlockDependencies(1);
skillAugmentation.AddNode(4)
    .Location(5, 2)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(1);

skillAugmentation.AddNode(5)
    .Location(2, 3)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(2);
skillAugmentation.AddNode(6)
    .Location(3, 3)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(2);
skillAugmentation.AddNode(7)
    .Location(4, 3)
    .BloodOrbCost(1200)
    .Unlocks(CustomSkillId.HealingFlash)
    .HasUnlockDependencies(3);
skillAugmentation.AddNode(8)
    .Location(5, 3)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(4);
skillAugmentation.AddNode(9)
    .Location(6, 3)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(4);

skillAugmentation.AddNode(10)
    .Location(1, 4)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(5);
skillAugmentation.AddNode(11)
    .Location(2, 4)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(5);
skillAugmentation.AddNode(12)
    .Location(3, 4)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(6);
skillAugmentation.AddNode(13)
    .Location(4, 4)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
    .HasUnlockDependencies(7);
skillAugmentation.AddNode(14)
    .Location(5, 4)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(8);
skillAugmentation.AddNode(15)
    .Location(6, 4)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(9);
skillAugmentation.AddNode(16)
    .Location(7, 4)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(9);

skillAugmentation.AddNode(17)
    .Location(2, 5)
    .BloodOrbCost(1200)
    .Unlocks(AbilityId.SeekerSlayer)
    .HasUnlockDependencies(10, 11);
skillAugmentation.AddNode(18)
    .Location(4, 5)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(12, 13, 14);
skillAugmentation.AddNode(19)
    .Location(6, 5)
    .BloodOrbCost(1200)
    .Unlocks(AbilityId.AidArrowChant)
    .HasUnlockDependencies(15, 16);

skillAugmentation.AddNode(20)
    .Location(2, 6)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(17);
skillAugmentation.AddNode(21)
    .Location(3, 6)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 10)
    .HasUnlockDependencies(17);
skillAugmentation.AddNode(22)
    .Location(4, 6)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(18);
skillAugmentation.AddNode(23)
    .Location(5, 6)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2)
    .HasUnlockDependencies(19);
skillAugmentation.AddNode(24)
    .Location(6, 6)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(19);

skillAugmentation.AddNode(25)
    .Location(2, 7)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependencies(20, 21);
skillAugmentation.AddNode(26)
    .Location(4, 7)
    .BloodOrbCost(1700)
    .Unlocks(AbilityId.ForwardKickSlayer)
    .HasUnlockDependencies(22);
skillAugmentation.AddNode(27)
    .Location(6, 7)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(23, 24);

skillAugmentation.AddNode(28)
    .Location(4, 8)
    .BloodOrbCost(1800)
    .Unlocks(AbilityId.SteadyAdvance)
    .HasUnlockDependencies(25, 27);
#endregion

#region PAGE2
skillAugmentation.AddNode(29)
    .Location(4, 9)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(28)
    .HasQuestDependency(QuestId.SkillAugmentationElementArcherTrialII);

skillAugmentation.AddNode(30)
    .Location(2, 10)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(29);
skillAugmentation.AddNode(31)
    .Location(6, 10)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(29);

skillAugmentation.AddNode(32)
    .Location(2, 11)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(30);
skillAugmentation.AddNode(33)
    .Location(3, 11)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(30);
skillAugmentation.AddNode(34)
    .Location(5, 11)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(31);
skillAugmentation.AddNode(35)
    .Location(6, 11)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(31);

skillAugmentation.AddNode(36)
    .Location(1, 12)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(32);
skillAugmentation.AddNode(37)
    .Location(2, 12)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(32);
skillAugmentation.AddNode(38)
    .Location(6, 12)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(35);
skillAugmentation.AddNode(39)
    .Location(7, 12)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(35);

skillAugmentation.AddNode(40)
    .Location(1, 13)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(36);
skillAugmentation.AddNode(41)
    .Location(2, 13)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(37);
skillAugmentation.AddNode(42)
    .Location(3, 13)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(37);
skillAugmentation.AddNode(43)
    .Location(5, 13)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(38);
skillAugmentation.AddNode(44)
    .Location(6, 13)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(38);
skillAugmentation.AddNode(45)
    .Location(7, 13)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(39);

skillAugmentation.AddNode(46)
    .Location(2, 14)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 10)
    .HasUnlockDependencies(40, 41);
skillAugmentation.AddNode(47)
    .Location(3, 14)
    .BloodOrbCost(2000)
    .Unlocks(AbilityId.DemonShield)
    .HasUnlockDependencies(42);
skillAugmentation.AddNode(48)
    .Location(5, 14)
    .BloodOrbCost(2000)
    .Unlocks(AbilityId.InvigoratingArrowsDuration)
    .HasUnlockDependencies(43);
skillAugmentation.AddNode(49)
    .Location(6, 14)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 1)
    .HasUnlockDependencies(44, 45);

skillAugmentation.AddNode(50)
    .Location(2, 15)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(46);
skillAugmentation.AddNode(51)
    .Location(4, 15)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
    .HasUnlockDependencies(47, 48);
skillAugmentation.AddNode(52)
    .Location(6, 15)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(49);

skillAugmentation.AddNode(53)
    .Location(2, 16)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(50);
skillAugmentation.AddNode(54)
    .Location(6, 16)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependencies(52);
#endregion

#region PAGE3
skillAugmentation.AddNode(55)
    .Location(4, 17)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(53, 54)
    .HasQuestDependency(QuestId.SkillAugmentationElementArcherTrialIII);

skillAugmentation.AddNode(56)
    .Location(2, 18)
    .BloodOrbCost(1450)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(55);
skillAugmentation.AddNode(57)
    .Location(6, 18)
    .BloodOrbCost(1450)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(55);

skillAugmentation.AddNode(58)
    .Location(1, 19)
    .BloodOrbCost(1550)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(56);
skillAugmentation.AddNode(59)
    .Location(3, 19)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(56);
skillAugmentation.AddNode(60)
    .Location(4, 19)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(55);
skillAugmentation.AddNode(61)
    .Location(5, 19)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(57);
skillAugmentation.AddNode(62)
    .Location(7, 19)
    .BloodOrbCost(1550)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(57);

skillAugmentation.AddNode(63)
    .Location(2, 20)
    .BloodOrbCost(1650)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(58, 59);
skillAugmentation.AddNode(64)
    .Location(4, 20)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependencies(60);
skillAugmentation.AddNode(65)
    .Location(6, 20)
    .BloodOrbCost(1650)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(61, 62);

skillAugmentation.AddNode(66)
    .Location(2, 21)
    .BloodOrbCost(1850)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(63);
skillAugmentation.AddNode(67)
    .Location(4, 21)
    .BloodOrbCost(2300)
    .Unlocks(CustomSkillId.TearingTentacleArrow)
    .HasUnlockDependencies(64);
skillAugmentation.AddNode(68)
    .Location(6, 21)
    .BloodOrbCost(1850)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(65);

skillAugmentation.AddNode(69)
    .Location(4, 22)
    .BloodOrbCost(2500)
    .Unlocks(AbilityId.SeekerArrowsBlink)
    .HasUnlockDependencies(66, 68);

skillAugmentation.AddNode(70)
    .Location(3, 23)
    .BloodOrbCost(1900)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(69);
skillAugmentation.AddNode(71)
    .Location(4, 23)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(69);
skillAugmentation.AddNode(72)
    .Location(5, 23)
    .BloodOrbCost(1900)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(69);

skillAugmentation.AddNode(73)
    .Location(2, 24)
    .BloodOrbCost(2200)
    .Unlocks(OrbGainParamType.JobHpMax, 40)
    .HasUnlockDependencies(70);
skillAugmentation.AddNode(74)
    .Location(3, 24)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(70);
skillAugmentation.AddNode(75)
    .Location(4, 24)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(71);
skillAugmentation.AddNode(76)
    .Location(5, 24)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(72);
skillAugmentation.AddNode(77)
    .Location(6, 24)
    .BloodOrbCost(2100)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(72);

skillAugmentation.AddNode(78)
    .Location(2, 25)
    .BloodOrbCost(2100)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(73);
skillAugmentation.AddNode(79)
    .Location(3, 25)
    .BloodOrbCost(2600)
    .Unlocks(AbilityId.AidArrowBlink)
    .HasUnlockDependencies(74);
skillAugmentation.AddNode(80)
    .Location(5, 25)
    .BloodOrbCost(2600)
    .Unlocks(AbilityId.FrontKickCrusher)
    .HasUnlockDependencies(76);
skillAugmentation.AddNode(81)
    .Location(6, 25)
    .BloodOrbCost(2100)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(77);

skillAugmentation.AddNode(82)
    .Location(2, 26)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 1)
    .HasUnlockDependencies(78, 79);
skillAugmentation.AddNode(83)
    .Location(6, 26)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 1)
    .HasUnlockDependencies(80, 81);

skillAugmentation.AddNode(84)
    .Location(4, 27)
    .BloodOrbCost(3000)
    .Unlocks(AbilityId.SalvationalMagick)
    .HasUnlockDependencies(82, 83);
#endregion

#region PAGE4
skillAugmentation.AddNode(85)
    .Location(3, 28)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(84)
    .HasQuestDependency(QuestId.SkillAugmentationElementArcherTrialIV);
skillAugmentation.AddNode(86)
    .Location(5, 28)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(84)
    .HasQuestDependency(QuestId.SkillAugmentationElementArcherTrialIV);

skillAugmentation.AddNode(87)
    .Location(2, 29)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(85);
skillAugmentation.AddNode(88)
    .Location(3, 29)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(85);
skillAugmentation.AddNode(89)
    .Location(5, 29)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(86);
skillAugmentation.AddNode(90)
    .Location(6, 29)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(86);

skillAugmentation.AddNode(91)
    .Location(1, 30)
    .BloodOrbCost(1900)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(87);
skillAugmentation.AddNode(92)
    .Location(7, 30)
    .BloodOrbCost(1900)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(90);

skillAugmentation.AddNode(93)
    .Location(2, 31)
    .BloodOrbCost(2100)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(91);
skillAugmentation.AddNode(94)
    .Location(3, 31)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(88);
skillAugmentation.AddNode(95)
    .Location(5, 31)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(89);
skillAugmentation.AddNode(96)
    .Location(6, 31)
    .BloodOrbCost(2100)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(92);

skillAugmentation.AddNode(97)
    .Location(1, 32)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependencies(93);
skillAugmentation.AddNode(98)
    .Location(2, 32)
    .BloodOrbCost(2300)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(93);
skillAugmentation.AddNode(99)
    .Location(4, 32)
    .BloodOrbCost(2350)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(94, 95);
skillAugmentation.AddNode(100)
    .Location(6, 32)
    .BloodOrbCost(2300)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(96);
skillAugmentation.AddNode(101)
    .Location(7, 32)
    .BloodOrbCost(2350)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(96);

skillAugmentation.AddNode(102)
    .Location(1, 33)
    .BloodOrbCost(2500)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(97, 98);
skillAugmentation.AddNode(103)
    .Location(3, 33)
    .BloodOrbCost(2500)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(99);
skillAugmentation.AddNode(104)
    .Location(5, 33)
    .BloodOrbCost(2500)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(99);
skillAugmentation.AddNode(105)
    .Location(7, 33)
    .BloodOrbCost(2500)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(100, 101);

skillAugmentation.AddNode(106)
    .Location(2, 34)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 1)
    .HasUnlockDependencies(102);
skillAugmentation.AddNode(107)
    .Location(4, 34)
    .BloodOrbCost(2800)
    .Unlocks(AbilityId.InvigorationArrowExpand)
    .HasUnlockDependencies(103, 104);
skillAugmentation.AddNode(108)
    .Location(6, 34)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 1)
    .HasUnlockDependencies(105);

skillAugmentation.AddNode(109)
    .Location(3, 35)
    .BloodOrbCost(2650)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(106);
skillAugmentation.AddNode(110)
    .Location(5, 35)
    .BloodOrbCost(2650)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(108);

skillAugmentation.AddNode(111)
    .Location(4, 36)
    .BloodOrbCost(3000)
    .Unlocks(AbilityId.CounterEye)
    .HasUnlockDependencies(109, 110);
#endregion

return skillAugmentation;

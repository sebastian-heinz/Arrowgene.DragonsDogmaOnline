#load "libs.csx"

public class SkillAugmentation : ISkillAugmentation
{
    public override JobId JobId => JobId.Alchemist;
}

var skillAugmentation = new SkillAugmentation();

#region PAGE1
skillAugmentation.AddNode(1)
    .Location(2, 1)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasQuestDependency(QuestId.SkillAugmentationAlchemistTrialI);
skillAugmentation.AddNode(2)
    .Location(4, 1)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasQuestDependency(QuestId.SkillAugmentationAlchemistTrialI);
skillAugmentation.AddNode(3)
    .Location(6, 1)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasQuestDependency(QuestId.SkillAugmentationAlchemistTrialI);

skillAugmentation.AddNode(4)
    .Location(1, 2)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(1);
skillAugmentation.AddNode(5)
    .Location(2, 2)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
    .HasUnlockDependencies(1);
skillAugmentation.AddNode(6)
    .Location(4, 2)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(2);
skillAugmentation.AddNode(7)
    .Location(6, 2)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(3);

skillAugmentation.AddNode(8)
    .Location(1, 3)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(4);
skillAugmentation.AddNode(9)
    .Location(2, 3)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(5);
skillAugmentation.AddNode(10)
    .Location(3, 3)
    .BloodOrbCost(1800)
    .Unlocks(CustomSkillId.AlchemicalBurst)
    .HasUnlockDependencies(5, 6);
skillAugmentation.AddNode(11)
    .Location(4, 3)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(6);
skillAugmentation.AddNode(12)
    .Location(5, 3)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(7);
skillAugmentation.AddNode(13)
    .Location(6, 3)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(7);
skillAugmentation.AddNode(14)
    .Location(7, 3)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(7);

skillAugmentation.AddNode(15)
    .Location(3, 4)
    .BloodOrbCost(1200)
    .Unlocks(AbilityId.AlchemicStrikeSlayer)
    .HasUnlockDependencies(9, 11);
skillAugmentation.AddNode(16)
    .Location(5, 4)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(12);
skillAugmentation.AddNode(17)
    .Location(6, 4)
    .BloodOrbCost(1700)
    .Unlocks(AbilityId.AlchemicEvadeSlayer)
    .HasUnlockDependencies(13);
skillAugmentation.AddNode(18)
    .Location(7, 4)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(14);

skillAugmentation.AddNode(19)
    .Location(1, 5)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(8);
skillAugmentation.AddNode(20)
    .Location(2, 5)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(8);
skillAugmentation.AddNode(21)
    .Location(3, 5)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(15);
skillAugmentation.AddNode(22)
    .Location(5, 5)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(16);

skillAugmentation.AddNode(23)
    .Location(1, 6)
    .BloodOrbCost(1600)
    .Unlocks(AbilityId.ElixerSlayer)
    .HasUnlockDependencies(19);
skillAugmentation.AddNode(24)
    .Location(2, 6)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 1)
    .HasUnlockDependencies(20);
skillAugmentation.AddNode(25)
    .Location(3, 6)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
    .HasUnlockDependencies(21);
skillAugmentation.AddNode(26)
    .Location(5, 6)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.AllJobsHpMax, 30)
    .HasUnlockDependencies(22);

skillAugmentation.AddNode(27)
    .Location(4, 7)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(25, 26);

skillAugmentation.AddNode(28)
    .Location(4, 8)
    .BloodOrbCost(1800)
    .Unlocks(AbilityId.EnduringVision)
    .HasUnlockDependencies(27);
#endregion

#region PAGE2
skillAugmentation.AddNode(29)
    .Location(4, 9)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependencies(28)
    .HasQuestDependency(QuestId.SkillAugmentationAlchemistTrialII);

skillAugmentation.AddNode(30)
    .Location(1, 10)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(29);
skillAugmentation.AddNode(31)
    .Location(3, 10)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(29);
skillAugmentation.AddNode(32)
    .Location(4, 10)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(29);
skillAugmentation.AddNode(33)
    .Location(5, 10)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(29);
skillAugmentation.AddNode(34)
    .Location(7, 10)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(29);

skillAugmentation.AddNode(35)
    .Location(1, 11)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(30);
skillAugmentation.AddNode(36)
    .Location(2, 11)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependencies(31);
skillAugmentation.AddNode(37)
    .Location(3, 11)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(31);
skillAugmentation.AddNode(38)
    .Location(4, 11)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependencies(32);
skillAugmentation.AddNode(39)
    .Location(5, 11)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependencies(33);
skillAugmentation.AddNode(40)
    .Location(6, 11)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(33);
skillAugmentation.AddNode(41)
    .Location(7, 11)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(34);

skillAugmentation.AddNode(42)
    .Location(1, 12)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(35);
skillAugmentation.AddNode(43)
    .Location(2, 12)
    .BloodOrbCost(1500)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(36);
skillAugmentation.AddNode(44)
    .Location(3, 12)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependencies(37);
skillAugmentation.AddNode(45)
    .Location(4, 12)
    .BloodOrbCost(1800)
    .Unlocks(AbilityId.AlchemicalRadiusSlayer)
    .HasUnlockDependencies(38);
skillAugmentation.AddNode(46)
    .Location(5, 12)
    .BloodOrbCost(1500)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(39);
skillAugmentation.AddNode(47)
    .Location(6, 12)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
    .HasUnlockDependencies(40);
skillAugmentation.AddNode(48)
    .Location(7, 12)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(41);

skillAugmentation.AddNode(49)
    .Location(1, 13)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(42);
skillAugmentation.AddNode(50)
    .Location(3, 13)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(44);
skillAugmentation.AddNode(51)
    .Location(5, 13)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(46);
skillAugmentation.AddNode(52)
    .Location(7, 13)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(48);

skillAugmentation.AddNode(53)
    .Location(2, 14)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.AllJobsHpMax, 35)
    .HasUnlockDependencies(49, 50);
skillAugmentation.AddNode(54)
    .Location(6, 14)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 2)
    .HasUnlockDependencies(51, 52);

skillAugmentation.AddNode(55)
    .Location(4, 15)
    .BloodOrbCost(1800)
    .Unlocks(AbilityId.Stubborn)
    .HasUnlockDependencies(53, 54);
#endregion

#region PAGE3
skillAugmentation.AddNode(56)
    .Location(4, 16)
    .BloodOrbCost(1300)
    .Unlocks(OrbGainParamType.JobStaminaMax, 25)
    .HasUnlockDependencies(55)
    .HasQuestDependency(QuestId.SkillAugmentationAlchemistTrialIII);

skillAugmentation.AddNode(57)
    .Location(3, 17)
    .BloodOrbCost(1450)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(56);
skillAugmentation.AddNode(58)
    .Location(4, 17)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(56);
skillAugmentation.AddNode(59)
    .Location(5, 17)
    .BloodOrbCost(1450)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(56);

skillAugmentation.AddNode(60)
    .Location(2, 18)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(57);
skillAugmentation.AddNode(61)
    .Location(3, 18)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(57);
skillAugmentation.AddNode(62)
    .Location(5, 18)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(59);
skillAugmentation.AddNode(63)
    .Location(6, 18)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(59);

skillAugmentation.AddNode(64)
    .Location(1, 19)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(60);
skillAugmentation.AddNode(65)
    .Location(2, 19)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(60);
skillAugmentation.AddNode(66)
    .Location(4, 19)
    .BloodOrbCost(2300)
    .Unlocks(CustomSkillId.DolusAeris)
    .HasUnlockDependencies(61, 62);
skillAugmentation.AddNode(67)
    .Location(6, 19)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(63);
skillAugmentation.AddNode(68)
    .Location(7, 19)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(63);

skillAugmentation.AddNode(69)
    .Location(2, 20)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(64, 65);
skillAugmentation.AddNode(70)
    .Location(4, 20)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
    .HasUnlockDependencies(66);
skillAugmentation.AddNode(71)
    .Location(6, 20)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(67, 68);

skillAugmentation.AddNode(72)
    .Location(2, 21)
    .BloodOrbCost(2600)
    .Unlocks(AbilityId.AlchemyCrusher)
    .HasUnlockDependencies(69);
skillAugmentation.AddNode(73)
    .Location(4, 21)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(70);
skillAugmentation.AddNode(74)
    .Location(6, 21)
    .BloodOrbCost(2600)
    .Unlocks(AbilityId.AlchemicEvadeCrusher)
    .HasUnlockDependencies(71);

skillAugmentation.AddNode(75)
    .Location(1, 22)
    .BloodOrbCost(2500)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(72);
skillAugmentation.AddNode(76)
    .Location(2, 22)
    .BloodOrbCost(2600)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(72);
skillAugmentation.AddNode(77)
    .Location(4, 22)
    .BloodOrbCost(2700)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
    .HasUnlockDependencies(73);
skillAugmentation.AddNode(78)
    .Location(6, 22)
    .BloodOrbCost(2600)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(74);
skillAugmentation.AddNode(79)
    .Location(7, 22)
    .BloodOrbCost(2700)
    .Unlocks(OrbGainParamType.JobHpMax, 40)
    .HasUnlockDependencies(74);

skillAugmentation.AddNode(80)
    .Location(2, 23)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 1)
    .HasUnlockDependencies(75, 76);
skillAugmentation.AddNode(81)
    .Location(4, 23)
    .BloodOrbCost(3000)
    .Unlocks(AbilityId.ElixirCrusher)
    .HasUnlockDependencies(77);
skillAugmentation.AddNode(82)
    .Location(6, 23)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsHpMax, 30)
    .HasUnlockDependencies(78, 79);

skillAugmentation.AddNode(83)
    .Location(4, 24)
    .BloodOrbCost(3000)
    .Unlocks(AbilityId.SkyAnnihilation)
    .HasUnlockDependencies(80, 82);
#endregion

#region PAGE4
skillAugmentation.AddNode(84)
    .Location(4, 25)
    .BloodOrbCost(1300)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependencies(83)
    .HasQuestDependency(QuestId.SkillAugmentationAlchemistTrialIV);

skillAugmentation.AddNode(85)
    .Location(3, 26)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(84);
skillAugmentation.AddNode(86)
    .Location(5, 26)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(84);

skillAugmentation.AddNode(87)
    .Location(2, 27)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(85);
skillAugmentation.AddNode(88)
    .Location(4, 27)
    .BloodOrbCost(1900)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(85, 86);
skillAugmentation.AddNode(89)
    .Location(6, 27)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(86);

skillAugmentation.AddNode(90)
    .Location(1, 28)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(87);
skillAugmentation.AddNode(91)
    .Location(3, 28)
    .BloodOrbCost(1900)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(88);
skillAugmentation.AddNode(92)
    .Location(5, 28)
    .BloodOrbCost(1900)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(88);
skillAugmentation.AddNode(93)
    .Location(7, 28)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(89);

skillAugmentation.AddNode(94)
    .Location(1, 29)
    .BloodOrbCost(2200)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependencies(90);
skillAugmentation.AddNode(95)
    .Location(3, 29)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(91);
skillAugmentation.AddNode(96)
    .Location(5, 29)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(92);
skillAugmentation.AddNode(97)
    .Location(7, 29)
    .BloodOrbCost(2200)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependencies(93);

skillAugmentation.AddNode(98)
    .Location(1, 30)
    .BloodOrbCost(2200)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(94);
skillAugmentation.AddNode(99)
    .Location(2, 30)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(87);
skillAugmentation.AddNode(100)
    .Location(4, 30)
    .BloodOrbCost(2200)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(95, 96);
skillAugmentation.AddNode(101)
    .Location(6, 30)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(89);
skillAugmentation.AddNode(102)
    .Location(7, 30)
    .BloodOrbCost(2200)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(97);

skillAugmentation.AddNode(103)
    .Location(2, 31)
    .BloodOrbCost(2500)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(98, 99);
skillAugmentation.AddNode(104)
    .Location(4, 31)
    .BloodOrbCost(2800)
    .Unlocks(AbilityId.AlchemicalRadiusCrusher)
    .HasUnlockDependencies(100);
skillAugmentation.AddNode(105)
    .Location(6, 31)
    .BloodOrbCost(2500)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(101, 102);

skillAugmentation.AddNode(106)
    .Location(2, 32)
    .BloodOrbCost(2900)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(103);
skillAugmentation.AddNode(107)
    .Location(3, 32)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsPhysicalDefence, 1)
    .HasUnlockDependencies(103);
skillAugmentation.AddNode(108)
    .Location(5, 32)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsHpMax, 30)
    .HasUnlockDependencies(105);
skillAugmentation.AddNode(109)
    .Location(6, 32)
    .BloodOrbCost(2900)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(105);

skillAugmentation.AddNode(110)
    .Location(4, 33)
    .BloodOrbCost(3200)
    .Unlocks(AbilityId.DefenseAlchemy)
    .HasUnlockDependencies(106, 109);
#endregion

return skillAugmentation;

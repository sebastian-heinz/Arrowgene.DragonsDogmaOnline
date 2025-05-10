public class SkillAugmentation : ISkillAugmentation
{
    public override JobId JobId => JobId.Warrior;
}

var skillAugmentation = new SkillAugmentation();

#region TIER1
skillAugmentation.AddNode(1)
    .Location(4, 1)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasQuestDependency(60200128);

skillAugmentation.AddNode(2)
    .Location(4, 2)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
    .HasUnlockDependency(1);

skillAugmentation.AddNode(3)
    .Location(3, 3)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependency(2);
skillAugmentation.AddNode(4)
    .Location(5, 3)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependency(2);

skillAugmentation.AddNode(5)
    .Location(4, 4)
    .BloodOrbCost(1200)
    .Unlocks(CustomSkillId.GreatGougingFang)
    .HasUnlockDependencies(3, 4);

skillAugmentation.AddNode(6)
    .Location(3, 5)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependency(5);
skillAugmentation.AddNode(7)
    .Location(5, 5)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependency(5);

skillAugmentation.AddNode(8)
    .Location(2, 6)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 15)
    .HasUnlockDependency(6);
skillAugmentation.AddNode(9)
    .Location(3, 6)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependency(6);
skillAugmentation.AddNode(10)
    .Location(5, 6)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependency(7);
skillAugmentation.AddNode(11)
    .Location(6, 6)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependency(7);

skillAugmentation.AddNode(12)
    .Location(1, 7)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependency(8);
skillAugmentation.AddNode(13)
    .Location(2, 7)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependency(8);
skillAugmentation.AddNode(14)
    .Location(3, 7)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependency(9);
skillAugmentation.AddNode(15)
    .Location(5, 7)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependency(10);
skillAugmentation.AddNode(16)
    .Location(7, 7)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobStaminaMax, 15)
    .HasUnlockDependency(11);

skillAugmentation.AddNode(17)
    .Location(1, 8)
    .BloodOrbCost(1200)
    .Unlocks(AbilityId.HackSlayer)
    .HasUnlockDependency(12);
skillAugmentation.AddNode(18)
    .Location(2, 8)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependency(13);
skillAugmentation.AddNode(19)
    .Location(3, 8)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependency(14);
skillAugmentation.AddNode(20)
    .Location(5, 8)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
    .HasUnlockDependency(15);
skillAugmentation.AddNode(21)
    .Location(6, 8)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependency(15);
skillAugmentation.AddNode(22)
    .Location(7, 8)
    .BloodOrbCost(1700)
    .Unlocks(AbilityId.InverseSlashCrusher)
    .HasUnlockDependency(16);

skillAugmentation.AddNode(23)
    .Location(2, 9)
    .BloodOrbCost(1700)
    .Unlocks(AbilityId.SavageLashEradicate)
    .HasUnlockDependency(18);
skillAugmentation.AddNode(24)
    .Location(3, 9)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1)
    .HasUnlockDependency(19);
skillAugmentation.AddNode(25)
    .Location(5, 9)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependency(20);
skillAugmentation.AddNode(26)
    .Location(6, 9)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.AllJobsMagicalDefence, 1)
    .HasUnlockDependency(21);

skillAugmentation.AddNode(27)
    .Location(4, 10)
    .BloodOrbCost(1800)
    .Unlocks(AbilityId.Brandish)
    .HasUnlockDependencies(24, 25);
#endregion

#region TIER2
skillAugmentation.AddNode(28)
    .Location(4, 11)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependency(27)
    .HasQuestDependency(60200129);

skillAugmentation.AddNode(29)
    .Location(2, 12)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependency(28);
skillAugmentation.AddNode(30)
    .Location(4, 12)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependency(28);
skillAugmentation.AddNode(31)
    .Location(6, 12)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependency(28);

skillAugmentation.AddNode(32)
    .Location(2, 13)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependency(29);
skillAugmentation.AddNode(33)
    .Location(3, 13)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependency(29);
skillAugmentation.AddNode(34)
    .Location(4, 13)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependency(30);
skillAugmentation.AddNode(35)
    .Location(5, 13)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependency(31);
skillAugmentation.AddNode(36)
    .Location(6, 13)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependency(31);

skillAugmentation.AddNode(37)
    .Location(2, 14)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependency(32);
skillAugmentation.AddNode(38)
    .Location(4, 14)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(33, 34, 35);
skillAugmentation.AddNode(39)
    .Location(6, 14)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependency(36);

skillAugmentation.AddNode(40)
    .Location(2, 15)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependency(37);
skillAugmentation.AddNode(41)
    .Location(6, 15)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependency(39);

skillAugmentation.AddNode(42)
    .Location(1, 16)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependency(40);
skillAugmentation.AddNode(43)
    .Location(2, 16)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependency(40);
skillAugmentation.AddNode(44)
    .Location(6, 16)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependency(41);
skillAugmentation.AddNode(45)
    .Location(7, 16)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependency(41);

skillAugmentation.AddNode(46)
    .Location(1, 17)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependency(42);
skillAugmentation.AddNode(47)
    .Location(2, 17)
    .BloodOrbCost(1700)
    .Unlocks(AbilityId.DevastateSlayer)
    .HasUnlockDependency(43);
skillAugmentation.AddNode(48)
    .Location(6, 17)
    .BloodOrbCost(1700)
    .Unlocks(AbilityId.AttackCover)
    .HasUnlockDependency(44);
skillAugmentation.AddNode(49)
    .Location(7, 17)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependency(45);

skillAugmentation.AddNode(50)
    .Location(2, 18)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.AllJobsHpMax, 25)
    .HasUnlockDependencies(47, 48);
skillAugmentation.AddNode(51)
    .Location(4, 18)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(47, 48);
skillAugmentation.AddNode(52)
    .Location(6, 18)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1)
    .HasUnlockDependencies(47, 48);

skillAugmentation.AddNode(53)
    .Location(3, 19)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependency(51);
skillAugmentation.AddNode(54)
    .Location(5, 19)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependency(51);

skillAugmentation.AddNode(55)
    .Location(4, 20)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(53, 54);
#endregion

#region TIER3
skillAugmentation.AddNode(56)
    .Location(2, 21)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependency(55)
    .HasQuestDependency(60200130);
skillAugmentation.AddNode(57)
    .Location(4, 21)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependency(55)
    .HasQuestDependency(60200130);
skillAugmentation.AddNode(58)
    .Location(6, 21)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependency(55)
    .HasQuestDependency(60200130);

skillAugmentation.AddNode(59)
    .Location(2, 22)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependency(56);
skillAugmentation.AddNode(60)
    .Location(3, 22)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependency(57);
skillAugmentation.AddNode(61)
    .Location(4, 22)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependency(57);
skillAugmentation.AddNode(62)
    .Location(5, 22)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependency(57);
skillAugmentation.AddNode(63)
    .Location(6, 22)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependency(58);

skillAugmentation.AddNode(64)
    .Location(4, 23)
    .BloodOrbCost(2300)
    .Unlocks(CustomSkillId.EarthquakeFang)
    .HasUnlockDependencies(60, 61, 62);

skillAugmentation.AddNode(65)
    .Location(1, 24)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependency(59);
skillAugmentation.AddNode(66)
    .Location(2, 24)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependency(59);
skillAugmentation.AddNode(67)
    .Location(3, 24)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependency(64);
skillAugmentation.AddNode(68)
    .Location(4, 24)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependency(64);
skillAugmentation.AddNode(69)
    .Location(5, 24)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependency(64);
skillAugmentation.AddNode(70)
    .Location(6, 24)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependency(63);
skillAugmentation.AddNode(71)
    .Location(7, 24)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependency(63);

skillAugmentation.AddNode(72)
    .Location(4, 25)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependency(68);

skillAugmentation.AddNode(73)
    .Location(2, 26)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependency(68);
skillAugmentation.AddNode(74)
    .Location(4, 26)
    .BloodOrbCost(2500)
    .Unlocks(AbilityId.HackCrusher)
    .HasUnlockDependency(68);
skillAugmentation.AddNode(75)
    .Location(6, 26)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependency(68);

skillAugmentation.AddNode(76)
    .Location(3, 27)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependency(74);
skillAugmentation.AddNode(77)
    .Location(5, 27)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependency(74);

skillAugmentation.AddNode(78)
    .Location(1, 28)
    .BloodOrbCost(2800)
    .Unlocks(AbilityId.InverseSlashExterminator)
    .HasUnlockDependency(73);
skillAugmentation.AddNode(79)
    .Location(2, 28)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependency(73);
skillAugmentation.AddNode(80)
    .Location(4, 28)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(76, 77);
skillAugmentation.AddNode(81)
    .Location(6, 28)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependency(75);
skillAugmentation.AddNode(82)
    .Location(7, 28)
    .BloodOrbCost(2800)
    .Unlocks(AbilityId.SavageLashCrusher)
    .HasUnlockDependency(75);

skillAugmentation.AddNode(83)
    .Location(2, 29)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1)
    .HasUnlockDependency(79);
skillAugmentation.AddNode(84)
    .Location(6, 29)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsHpMax, 25)
    .HasUnlockDependency(81);

skillAugmentation.AddNode(85)
    .Location(4, 30)
    .BloodOrbCost(3200)
    .Unlocks(AbilityId.GreatGrasp)
    .HasUnlockDependencies(83, 84);
#endregion

#region TIER4
skillAugmentation.AddNode(86)
    .Location(4, 31)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(85)
    .HasQuestDependency(60200131);

skillAugmentation.AddNode(87)
    .Location(1, 32)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(86);
skillAugmentation.AddNode(88)
    .Location(3, 32)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(86);
skillAugmentation.AddNode(89)
    .Location(4, 32)
    .BloodOrbCost(1500)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(86);
skillAugmentation.AddNode(90)
    .Location(5, 32)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(86);
skillAugmentation.AddNode(91)
    .Location(7, 32)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(86);

skillAugmentation.AddNode(92)
    .Location(2, 33)
    .BloodOrbCost(1850)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(88);
skillAugmentation.AddNode(93)
    .Location(3, 33)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(88);
skillAugmentation.AddNode(94)
    .Location(5, 33)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(90);
skillAugmentation.AddNode(95)
    .Location(6, 33)
    .BloodOrbCost(1850)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(90);

skillAugmentation.AddNode(96)
    .Location(2, 34)
    .BloodOrbCost(2050)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(92, 93);
skillAugmentation.AddNode(97)
    .Location(6, 34)
    .BloodOrbCost(2050)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(94, 95);

skillAugmentation.AddNode(98)
    .Location(1, 35)
    .BloodOrbCost(2300)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(87);
skillAugmentation.AddNode(99)
    .Location(3, 35)
    .BloodOrbCost(1950)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(89);
skillAugmentation.AddNode(100)
    .Location(5, 35)
    .BloodOrbCost(1950)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(89);
skillAugmentation.AddNode(101)
    .Location(7, 35)
    .BloodOrbCost(2300)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(91);

skillAugmentation.AddNode(102)
    .Location(2, 36)
    .BloodOrbCost(2800)
    .Unlocks(AbilityId.DevastateCrusher)
    .HasUnlockDependencies(96, 98);
skillAugmentation.AddNode(103)
    .Location(3, 36)
    .BloodOrbCost(2600)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(99);
skillAugmentation.AddNode(104)
    .Location(5, 36)
    .BloodOrbCost(2600)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(100);
skillAugmentation.AddNode(105)
    .Location(6, 36)
    .BloodOrbCost(2800)
    .Unlocks(AbilityId.ExcessGrudge)
    .HasUnlockDependencies(97, 101);

skillAugmentation.AddNode(106)
    .Location(2, 37)
    .BloodOrbCost(2900)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(102, 103, 104, 105);
skillAugmentation.AddNode(107)
    .Location(4, 37)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(102, 103, 104, 105);
skillAugmentation.AddNode(108)
    .Location(6, 37)
    .BloodOrbCost(2900)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependencies(102, 103, 104, 105);

skillAugmentation.AddNode(109)
    .Location(3, 38)
    .BloodOrbCost(3000)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(107);
skillAugmentation.AddNode(110)
    .Location(5, 38)
    .BloodOrbCost(3000)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(107);

skillAugmentation.AddNode(111)
    .Location(2, 39)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 1)
    .HasUnlockDependencies(106, 109);
skillAugmentation.AddNode(112)
    .Location(6, 39)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsHpMax, 20)
    .HasUnlockDependencies(108, 110);
#endregion

return skillAugmentation;

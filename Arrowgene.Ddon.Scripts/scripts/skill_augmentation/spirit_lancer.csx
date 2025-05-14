public class SkillAugmentation : ISkillAugmentation
{
    public override JobId JobId => JobId.SpiritLancer;
}

var skillAugmentation = new SkillAugmentation();

#region TIER1
skillAugmentation.AddNode(1)
    .Location(4, 1)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasQuestDependency(60200136);

skillAugmentation.AddNode(2)
    .Location(3, 2)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(1);
skillAugmentation.AddNode(3)
    .Location(4, 2)
    .BloodOrbCost(700)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependencies(1);
skillAugmentation.AddNode(4)
    .Location(5, 2)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(1);

skillAugmentation.AddNode(5)
    .Location(2, 3)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(2);
skillAugmentation.AddNode(6)
    .Location(4, 3)
    .BloodOrbCost(1100)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(3);
skillAugmentation.AddNode(7)
    .Location(6, 3)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(4);

skillAugmentation.AddNode(8)
    .Location(1, 4)
    .BloodOrbCost(1300)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(5);
skillAugmentation.AddNode(9)
    .Location(4, 4)
    .BloodOrbCost(1100)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(6);
skillAugmentation.AddNode(10)
    .Location(7, 4)
    .BloodOrbCost(1300)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(7);

skillAugmentation.AddNode(11)
    .Location(2, 5)
    .BloodOrbCost(1100)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(5);
skillAugmentation.AddNode(12)
    .Location(4, 5)
    .BloodOrbCost(1100)
    .Unlocks(CustomSkillId.CureGlasta)
    .HasUnlockDependencies(9);
skillAugmentation.AddNode(13)
    .Location(6, 5)
    .BloodOrbCost(1100)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(7);

skillAugmentation.AddNode(14)
    .Location(2, 6)
    .BloodOrbCost(1450)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(8, 11);
skillAugmentation.AddNode(15)
    .Location(3, 6)
    .BloodOrbCost(1450)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(12);
skillAugmentation.AddNode(16)
    .Location(5, 6)
    .BloodOrbCost(1450)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(12);
skillAugmentation.AddNode(17)
    .Location(6, 6)
    .BloodOrbCost(1450)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(10, 13);

skillAugmentation.AddNode(18)
    .Location(2, 7)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependencies(14, 15);
skillAugmentation.AddNode(19)
    .Location(6, 7)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependencies(16, 17);

skillAugmentation.AddNode(20)
    .Location(1, 8)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
    .HasUnlockDependencies(18);
skillAugmentation.AddNode(21)
    .Location(3, 8)
    .BloodOrbCost(1800)
    .Unlocks(AbilityId.RushingSpearSlayer)
    .HasUnlockDependencies(18);
skillAugmentation.AddNode(22)
    .Location(5, 8)
    .BloodOrbCost(1800)
    .Unlocks(AbilityId.RisingSpearSlayer)
    .HasUnlockDependencies(19);
skillAugmentation.AddNode(23)
    .Location(7, 8)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
    .HasUnlockDependencies(19);

skillAugmentation.AddNode(24)
    .Location(4, 9)
    .BloodOrbCost(1900)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(21, 22);

skillAugmentation.AddNode(25)
    .Location(3, 10)
    .BloodOrbCost(2000)
    .Unlocks(AbilityId.CrushingSpearSlayer)
    .HasUnlockDependencies(20);
skillAugmentation.AddNode(26)
    .Location(5, 10)
    .BloodOrbCost(2000)
    .Unlocks(AbilityId.EnhancedVitality)
    .HasUnlockDependencies(23);
#endregion

#region TIER2
skillAugmentation.AddNode(27)
    .Location(4, 11)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(25, 26)
    .HasQuestDependency(60200137);

skillAugmentation.AddNode(28)
    .Location(3, 12)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(27);
skillAugmentation.AddNode(29)
    .Location(4, 12)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependencies(27);
skillAugmentation.AddNode(30)
    .Location(5, 12)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(27);

skillAugmentation.AddNode(31)
    .Location(3, 13)
    .BloodOrbCost(1100)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(28);
skillAugmentation.AddNode(32)
    .Location(4, 13)
    .BloodOrbCost(1100)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(29);
skillAugmentation.AddNode(33)
    .Location(5, 13)
    .BloodOrbCost(1100)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(30);

skillAugmentation.AddNode(34)
    .Location(3, 14)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(31);
skillAugmentation.AddNode(35)
    .Location(4, 14)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(32);
skillAugmentation.AddNode(36)
    .Location(5, 14)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(33);

skillAugmentation.AddNode(37)
    .Location(3, 15)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(34);
skillAugmentation.AddNode(38)
    .Location(4, 15)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(35);
skillAugmentation.AddNode(39)
    .Location(5, 15)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(36);

skillAugmentation.AddNode(40)
    .Location(3, 16)
    .BloodOrbCost(1500)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(37);
skillAugmentation.AddNode(41)
    .Location(4, 16)
    .BloodOrbCost(1500)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(38);
skillAugmentation.AddNode(42)
    .Location(5, 16)
    .BloodOrbCost(1500)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(39);

skillAugmentation.AddNode(43)
    .Location(1, 17)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependencies(40);
skillAugmentation.AddNode(44)
    .Location(4, 17)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(41);
skillAugmentation.AddNode(45)
    .Location(7, 17)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependencies(42);

skillAugmentation.AddNode(46)
    .Location(2, 18)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(43);
skillAugmentation.AddNode(47)
    .Location(4, 18)
    .BloodOrbCost(1800)
    .Unlocks(AbilityId.SweepingSpearSlayer)
    .HasUnlockDependencies(44);
skillAugmentation.AddNode(48)
    .Location(6, 18)
    .BloodOrbCost(1700)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(45);

skillAugmentation.AddNode(49)
    .Location(3, 19)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(46);
skillAugmentation.AddNode(50)
    .Location(5, 19)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(48);

skillAugmentation.AddNode(51)
    .Location(4, 20)
    .BloodOrbCost(2000)
    .Unlocks(AbilityId.ElementalDefense)
    .HasUnlockDependencies(49, 50);
#endregion

#region TIER3
skillAugmentation.AddNode(52)
    .Location(2, 21)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(51)
    .HasQuestDependency(60200138);
skillAugmentation.AddNode(53)
    .Location(6, 21)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(51)
    .HasQuestDependency(60200138);

skillAugmentation.AddNode(54)
    .Location(1, 22)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(52);
skillAugmentation.AddNode(55)
    .Location(3, 22)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(52);
skillAugmentation.AddNode(56)
    .Location(5, 22)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(53);
skillAugmentation.AddNode(57)
    .Location(7, 22)
    .BloodOrbCost(1600)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(53);

skillAugmentation.AddNode(58)
    .Location(2, 23)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(54);
skillAugmentation.AddNode(59)
    .Location(4, 23)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(55, 56);
skillAugmentation.AddNode(60)
    .Location(6, 23)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(57);

skillAugmentation.AddNode(61)
    .Location(2, 24)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(58);
skillAugmentation.AddNode(62)
    .Location(4, 24)
    .BloodOrbCost(2300)
    .Unlocks(CustomSkillId.ScriosGuard)
    .HasUnlockDependencies(59);
skillAugmentation.AddNode(63)
    .Location(6, 24)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(60);

skillAugmentation.AddNode(64)
    .Location(1, 25)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(61);
skillAugmentation.AddNode(65)
    .Location(3, 25)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(61);
skillAugmentation.AddNode(66)
    .Location(5, 25)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(63);
skillAugmentation.AddNode(67)
    .Location(7, 25)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(63);

skillAugmentation.AddNode(68)
    .Location(2, 26)
    .BloodOrbCost(2300)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
    .HasUnlockDependencies(64);
skillAugmentation.AddNode(69)
    .Location(4, 26)
    .BloodOrbCost(2500)
    .Unlocks(AbilityId.RushingSpearDestroyer)
    .HasUnlockDependencies(65, 66);
skillAugmentation.AddNode(70)
    .Location(6, 26)
    .BloodOrbCost(2300)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
    .HasUnlockDependencies(67);

skillAugmentation.AddNode(71)
    .Location(3, 27)
    .BloodOrbCost(2150)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(69);
skillAugmentation.AddNode(72)
    .Location(5, 27)
    .BloodOrbCost(2150)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(69);

skillAugmentation.AddNode(73)
    .Location(1, 28)
    .BloodOrbCost(2750)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(68);
skillAugmentation.AddNode(74)
    .Location(3, 28)
    .BloodOrbCost(2800)
    .Unlocks(AbilityId.RisingSpearDestroyer)
    .HasUnlockDependencies(71);
skillAugmentation.AddNode(75)
    .Location(5, 28)
    .BloodOrbCost(2800)
    .Unlocks(AbilityId.CrushingSpearDestroyer)
    .HasUnlockDependencies(72);
skillAugmentation.AddNode(76)
    .Location(7, 28)
    .BloodOrbCost(2750)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(70);

skillAugmentation.AddNode(77)
    .Location(2, 29)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsHpMax, 30)
    .HasUnlockDependencies(73);
skillAugmentation.AddNode(78)
    .Location(6, 29)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 10)
    .HasUnlockDependencies(76);

skillAugmentation.AddNode(79)
    .Location(4, 30)
    .BloodOrbCost(3200)
    .Unlocks(AbilityId.GreatEnchantment)
    .HasUnlockDependencies(77, 78);
#endregion

#region TIER4
skillAugmentation.AddNode(80)
    .Location(4, 31)
    .BloodOrbCost(1400)
    .Unlocks(OrbGainParamType.JobHpMax, 25)
    .HasUnlockDependencies(79)
    .HasQuestDependency(60200139);

skillAugmentation.AddNode(81)
    .Location(2, 32)
    .BloodOrbCost(1650)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(80);
skillAugmentation.AddNode(82)
    .Location(6, 32)
    .BloodOrbCost(1650)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(80);

skillAugmentation.AddNode(83)
    .Location(1, 33)
    .BloodOrbCost(1950)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(81);
skillAugmentation.AddNode(84)
    .Location(3, 33)
    .BloodOrbCost(1950)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(81);
skillAugmentation.AddNode(85)
    .Location(5, 33)
    .BloodOrbCost(1950)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(82);
skillAugmentation.AddNode(86)
    .Location(7, 33)
    .BloodOrbCost(1950)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(82);

skillAugmentation.AddNode(87)
    .Location(2, 34)
    .BloodOrbCost(2200)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(84, 85);
skillAugmentation.AddNode(88)
    .Location(4, 34)
    .BloodOrbCost(2200)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(84, 85);
skillAugmentation.AddNode(89)
    .Location(6, 34)
    .BloodOrbCost(2200)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(84, 85);

skillAugmentation.AddNode(90)
    .Location(1, 35)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependencies(83);
skillAugmentation.AddNode(91)
    .Location(3, 35)
    .BloodOrbCost(2900)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
    .HasUnlockDependencies(88);
skillAugmentation.AddNode(92)
    .Location(5, 35)
    .BloodOrbCost(2900)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
    .HasUnlockDependencies(88);
skillAugmentation.AddNode(93)
    .Location(7, 35)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependencies(86);

skillAugmentation.AddNode(94)
    .Location(2, 36)
    .BloodOrbCost(2600)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(87);
skillAugmentation.AddNode(95)
    .Location(4, 36)
    .BloodOrbCost(2500)
    .Unlocks(OrbGainParamType.JobHpMax, 30)
    .HasUnlockDependencies(88);
skillAugmentation.AddNode(96)
    .Location(6, 36)
    .BloodOrbCost(2600)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasUnlockDependencies(89);

skillAugmentation.AddNode(97)
    .Location(1, 37)
    .BloodOrbCost(2700)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependencies(90);
skillAugmentation.AddNode(98)
    .Location(7, 37)
    .BloodOrbCost(2700)
    .Unlocks(OrbGainParamType.JobHpMax, 35)
    .HasUnlockDependencies(93);

skillAugmentation.AddNode(99)
    .Location(2, 38)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependencies(94, 97);
skillAugmentation.AddNode(100)
    .Location(4, 38)
    .BloodOrbCost(2800)
    .Unlocks(AbilityId.SweepingSpearDestroyer)
    .HasUnlockDependencies(91, 92);
skillAugmentation.AddNode(101)
    .Location(6, 38)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependencies(96, 98);

skillAugmentation.AddNode(102)
    .Location(3, 39)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsHpMax, 30)
    .HasUnlockDependencies(99);
skillAugmentation.AddNode(103)
    .Location(5, 39)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.AllJobsStaminaMax, 10)
    .HasUnlockDependencies(101);

skillAugmentation.AddNode(104)
    .Location(4, 40)
    .BloodOrbCost(3200)
    .Unlocks(AbilityId.SpiritHoard)
    .HasUnlockDependencies(102, 103);
#endregion

return skillAugmentation;

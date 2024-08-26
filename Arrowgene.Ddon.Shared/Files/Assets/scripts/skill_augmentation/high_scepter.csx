public class SkillAugmentation : ISkillAugmentation
{
    public override JobId JobId => JobId.HighScepter;
}

var skillAugmentation = new SkillAugmentation();
// 1st Layer
skillAugmentation.AddNode(0)
    .Location(4, 1)
    .BloodOrbCost(500)
    .Unlocks(OrbGainParamType.JobStaminaMax, 10)
    .HasQuestDependency(60300400);

// 2nd Layer
skillAugmentation.AddNode(1)
    .Location(3, 2)
    .BloodOrbCost(700)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependency(0);      
skillAugmentation.AddNode(2)
    .Location(5, 2)
    .BloodOrbCost(700)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependency(0);

// 3rd Layer
skillAugmentation.AddNode(3)
    .Location(2, 3)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasUnlockDependency(1);
skillAugmentation.AddNode(4)
    .Location(3, 3)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependency(1);
skillAugmentation.AddNode(5)
    .Location(5, 3)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependency(2);
skillAugmentation.AddNode(6)
    .Location(6, 3)
    .BloodOrbCost(800)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasUnlockDependency(2);

// 4th Layer
skillAugmentation.AddNode(7)
    .Location(2, 4)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasUnlockDependency(3); 
skillAugmentation.AddNode(8)
    .Location(4, 4)
    .BloodOrbCost(1000)
    .Unlocks(CustomSkillId.EclipseBright)
    .HasUnlockDependencies(4, 5);
skillAugmentation.AddNode(9)
    .Location(6, 4)
    .BloodOrbCost(1000)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasUnlockDependency(6);
                
// 5th Layer
skillAugmentation.AddNode(10)
    .Location(2, 5)
    .BloodOrbCost(1100)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasUnlockDependency(7);
skillAugmentation.AddNode(11)
    .Location(3, 5)
    .BloodOrbCost(1100)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 2)
    .HasUnlockDependency(7);
                
skillAugmentation.AddNode(12)
    .Location(5, 5)
    .BloodOrbCost(1100)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 2)
    .HasUnlockDependency(9);
skillAugmentation.AddNode(13)
    .Location(6, 5)
    .BloodOrbCost(1100)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasUnlockDependency(9);

// 6th Layer
skillAugmentation.AddNode(14)
    .Location(3, 6)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
    .HasUnlockDependency(11);
skillAugmentation.AddNode(15)
    .Location(5, 6)
    .BloodOrbCost(1200)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
    .HasUnlockDependency(12);

// 7th Layer
skillAugmentation.AddNode(16)
    .Location(1, 7)
    .BloodOrbCost(1300)
    .Unlocks(OrbGainParamType.JobStaminaMax, 20)
    .HasUnlockDependency(14);
skillAugmentation.AddNode(17)
    .Location(3, 7)
    .BloodOrbCost(1500)
    .Unlocks(AbilityId.ArcSlashSlayer)
    .HasUnlockDependency(14);
skillAugmentation.AddNode(18)
    .Location(5, 7)
    .BloodOrbCost(1500)
    .Unlocks(AbilityId.SkySlashSlayer)
    .HasUnlockDependency(15);
skillAugmentation.AddNode(19)
    .Location(7, 7)
    .BloodOrbCost(1300)
    .Unlocks(OrbGainParamType.JobStaminaMax, 20)
    .HasUnlockDependency(15);
                
// 8th Layer
skillAugmentation.AddNode(20)
    .Location(1, 8)
    .BloodOrbCost(1500)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasUnlockDependency(16);
skillAugmentation.AddNode(21)
    .Location(2, 8)
    .BloodOrbCost(1300)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 2)
    .HasUnlockDependency(16);
skillAugmentation.AddNode(22)
    .Location(4, 8)
    .BloodOrbCost(1500)
    .Unlocks(AbilityId.QuadrupleSlashSlayer)
    .HasUnlockDependencies(17, 18);
skillAugmentation.AddNode(23)
    .Location(6, 8)
    .BloodOrbCost(1300)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 2)
    .HasUnlockDependency(19);
skillAugmentation.AddNode(24)
    .Location(7, 8)
    .BloodOrbCost(1500)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasUnlockDependency(19);
                
// 9th Layer
skillAugmentation.AddNode(25)
    .Location(2, 9)
    .BloodOrbCost(1500)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
    .HasUnlockDependency(21);
skillAugmentation.AddNode(26)
    .Location(6, 9)
    .BloodOrbCost(1500)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
    .HasUnlockDependency(23);

// 10th Layer
skillAugmentation.AddNode(27)
    .Location(1, 10)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2)
    .HasUnlockDependency(20);
skillAugmentation.AddNode(28)
    .Location(4, 10)
    .BloodOrbCost(1800)
    .Unlocks(AbilityId.FallingSlashSlayer)
    .HasUnlockDependencies(25, 26);
skillAugmentation.AddNode(29)
    .Location(7, 10)
    .BloodOrbCost(1800)
    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2)
    .HasUnlockDependency(24);

// 11th Layer
skillAugmentation.AddNode(30)
    .Location(4, 11)
    .BloodOrbCost(2000)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasUnlockDependency(28);
                
// 12th Layer
skillAugmentation.AddNode(31)
    .Location(1, 12)
    .BloodOrbCost(2200)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 1)
    .HasUnlockDependency(30);
skillAugmentation.AddNode(32)
    .Location(7, 12)
    .BloodOrbCost(2200)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 1)
    .HasUnlockDependency(30);
                
// 13th Layer
skillAugmentation.AddNode(33)
    .Location(1, 13)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 1)
    .HasUnlockDependency(31);
skillAugmentation.AddNode(34)
    .Location(2, 13)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasUnlockDependency(31);
skillAugmentation.AddNode(35)
    .Location(3, 13)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobStaminaMax, 15)
    .HasUnlockDependency(31);
skillAugmentation.AddNode(36)
    .Location(5, 13)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobStaminaMax, 15)
    .HasUnlockDependency(32);
skillAugmentation.AddNode(37)
    .Location(6, 13)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasUnlockDependency(32);
skillAugmentation.AddNode(38)
    .Location(7, 13)
    .BloodOrbCost(2400)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 1)
    .HasUnlockDependency(32);
                
// 14th Layer
skillAugmentation.AddNode(39)
    .Location(1, 14)
    .BloodOrbCost(2600)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 2)
    .HasUnlockDependency(33);
skillAugmentation.AddNode(40)
    .Location(2, 14)
    .BloodOrbCost(2600)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasUnlockDependency(34);
skillAugmentation.AddNode(41)
    .Location(3, 14)
    .BloodOrbCost(2600)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
    .HasUnlockDependency(35);
skillAugmentation.AddNode(42)
    .Location(5, 14)
    .BloodOrbCost(2600)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
    .HasUnlockDependency(36);
skillAugmentation.AddNode(43)
    .Location(6, 14)
    .BloodOrbCost(2600)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasUnlockDependency(37);
skillAugmentation.AddNode(44)
    .Location(7, 14)
    .BloodOrbCost(2600)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 2)
    .HasUnlockDependency(38);
                
// 15th Layer
skillAugmentation.AddNode(45)
    .Location(1, 15)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.JobMagicalDefence, 2)
    .HasUnlockDependency(39);
skillAugmentation.AddNode(46)
    .Location(2, 15)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasUnlockDependency(40);
skillAugmentation.AddNode(47)
    .Location(6, 15)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.JobHpMax, 50)
    .HasUnlockDependency(43);
skillAugmentation.AddNode(48)
    .Location(7, 15)
    .BloodOrbCost(2800)
    .Unlocks(OrbGainParamType.JobPhysicalDefence, 2)
    .HasUnlockDependency(44);
                
// 16th Layer
skillAugmentation.AddNode(49)
    .Location(1, 16)
    .BloodOrbCost(3000)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
    .HasUnlockDependency(45);
skillAugmentation.AddNode(50)
    .Location(2, 16)
    .BloodOrbCost(3000)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 2)
    .HasUnlockDependency(46);
skillAugmentation.AddNode(51)
    .Location(6, 16)
    .BloodOrbCost(3000)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
    .HasUnlockDependency(47);
skillAugmentation.AddNode(52)
    .Location(7, 16)
    .BloodOrbCost(3000)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 2)
    .HasUnlockDependency(48);

// 17th Layer
skillAugmentation.AddNode(53)
    .Location(1, 17)
    .BloodOrbCost(3300)
    .Unlocks(AbilityId.OrdinaryAttack)
    .HasUnlockDependency(49);
skillAugmentation.AddNode(54)
    .Location(7, 17)
    .BloodOrbCost(3300)
    .Unlocks(AbilityId.Respiration)
    .HasUnlockDependency(52);

// 18th Layer
skillAugmentation.AddNode(55)
    .Location(2, 18)
    .BloodOrbCost(3300)
    .Unlocks(OrbGainParamType.JobMagicalAttack, 3)
    .HasUnlockDependency(50);
skillAugmentation.AddNode(56)
    .Location(6, 18)
    .BloodOrbCost(3300)
    .Unlocks(OrbGainParamType.JobPhysicalAttack, 3)
    .HasUnlockDependency(51);

// 19th Layer
skillAugmentation.AddNode(57)
    .Location(4, 19)
    .BloodOrbCost(3300)
    .Unlocks(OrbGainParamType.JobStaminaMax, 20)
    .HasUnlockDependencies(55, 56);

// 20th Layer
skillAugmentation.AddNode(58)
    .Location(3, 20)
    .BloodOrbCost(4000)
    .Unlocks(OrbGainParamType.AllJobsMagicalAttack, 2)
    .HasUnlockDependency(57);
skillAugmentation.AddNode(59)
    .Location(5, 20)
    .BloodOrbCost(4000)
    .Unlocks(OrbGainParamType.AllJobsPhysicalAttack, 2)
    .HasUnlockDependency(57);

return skillAugmentation;

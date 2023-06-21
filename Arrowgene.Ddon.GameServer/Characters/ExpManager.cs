#nullable enable
using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Characters
{
    struct BaseStats
    {
        public ushort Lv;
        public ushort Atk;
        public double AtkRate;
        public ushort Def;
        public double DefRate;
        public ushort MAtk;
        public double MAtkRate;
        public ushort MDef;
        public double MDefRate;
    }

    public class ExpManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ExpManager));

        private static readonly byte LV_CAP = 120;

        // E.g. EXP_UNTIL_NEXT_LV[3] = 800, meaning as Lv 3 you need 800 exp to level to Lv 4
        public static readonly uint[] EXP_UNTIL_NEXT_LV = new uint[] {
            /********/   0,
            /* Lv 1 */   300,
            /* Lv 2 */   500,
            /* Lv 3 */   800,
            /* Lv 4 */   1200,
            /* Lv 5 */   1700,
            /* Lv 6 */   2300,
            /* Lv 7 */   3000,
            /* Lv 8 */   3800,
            /* Lv 9 */   4700,
            /* Lv 10 */  5700,
            /* Lv 11 */  6800,
            /* Lv 12 */  8000,
            /* Lv 13 */  9300,
            /* Lv 14 */  10700,
            /* Lv 15 */  12200,
            /* Lv 16 */  13800,
            /* Lv 17 */  15500,
            /* Lv 18 */  17300,
            /* Lv 19 */  19200,
            /* Lv 20 */  21200,
            /* Lv 21 */  23300,
            /* Lv 22 */  25500,
            /* Lv 23 */  27800,
            /* Lv 24 */  30200,
            /* Lv 25 */  32700,
            /* Lv 26 */  35300,
            /* Lv 27 */  38000,
            /* Lv 28 */  40800,
            /* Lv 29 */  43700,
            /* Lv 30 */  46700,
            /* Lv 31 */  49800,
            /* Lv 32 */  53000,
            /* Lv 33 */  56300,
            /* Lv 34 */  59700,
            /* Lv 35 */  63200,
            /* Lv 36 */  66800,
            /* Lv 37 */  70500,
            /* Lv 38 */  74300,
            /* Lv 39 */  78200,
            /* Lv 40 */  152500,
            /* Lv 41 */  187100,
            /* Lv 42 */  210000,
            /* Lv 43 */  235300,
            /* Lv 44 */  263200,
            /* Lv 45 */  267700,
            /* Lv 46 */  272300,
            /* Lv 47 */  277000,
            /* Lv 48 */  281800,
            /* Lv 49 */  286700,
            /* Lv 50 */  291700,
            /* Lv 51 */  296800,
            /* Lv 52 */  302000,
            /* Lv 53 */  307300,
            /* Lv 54 */  312700,
            /* Lv 55 */  318200,
            /* Lv 56 */  323800,
            /* Lv 57 */  329500,
            /* Lv 58 */  335300,
            /* Lv 59 */  341200,
            /* Lv 60 */  756600,
            /* Lv 61 */  762700,
            /* Lv 62 */  768900,
            /* Lv 63 */  775200,
            /* Lv 64 */  781600,
            /* Lv 65 */  788100,
            /* Lv 66 */  985000,
            /* Lv 67 */  1085000,
            /* Lv 68 */  1185000,
            /* Lv 69 */  1335000,
            /* Lv 70 */  1535000, // (PP Unlocked)
            /* Lv 71 */  1735000,
            /* Lv 72 */  1935000,
            /* Lv 73 */  2185000,
            /* Lv 74 */  2435000,
            /* Lv 75 */  2735000,
            /* Lv 76 */  3035000,
            /* Lv 77 */  3335000,
            /* Lv 78 */  3685000,
            /* Lv 79 */  4035000,
            /* Lv 80 */  4200000, 
            /* Lv 81 */  4200000,
            /* Lv 82 */  4200000,
            /* Lv 83 */  4200000,
            /* Lv 84 */  4200000,
            /* Lv 85 */  4200000,
            /* Lv 86 */  4200000,
            /* Lv 87 */  4200000,
            /* Lv 88 */  4200000,
            /* Lv 89 */  4200000,
            /* Lv 90 */  4200000,
            /* Lv 91 */  4200000,
            /* Lv 92 */  4200000,
            /* Lv 93 */  4200000,
            /* Lv 94 */  4200000,
            /* Lv 95 */  4200000,
            /* Lv 96 */  4200000,
            /* Lv 97 */  4200000,
            /* Lv 98 */  4200000,
            /* Lv 99 */  4200000,
            /* Lv 100 */ 4461000,
            /* Lv 101 */ 5000000,
            /* Lv 102 */ 5000000,
            /* Lv 103 */ 5000000,
            /* Lv 104 */ 5000000,
            /* Lv 105 */ 5000000,
            /* Lv 106 */ 5000000,
            /* Lv 107 */ 5000000,
            /* Lv 108 */ 5000000,
            /* Lv 109 */ 5000000,
            /* Lv 110 */ 5000000,
            /* Lv 111 */ 5000000,
            /* Lv 112 */ 5000000,
            /* Lv 113 */ 5000000,
            /* Lv 114 */ 5000000,
            /* Lv 115 */ 5000000,
            /* Lv 116 */ 5000000,
            /* Lv 117 */ 5000000,
            /* Lv 118 */ 5000000,
            /* Lv 119 */ 5000000,
        };
        
        // E.g. LEVEL_UP_JOB_POINTS_EARNED[3] = 300, meaning you earn 300 JP when you reach Lv 3
        public static readonly uint[] LEVEL_UP_JOB_POINTS_EARNED = new uint[] {0,200,200,300,300,400,400,500,600,700,700,800,1000,1200,1400,1600,1800,2000,2300,2600,2900,3300,3500,3800,3800,4000,4000,4500,4500,5000,5000,5500,5800,5800,6500,6500,6800,6800,8000,8000,9000,10000,10000,10000,10000,10000,10000,10000,10000,10000,10000,10000,10000,10000,10000,10000,10000,10000,10000,10000,20000,20000,20000,20000,20000,20000,20000,20000,20000,20000,20000,20000,20000,20000,20000,20000,20000,20000,20000,20000,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

        // Growth sources: https://wiki.famitsu.com/dd-online/%E3%82%B8%E3%83%A7%E3%83%96/%E3%83%95%E3%82%A1%E3%82%A4%E3%82%BF%E3%83%BC
        private static readonly Dictionary<JobId, BaseStats> BASE_STATS_TABLE = new Dictionary<JobId, BaseStats>()
            {
                {
                    // Fighter growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 120(base lv1) +1.2 / Magi Atk 20(base lv1) +2 / Magi Def 83(base lv1) +0.8 / Blow 25(base lv1) +0
                    JobId.Fighter,
                    new BaseStats() {
                        Lv = 1,
                        Atk = 30,
                        AtkRate = 3,
                        Def = 120,
                        DefRate = 1.2,
                        MAtk = 20,
                        MAtkRate = 2,
                        MDef = 83,
                        MDefRate = 0.8
                    }
                },
                {
                    // Seeker growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 83(base lv1) +0.8 / Magi Atk 20(base lv1) +2 / Magi Def 83(base lv1) +0.8 / Blow 10(base lv1) +0
                    JobId.Seeker,
                    new BaseStats() {
                        Lv = 1,
                        Atk = 30,
                        AtkRate = 3,
                        Def = 83,
                        DefRate = 0.8,
                        MAtk = 20,
                        MAtkRate = 2,
                        MDef = 83,
                        MDefRate = 0.8
                    }
                },
                {
                    // Hunter growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 83(base lv1) +0.8 / Magi Atk 20(base lv1) +2 / Magi Def 83(base lv1) +0.8 / Blow 25(base lv1) +0
                    JobId.Hunter,
                    new BaseStats() {
                        Lv = 1,
                        Atk = 30,
                        AtkRate = 3,
                        Def = 83,
                        DefRate = 0.8,
                        MAtk = 20,
                        MAtkRate = 2,
                        MDef = 83,
                        MDefRate = 0.8
                    }
                },
                {
                    // Priest growth per lv: Phys Atk 20(base lv1) +2 / Phys Def 58(base lv1) +0.4 / Magi Atk 30(base lv1) +3 / Magi Def 100(base lv1) +1 / Blow 18(base lv1) +0
                    JobId.Priest,
                    new BaseStats() {
                        Lv = 1,
                        Atk = 20,
                        AtkRate = 2,
                        Def = 58,
                        DefRate = 0.4,
                        MAtk = 30,
                        MAtkRate = 3,
                        MDef = 100,
                        MDefRate = 1
                    }
                },
                {
                    // ShieldSage growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 120(base lv1) +1.2 / Magi Atk 30(base lv1) +3 / Magi Def 120(base lv1) +1.2 / Blow 30(base lv1) +0
                    JobId.ShieldSage,
                    new BaseStats() {
                        Lv = 1,
                        Atk = 30,
                        AtkRate = 3,
                        Def = 120,
                        DefRate = 1.2,
                        MAtk = 30,
                        MAtkRate = 3,
                        MDef = 120,
                        MDefRate = 1.2
                    }
                },
                {
                    // Sorcerer growth per lv: Phys Atk 20(base lv1) +2 / Phys Def 58(base lv1) +0.4 / Magi Atk 30(base lv1) +3 / Magi Def 100(base lv1) +1 / Blow 30(base lv1) +0
                    JobId.Sorcerer,
                    new BaseStats() {
                        Lv = 1,
                        Atk = 20,
                        AtkRate = 2,
                        Def = 58,
                        DefRate = 0.4,
                        MAtk = 30,
                        MAtkRate = 3,
                        MDef = 100,
                        MDefRate = 1
                    }
                },
                {
                    // Warrior growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 120(base lv1) +1.2 / Magi Atk 20(base lv1) +2 / Magi Def 83(base lv1) +0.8 / Blow 35(base lv1) +0
                    JobId.Warrior,
                    new BaseStats() {
                        Lv = 1,
                        Atk = 30,
                        AtkRate = 3,
                        Def = 120,
                        DefRate = 1.2,
                        MAtk = 20,
                        MAtkRate = 2,
                        MDef = 83,
                        MDefRate = 0.8
                    }
                },
                {
                    // ElementArcher growth per lv: Phys Atk 20(base lv1) +2 / Phys Def 58(base lv1) +0.4 / Magi Atk 30(base lv1) +3 / Magi Def 100(base lv1) +1 / Blow 18(base lv1) +0
                    JobId.ElementArcher,
                    new BaseStats() {
                        Lv = 1,
                        Atk = 20,
                        AtkRate = 2,
                        Def = 58,
                        DefRate = 0.4,
                        MAtk = 30,
                        MAtkRate = 3,
                        MDef = 100,
                        MDefRate = 1
                    }
                },
                {
                    // Growth sources: https://dogmaonlineyyy.wiki.fc2.com/wiki/%E3%82%A2%E3%83%AB%E3%82%B1%E3%83%9F%E3%82%B9%E3%83%88
                    // Growth not actually there, it's just following the above logic and patterns
                    // Alchemist growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 100(base lv1) +1 / Magi Atk 20(base lv1) +2 / Magi Def 100(base lv1) +1 / Blow 25(base lv1) +0
                    JobId.Alchemist,
                    new BaseStats() {
                        Lv = 1,
                        Atk = 30,
                        AtkRate = 3,
                        Def = 100,
                        DefRate = 1,
                        MAtk = 20,
                        MAtkRate = 2,
                        MDef = 100,
                        MDefRate = 1
                    }
                },
                {
                    // Confirmed statuses and added proper growth rate
                    // SpiritLancer growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 83(base lv1) +0.8 / Magi Atk 30(base lv1) +3 / Magi Def 83(base lv1) +0.8 / Blow 25(base lv1) +0
                    JobId.SpiritLancer,
                    new BaseStats() {
                        Lv = 1,
                        Atk = 30,
                        AtkRate = 3,
                        Def = 83,
                        DefRate = 0.8,
                        MAtk = 30,
                        MAtkRate = 3,
                        MDef = 83,
                        MDefRate = 0.8
                    }
                },
                {
                    // Many videos has its initial values different but growth rate are the same.
                    // This is the source of the proper values and growth rate: https://youtu.be/Ov_t6CBeugE?t=291
                    // HighScepter growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 83(base lv1) +0.8 / Magi Atk 30(base lv1) +3 / Magi Def 83(base lv1) +0.8 / Blow 25(base lv1) +0
                    JobId.HighScepter,
                    new BaseStats() {
                        Lv = 1,
                        Atk = 30,
                        AtkRate = 3,
                        Def = 83,
                        DefRate = 0.8,
                        MAtk = 30,
                        MAtkRate = 3,
                        MDef = 83,
                        MDefRate = 0.8
                    }
                },
            };

        public ExpManager(IDatabase database, GameClientLookup gameClientLookup)
        {
            this._database = database;
            this._gameClientLookup = gameClientLookup;
        }

        protected readonly IDatabase _database;
        protected readonly GameClientLookup _gameClientLookup;

        
        public void AddExp(GameClient client, CharacterCommon characterToAddExpTo, uint gainedExp, uint extraBonusExp)
        {
            CDataCharacterJobData? activeCharacterJobData = characterToAddExpTo.ActiveCharacterJobData;
            if (activeCharacterJobData != null && activeCharacterJobData.Lv < ExpManager.LV_CAP)
            {
                // ------
                // EXP UP

                activeCharacterJobData.Exp += gainedExp;
                activeCharacterJobData.Exp += extraBonusExp;

                if(characterToAddExpTo is Character)
                {
                    S2CJobCharacterJobExpUpNtc expNtc = new S2CJobCharacterJobExpUpNtc();
                    expNtc.JobId = activeCharacterJobData.Job;
                    expNtc.AddExp = gainedExp;
                    expNtc.ExtraBonusExp = extraBonusExp;
                    expNtc.TotalExp = activeCharacterJobData.Exp;
                    expNtc.Type = 0; // TODO: Figure out
                    client.Send(expNtc);
                }
                else if(characterToAddExpTo is Pawn)
                {
                    S2CJobPawnJobExpUpNtc expNtc = new S2CJobPawnJobExpUpNtc();
                    expNtc.JobId = activeCharacterJobData.Job;
                    expNtc.AddExp = gainedExp;
                    expNtc.ExtraBonusExp = extraBonusExp;
                    expNtc.TotalExp = activeCharacterJobData.Exp;
                    expNtc.Type = 0; // TODO: Figure out
                    client.Send(expNtc);
                }


                // --------
                // LEVEL UP
                uint currentLevel = activeCharacterJobData.Lv;
                uint targetLevel = currentLevel;
                uint addJobPoint = 0;

                while (targetLevel < LV_CAP && activeCharacterJobData.Exp >= ExpManager.TotalExpToLevelUpTo(targetLevel + 1))
                {
                    targetLevel++;
                    addJobPoint+=LEVEL_UP_JOB_POINTS_EARNED[targetLevel];
                }

                if (currentLevel != targetLevel || addJobPoint != 0)
                {
                    activeCharacterJobData.Lv = targetLevel;
                    activeCharacterJobData.JobPoint += addJobPoint;

                    BaseStats baseStats = BASE_STATS_TABLE[activeCharacterJobData.Job];
                    activeCharacterJobData.Atk = (ushort)(baseStats.Atk + (activeCharacterJobData.Lv - 1) * baseStats.AtkRate);
                    activeCharacterJobData.Def = (ushort)(baseStats.Def + (activeCharacterJobData.Lv - 1) * baseStats.DefRate);
                    activeCharacterJobData.MAtk = (ushort)(baseStats.MAtk + (activeCharacterJobData.Lv - 1) * baseStats.MAtkRate);
                    activeCharacterJobData.MDef = (ushort)(baseStats.MDef + (activeCharacterJobData.Lv - 1) * baseStats.MDefRate);
    
                    // PERSIST CHANGES IN DB
                    this._database.UpdateCharacterJobData(characterToAddExpTo.CommonId, activeCharacterJobData);

                    if(characterToAddExpTo is Character)
                    {
                        // Inform client of lvl up
                        S2CJobCharacterJobLevelUpNtc lvlNtc = new S2CJobCharacterJobLevelUpNtc();
                        lvlNtc.Job = characterToAddExpTo.Job;
                        lvlNtc.Level = activeCharacterJobData.Lv;
                        lvlNtc.AddJobPoint = addJobPoint;
                        lvlNtc.TotalJobPoint = activeCharacterJobData.JobPoint;
                        GameStructure.CDataCharacterLevelParam(lvlNtc.CharacterLevelParam, (Character) characterToAddExpTo);
                        client.Send(lvlNtc);

                        // Inform other party members
                        S2CJobCharacterJobLevelUpMemberNtc lvlMemberNtc = new S2CJobCharacterJobLevelUpMemberNtc();
                        lvlMemberNtc.CharacterId = ((Character) characterToAddExpTo).CharacterId;
                        lvlMemberNtc.Job = characterToAddExpTo.Job;
                        lvlMemberNtc.Level = activeCharacterJobData.Lv;
                        GameStructure.CDataCharacterLevelParam(lvlMemberNtc.CharacterLevelParam, (Character) characterToAddExpTo);
                        client.Party.SendToAllExcept(lvlMemberNtc, client);
                    }
                    else if(characterToAddExpTo is Pawn)
                    {
                        // Inform client of lvl up
                        S2CJobPawnJobLevelUpNtc lvlNtc = new S2CJobPawnJobLevelUpNtc();
                        lvlNtc.PawnId = ((Pawn) characterToAddExpTo).PawnId;
                        lvlNtc.Job = characterToAddExpTo.Job;
                        lvlNtc.Level = activeCharacterJobData.Lv;
                        lvlNtc.AddJobPoint = addJobPoint;
                        lvlNtc.TotalJobPoint = activeCharacterJobData.JobPoint;
                        GameStructure.CDataCharacterLevelParam(lvlNtc.CharacterLevelParam, (Pawn) characterToAddExpTo);
                        client.Send(lvlNtc);

                        // Inform other party members
                        S2CJobPawnJobLevelUpMemberNtc lvlMemberNtc = new S2CJobPawnJobLevelUpMemberNtc();
                        lvlMemberNtc.CharacterId = ((Pawn) characterToAddExpTo).CharacterId;
                        lvlMemberNtc.PawnId = ((Pawn) characterToAddExpTo).PawnId;
                        lvlMemberNtc.Job = characterToAddExpTo.Job;
                        lvlMemberNtc.Level = activeCharacterJobData.Lv;
                        GameStructure.CDataCharacterLevelParam(lvlMemberNtc.CharacterLevelParam, (Pawn) characterToAddExpTo);
                        client.Party.SendToAllExcept(lvlMemberNtc, client);
                    }
                }
            }
        }

        public static uint TotalExpToLevelUpTo(uint level)
        {
            uint totalExp = 0;
            for (int i = 1; i < level; i++)
            {
                totalExp += EXP_UNTIL_NEXT_LV[i];
            }
            return totalExp;
        }
    }
}

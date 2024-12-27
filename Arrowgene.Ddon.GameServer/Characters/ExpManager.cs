#nullable enable
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Server.Scripting.interfaces;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

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

        public static readonly byte LV_CAP = 120;

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

        private static readonly uint[] BBM_EXP_UNTIL_NEXT_LV = new uint[]
        {
            /********/ 0,
            /* LV 1 */ 500,
            /* LV 2 */ 500,
            /* LV 3 */ 500,
            /* LV 4 */ 500,
            /* LV 5 */ 500,
            /* LV 6 */ 500,
            /* LV 7 */ 500,
            /* LV 8 */ 500,
            /* LV 9 */ 500,
            /* LV 10 */ 500,
            /* LV 11 */ 500,
            /* LV 12 */ 500,
            /* LV 13 */ 500,
            /* LV 14 */ 500,
            /* LV 15 */ 500,
            /* LV 16 */ 500,
            /* LV 17 */ 500,
            /* LV 18 */ 500,
            /* LV 19 */ 500,
            /* LV 20 */ 500,
            /* LV 21 */ 3000,
            /* LV 22 */ 3000,
            /* LV 23 */ 3000,
            /* LV 24 */ 3000,
            /* LV 25 */ 3000,
            /* LV 26 */ 3000,
            /* LV 27 */ 3000,
            /* LV 28 */ 3000,
            /* LV 29 */ 3000,
            /* LV 30 */ 3000,
            /* LV 31 */ 3000,
            /* LV 32 */ 3000,
            /* LV 33 */ 3000,
            /* LV 34 */ 3000,
            /* LV 35 */ 3000,
            /* LV 36 */ 3000,
            /* LV 37 */ 3000,
            /* LV 38 */ 3000,
            /* LV 39 */ 3000,
            /* LV 40 */ 5000,
            /* LV 41 */ 5000,
            /* LV 42 */ 5000,
            /* LV 43 */ 5000,
            /* LV 44 */ 5000,
            /* LV 45 */ 5000,
            /* LV 46 */ 5000,
            /* LV 47 */ 5000,
            /* LV 48 */ 5000,
            /* LV 49 */ 5000,
            /* LV 50 */ 8000,
            /* LV 51 */ 8000,
            /* LV 52 */ 8000,
            /* LV 53 */ 8000,
            /* LV 54 */ 8000,
            /* LV 55 */ 8000,
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

        public ExpManager(DdonGameServer server, GameClientLookup gameClientLookup)
        {
            this._Server = server;
            this._gameClientLookup = gameClientLookup;
            this._GameSettings = server.GameLogicSettings;
        }

        private DdonGameServer _Server;
        protected readonly GameClientLookup _gameClientLookup;
        private GameLogicSetting _GameSettings;

        private bool CalculateAndAssignStats(CharacterCommon Character)
        {
            if (Character == null || Character.ActiveCharacterJobData == null)
            {
                return false;
            }

            var ActiveCharacterJobData = Character.ActiveCharacterJobData;

            BaseStats baseStats = BASE_STATS_TABLE[ActiveCharacterJobData.Job];
            ActiveCharacterJobData.Atk = (ushort)(baseStats.Atk + (ActiveCharacterJobData.Lv - 1) * baseStats.AtkRate);
            ActiveCharacterJobData.Def = (ushort)(baseStats.Def + (ActiveCharacterJobData.Lv - 1) * baseStats.DefRate);
            ActiveCharacterJobData.MAtk = (ushort)(baseStats.MAtk + (ActiveCharacterJobData.Lv - 1) * baseStats.MAtkRate);
            ActiveCharacterJobData.MDef = (ushort)(baseStats.MDef + (ActiveCharacterJobData.Lv - 1) * baseStats.MDefRate);

            return true;
        }

        public static CDataCharacterJobData CalculateBaseStats(JobId jobId, uint Level)
        {
            CDataCharacterJobData JobData = new CDataCharacterJobData();

            BaseStats baseStats = BASE_STATS_TABLE[jobId];
            JobData.Atk = (ushort)(baseStats.Atk + (Level - 1) * baseStats.AtkRate);
            JobData.Def = (ushort)(baseStats.Def + (Level - 1) * baseStats.DefRate);
            JobData.MAtk = (ushort)(baseStats.MAtk + (Level - 1) * baseStats.MAtkRate);
            JobData.MDef = (ushort)(baseStats.MDef + (Level - 1) * baseStats.MDefRate);

            return JobData;
        }

        public static CDataCharacterJobData CalculateBaseStats(CharacterCommon Character)
        {
            CDataCharacterJobData JobData = new CDataCharacterJobData();
            if (Character == null || Character.ActiveCharacterJobData == null)
            {
                return JobData;
            }

            var ActiveCharacterJobData = Character.ActiveCharacterJobData;
            BaseStats baseStats = BASE_STATS_TABLE[ActiveCharacterJobData.Job];
            JobData.Atk = (ushort)(baseStats.Atk + (ActiveCharacterJobData.Lv - 1) * baseStats.AtkRate);
            JobData.Def = (ushort)(baseStats.Def + (ActiveCharacterJobData.Lv - 1) * baseStats.DefRate);
            JobData.MAtk = (ushort)(baseStats.MAtk + (ActiveCharacterJobData.Lv - 1) * baseStats.MAtkRate);
            JobData.MDef = (ushort)(baseStats.MDef + (ActiveCharacterJobData.Lv - 1) * baseStats.MDefRate);

            return JobData;
        }

        public PacketQueue AddExp(GameClient client, CharacterCommon characterToAddExpTo, uint gainedExp, RewardSource rewardType, QuestType questType = QuestType.All, DbConnection? connectionIn = null)
        {
            PacketQueue packets = new();

            var lvCap = (client.GameMode == GameMode.Normal) 
                ? _Server.GameLogicSettings.JobLevelMax
                : BitterblackMazeManager.LevelCap(client.Character.BbmProgress);

            CDataCharacterJobData? activeCharacterJobData = characterToAddExpTo.ActiveCharacterJobData;
            if (activeCharacterJobData != null && activeCharacterJobData.Lv < lvCap)
            {
                // ------
                // EXP UP

                uint extraBonusExp = CalculateExpBonus(characterToAddExpTo, gainedExp, rewardType, questType);

                activeCharacterJobData.Exp += gainedExp;
                activeCharacterJobData.Exp += extraBonusExp;

                if (characterToAddExpTo is Character)
                {
                    S2CJobCharacterJobExpUpNtc expNtc = new S2CJobCharacterJobExpUpNtc();
                    expNtc.JobId = activeCharacterJobData.Job;
                    expNtc.AddExp = gainedExp + extraBonusExp;
                    expNtc.ExtraBonusExp = extraBonusExp;
                    expNtc.TotalExp = activeCharacterJobData.Exp;
                    expNtc.Type = (byte) rewardType; // TODO: Figure out
                    client.Enqueue(expNtc, packets);
                }
                else if(characterToAddExpTo is Pawn)
                {
                    S2CJobPawnJobExpUpNtc expNtc = new S2CJobPawnJobExpUpNtc();
                    expNtc.PawnId = ((Pawn)characterToAddExpTo).PawnId;
                    expNtc.JobId = activeCharacterJobData.Job;
                    expNtc.AddExp = gainedExp + extraBonusExp;
                    expNtc.ExtraBonusExp = extraBonusExp;
                    expNtc.TotalExp = activeCharacterJobData.Exp;
                    expNtc.Type = (byte) rewardType; // TODO: Figure out
                    client.Enqueue(expNtc, packets);
                }


                // --------
                // LEVEL UP
                uint currentLevel = activeCharacterJobData.Lv;
                uint targetLevel = currentLevel;
                uint addJobPoint = 0;

                while (targetLevel < lvCap && activeCharacterJobData.Exp >= ExpManager.TotalExpToLevelUpTo(targetLevel + 1, client.GameMode))
                {
                    targetLevel++;

                    if (client.GameMode == GameMode.Normal)
                    {
                        addJobPoint += GetScaledPointAmount(client.GameMode, RewardSource.None, PointType.JobPoints, LEVEL_UP_JOB_POINTS_EARNED[targetLevel]);
                    }
                }

                if (currentLevel != targetLevel || addJobPoint != 0)
                {
                    activeCharacterJobData.Lv = targetLevel;
                    activeCharacterJobData.JobPoint += addJobPoint;

                    CalculateAndAssignStats(characterToAddExpTo);

                    if(characterToAddExpTo is Character)
                    {
                        // Inform client of lvl up
                        S2CJobCharacterJobLevelUpNtc lvlNtc = new S2CJobCharacterJobLevelUpNtc();
                        lvlNtc.Job = characterToAddExpTo.Job;
                        lvlNtc.Level = activeCharacterJobData.Lv;
                        lvlNtc.AddJobPoint = addJobPoint;
                        lvlNtc.TotalJobPoint = activeCharacterJobData.JobPoint;
                        GameStructure.CDataCharacterLevelParam(lvlNtc.CharacterLevelParam, (Character) characterToAddExpTo);
                        client.Enqueue(lvlNtc, packets);

                        // Inform other party members
                        S2CJobCharacterJobLevelUpMemberNtc lvlMemberNtc = new S2CJobCharacterJobLevelUpMemberNtc();
                        lvlMemberNtc.CharacterId = ((Character) characterToAddExpTo).CharacterId;
                        lvlMemberNtc.Job = characterToAddExpTo.Job;
                        lvlMemberNtc.Level = activeCharacterJobData.Lv;
                        GameStructure.CDataCharacterLevelParam(lvlMemberNtc.CharacterLevelParam, (Character) characterToAddExpTo);
                        client.Party.EnqueueToAllExcept(lvlMemberNtc, packets, client);
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
                        client.Enqueue(lvlNtc, packets);

                        // Inform other party members
                        S2CJobPawnJobLevelUpMemberNtc lvlMemberNtc = new S2CJobPawnJobLevelUpMemberNtc();
                        lvlMemberNtc.CharacterId = ((Pawn) characterToAddExpTo).CharacterId;
                        lvlMemberNtc.PawnId = ((Pawn) characterToAddExpTo).PawnId;
                        lvlMemberNtc.Job = characterToAddExpTo.Job;
                        lvlMemberNtc.Level = activeCharacterJobData.Lv;
                        GameStructure.CDataCharacterLevelParam(lvlMemberNtc.CharacterLevelParam, (Pawn) characterToAddExpTo);
                        client.Party.EnqueueToAllExcept(lvlMemberNtc, packets, client);
                    }
                }

                // PERSIST CHANGES IN DB
                _Server.Database.UpdateCharacterJobData(characterToAddExpTo.CommonId, activeCharacterJobData, connectionIn);
            }

            return packets;
        }

        public void AddExpNtc(GameClient client, CharacterCommon characterToAddExpTo, uint gainedExp, RewardSource rewardType, QuestType questType = QuestType.All, DbConnection? connectionIn = null)
        {
            AddExp(client, characterToAddExpTo, gainedExp, rewardType, questType, connectionIn).Send();
        }

        public PacketQueue AddJp(GameClient client, CharacterCommon characterToJpExpTo, uint gainedJp, RewardSource rewardType, QuestType questType = QuestType.All, DbConnection? connectionIn = null)
        {
            PacketQueue packets = new(); // Only ever has one packet, but has to exist because there are problems with returning interface types
            CDataCharacterJobData? activeCharacterJobData = characterToJpExpTo.ActiveCharacterJobData;
            activeCharacterJobData.JobPoint += gainedJp;

            if (characterToJpExpTo is Character)
            {
                S2CUpdateCharacterJobPointNtc jpNtc = new S2CUpdateCharacterJobPointNtc();
                jpNtc.Job = characterToJpExpTo.Job;
                jpNtc.AddJobPoint = gainedJp;
                jpNtc.ExtraBonusJobPoint = 0;
                jpNtc.TotalJobPoint = activeCharacterJobData.JobPoint;
                client.Enqueue(jpNtc, packets);
            }
            else
            {
                S2CJobPawnJobPointNtc jpNtc = new S2CJobPawnJobPointNtc();
                jpNtc.PawnId = ((Pawn)characterToJpExpTo).PawnId;
                jpNtc.Job = characterToJpExpTo.Job;
                jpNtc.AddJobPoint = gainedJp;
                jpNtc.ExtraBonusJobPoint = 0;
                jpNtc.TotalJobPoint = activeCharacterJobData.JobPoint;
                client.Enqueue(jpNtc, packets);
            }

            // PERSIST CHANGES IN DB
            _Server.Database.UpdateCharacterJobData(characterToJpExpTo.CommonId, activeCharacterJobData, connectionIn);

            return packets;
        }

        public void AddJpNtc(GameClient client, CharacterCommon characterToJpExpTo, uint gainedJp, RewardSource rewardType, QuestType questType = QuestType.All, DbConnection? connectionIn = null)
        {
            AddJp(client, characterToJpExpTo, gainedJp, rewardType, questType, connectionIn).Send();
        }

        public void ResetExpData(GameClient client, CharacterCommon characterCommon)
        {
            foreach (var jobData in client.Character.CharacterJobDataList)
            {
                if (jobData.Lv == 1 && jobData.Exp == 0)
                {
                    // We can skip sending this NTC since the job was never leveled
                    continue;
                }

                jobData.Lv = 1;
                jobData.JobPoint = 0;
                jobData.Exp = 0;

                CalculateAndAssignStats(characterCommon);

                if (characterCommon is Character)
                {
                    S2CJobCharacterJobExpUpNtc expNtc = new S2CJobCharacterJobExpUpNtc();
                    expNtc.JobId = jobData.Job;
                    expNtc.AddExp = 0;
                    expNtc.ExtraBonusExp = 0;
                    expNtc.TotalExp = jobData.Exp;
                    client.Send(expNtc);

                    // Inform client of lvl up
                    S2CJobCharacterJobLevelUpNtc lvlNtc = new S2CJobCharacterJobLevelUpNtc();
                    lvlNtc.Job = jobData.Job;
                    lvlNtc.Level = jobData.Lv;
                    lvlNtc.AddJobPoint = 0;
                    lvlNtc.TotalJobPoint = 0;
                    GameStructure.CDataCharacterLevelParam(lvlNtc.CharacterLevelParam, (Character)characterCommon);
                    client.Send(lvlNtc);

                    // Inform other party members
                    S2CJobCharacterJobLevelUpMemberNtc lvlMemberNtc = new S2CJobCharacterJobLevelUpMemberNtc();
                    lvlMemberNtc.CharacterId = ((Character)characterCommon).CharacterId;
                    lvlMemberNtc.Job = jobData.Job;
                    lvlMemberNtc.Level = jobData.Lv;
                    GameStructure.CDataCharacterLevelParam(lvlMemberNtc.CharacterLevelParam, (Character)characterCommon);
                    client.Party.SendToAllExcept(lvlMemberNtc, client);
                }
                else if (characterCommon is Pawn)
                {
                    S2CJobPawnJobExpUpNtc expNtc = new S2CJobPawnJobExpUpNtc();
                    expNtc.JobId = jobData.Job;
                    expNtc.AddExp = 0;
                    expNtc.ExtraBonusExp = 0;
                    expNtc.TotalExp = jobData.Exp;
                    client.Send(expNtc);

                    // Inform client of lvl up
                    S2CJobPawnJobLevelUpNtc lvlNtc = new S2CJobPawnJobLevelUpNtc();
                    lvlNtc.PawnId = ((Pawn)characterCommon).PawnId;
                    lvlNtc.Job = jobData.Job;
                    lvlNtc.Level = jobData.Lv;
                    lvlNtc.AddJobPoint = 0;
                    lvlNtc.TotalJobPoint = 0;
                    GameStructure.CDataCharacterLevelParam(lvlNtc.CharacterLevelParam, (Pawn)characterCommon);
                    client.Send(lvlNtc);

                    // Inform other party members
                    S2CJobPawnJobLevelUpMemberNtc lvlMemberNtc = new S2CJobPawnJobLevelUpMemberNtc();
                    lvlMemberNtc.CharacterId = ((Pawn)characterCommon).CharacterId;
                    lvlMemberNtc.PawnId = ((Pawn)characterCommon).PawnId;
                    lvlMemberNtc.Job = jobData.Job;
                    lvlMemberNtc.Level = jobData.Lv;
                    GameStructure.CDataCharacterLevelParam(lvlMemberNtc.CharacterLevelParam, (Pawn)characterCommon);
                    client.Party.SendToAllExcept(lvlMemberNtc, client);
                }

                _Server.Database.UpdateCharacterJobData(characterCommon.CommonId, jobData);
            }
        }

        public static uint TotalExpToLevelUpTo(uint level, GameMode gameMode)
        {
            var expTable = (gameMode == GameMode.Normal) ? EXP_UNTIL_NEXT_LV : BBM_EXP_UNTIL_NEXT_LV;

            uint totalExp = 0;
            for (int i = 1; i < level; i++)
            {
                totalExp += expTable[i];
            }
            return totalExp;
        }

        private uint GetRookiesRingBonus(CharacterCommon characterCommon, uint baseExpAmount)
        {
            if (!_Server.GameLogicSettings.Get<bool>("RookiesRing", "EnableRookiesRing"))
            {
                return 0;
            }

            if (!characterCommon.Equipment.GetItems(EquipType.Performance).Exists(x => x?.ItemId == (uint) ItemId.RookiesRingOfBlessing))
            {
                return 0;
            }

            var rookiesRingInterface = _Server.ScriptManager.GameItemModule.GetItemInterface(ItemId.RookiesRingOfBlessing);
            if (rookiesRingInterface == null)
            {
                return baseExpAmount;
            }

            return (uint)(baseExpAmount * rookiesRingInterface.GetBonusMultiplier(_Server, characterCommon));
        }

        private uint GetCourseExpBonus(CharacterCommon characterCommon, uint baseExpAmount, RewardSource source, QuestType questType)
        {
            double multiplier = 0;
            switch (source)
            {
                case RewardSource.Enemy:
                    multiplier = _Server.GpCourseManager.EnemyExpBonus(characterCommon);
                    break;
                case RewardSource.Quest:
                    multiplier = _Server.GpCourseManager.QuestExpBonus(questType);
                    break;
            }

            return (uint) (baseExpAmount * multiplier);
        }

        public uint CalculateExpBonus(CharacterCommon characterCommon, uint baseExpAmount, RewardSource source, QuestType questType)
        {
            uint bonusAmount = 0;

            bonusAmount += GetRookiesRingBonus(characterCommon, baseExpAmount);
            bonusAmount += GetCourseExpBonus(characterCommon, baseExpAmount, source, questType);

            return bonusAmount;
        }

        private uint PartyLevelRange(PartyGroup party)
        {
            var firstMember = party.Clients.First();
            uint maxLevel = firstMember.Character.ActiveCharacterJobData.Lv;
            uint minLevel = firstMember.Character.ActiveCharacterJobData.Lv;

            foreach (var member in party.Members)
            {
                CharacterCommon characterCommon = null;
                if (member is PlayerPartyMember)
                {
                    var client = ((PlayerPartyMember)member).Client;
                    characterCommon = client.Character;
                }
                else if (member is PawnPartyMember)
                {
                    characterCommon = ((PawnPartyMember)member).Pawn;
                }
                else
                {
                    // Is this possible?
                    continue;
                }

                maxLevel = Math.Max(maxLevel, characterCommon.ActiveCharacterJobData.Lv);
                minLevel = Math.Min(minLevel, characterCommon.ActiveCharacterJobData.Lv);
            }

            return maxLevel - minLevel;
        }

        private uint PartyMemberMaxLevel(PartyGroup party)
        {
            uint maxLevel = party.Clients.First().Character.ActiveCharacterJobData.Lv;
            foreach (var member in party.Members)
            {
                CharacterCommon characterCommon = null;
                if (member is PlayerPartyMember)
                {
                    var client = ((PlayerPartyMember)member).Client;
                    characterCommon = client.Character;
                }
                else if (member is PawnPartyMember)
                {
                    characterCommon = ((PawnPartyMember)member).Pawn;
                }
                else
                {
                    // Is this possible?
                    continue;
                }

                maxLevel = Math.Max(maxLevel, characterCommon.ActiveCharacterJobData.Lv);
            }

            return maxLevel;
        }

        private bool AllMembersOwnedBySameCharacter(PartyGroup party)
        {
            uint characterId = 0;
            foreach (var member in party.Members)
            {
                uint id = 0;
                if (member is PlayerPartyMember)
                {
                    var client = ((PlayerPartyMember)member).Client;
                    id = client.Character.CharacterId;
                }
                else if (member is PawnPartyMember)
                {
                    var pawn = ((PawnPartyMember)member).Pawn;
                    if (pawn.IsRented)
                    {
                        return false;
                    }

                    id = pawn.CharacterId;
                }

                if (characterId == 0)
                {
                    characterId = id;
                }

                if (characterId != id)
                {
                    return false;
                }
            }

            return true;
        }

        private double CalculatePartyRangeMultipler(GameMode gameMode, PartyGroup party)
        {
            if (!_GameSettings.EnableAdjustPartyEnemyExp || gameMode == GameMode.BitterblackMaze)
            {
                return 1.0;
            }

            if (_GameSettings.DisableExpCorrectionForMyPawn && AllMembersOwnedBySameCharacter(party))
            {
                return 1.0;
            }

            var range = PartyLevelRange(party);

            double multiplier = 0;
            foreach (var tier in _GameSettings.AdjustPartyEnemyExpTiers)
            {
                if (range >= tier.MinLv && range <= tier.MaxLv)
                {
                    multiplier = tier.ExpMultiplier;
                    break;
                }
            }

            return multiplier;
        }

        private double CalculateTargetLvMultiplier(GameMode gameMode, PartyGroup party, uint targetLv)
        {
            if (!_GameSettings.EnableAdjustTargetLvEnemyExp || gameMode == GameMode.BitterblackMaze)
            {
                return 1.0;
            }

            var maxLevelInParty = PartyMemberMaxLevel(party);
            if (maxLevelInParty <= targetLv)
            {
                return 1.0;
            }

            var range = maxLevelInParty - targetLv;

            double multiplier = 0;
            foreach (var tier in _GameSettings.AdjustTargetLvEnemyExpTiers)
            {
                if (range >= tier.MinLv && range <= tier.MaxLv)
                {
                    multiplier = tier.ExpMultiplier;
                    break;
                }
            }

            return multiplier;
        }

        public uint GetScaledPointAmount(GameMode gameMode, RewardSource source, PointType type, uint amount)
        {
            double modifier = 1.0;
            switch (type)
            {
                case PointType.ExperiencePoints:
                    if(gameMode == GameMode.Normal) {
                        modifier = (source == RewardSource.Enemy) ? _GameSettings.EnemyExpModifier : _GameSettings.QuestExpModifier;
                    }
                    //No quests in BBM, reward source should always be Enemy
                    else if(gameMode == GameMode.BitterblackMaze) {
                        modifier =  _GameSettings.BBMEnemyExpModifier;
                    }
                    break;
                case PointType.JobPoints:
                    modifier = _GameSettings.JpModifier;
                    break;
                case PointType.PlayPoints:
                    modifier = _GameSettings.PpModifier;
                    break;
                case ExpType.AreaPoints:
                    modifier = _GameSettings.ApModifier;
                    break;
                default:
                    modifier = 1.0;
                    break;
            }
            return (uint)(amount * modifier);
        }

        public uint GetAdjustedExp(GameMode gameMode, RewardSource source, PartyGroup party, uint baseExpAmount, uint targetLv)
        {
            if (_Server.GpCourseManager.DisablePartyExpAdjustment())
            {
                return baseExpAmount;
            }

            double multiplier = 1.0;
            if (source == RewardSource.Enemy)
            {
                var targetMultiplier = CalculateTargetLvMultiplier(gameMode, party, targetLv);
                var partyRangeMultiplier = CalculatePartyRangeMultipler(gameMode, party);
                multiplier = Math.Min(partyRangeMultiplier, targetMultiplier);
            }
            else if (source == RewardSource.Quest)
            {
                // Currently no adjustments
            }

            return (uint)(multiplier * baseExpAmount);
        }

        private uint GetMaxAllowedPartyRange()
        {
            if (_GameSettings.AdjustPartyEnemyExpTiers.Count == 0)
            {
                return 0;
            }

            uint min = _GameSettings.AdjustPartyEnemyExpTiers[0].MinLv;
            uint max = _GameSettings.AdjustPartyEnemyExpTiers[0].MaxLv;

            foreach (var tier in _GameSettings.AdjustPartyEnemyExpTiers)
            {
                min = Math.Min(min, tier.MinLv);
                max = Math.Max(max, tier.MaxLv);
            }

            return max - min;
        }

        public bool RequiresPawnCatchup(GameMode gameMode, PartyGroup party, Pawn pawn)
        {
            if (!_GameSettings.EnablePawnCatchup || gameMode == GameMode.BitterblackMaze)
            {
                return false;
            }

            foreach (var client in party.Clients)
            {
                if (pawn.CharacterId == client.Character.CharacterId)
                {
                    if (pawn.ActiveCharacterJobData.Lv >= client.Character.ActiveCharacterJobData.Lv)
                    {
                        return false;
                    }
                    else if (client.Character.ActiveCharacterJobData.Lv - pawn.ActiveCharacterJobData.Lv > _GameSettings.PawnCatchupLvDiff)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        private double CalculatePawnCatchupTargetLvMultiplier(GameMode gameMode, Pawn pawn, uint targetLv)
        {
            if (!_GameSettings.EnableAdjustTargetLvEnemyExp || gameMode == GameMode.BitterblackMaze)
            {
                return 1.0;
            }

            if (pawn.ActiveCharacterJobData.Lv <= targetLv)
            {
                return 1.0;
            }

            var range = pawn.ActiveCharacterJobData.Lv - targetLv;

            double multiplier = 0;
            foreach (var tier in _GameSettings.AdjustTargetLvEnemyExpTiers)
            {
                if (range >= tier.MinLv && range <= tier.MaxLv)
                {
                    multiplier = tier.ExpMultiplier;
                    break;
                }
            }

            return multiplier;
        }

        public uint GetAdjustedPawnExp(GameMode gameMode, RewardSource source, PartyGroup party, Pawn pawn, uint baseExpAmount, uint targetLv)
        {
            if (!_GameSettings.EnablePawnCatchup || gameMode == GameMode.BitterblackMaze)
            {
                return baseExpAmount;
            }

            var targetMultiplier = CalculatePawnCatchupTargetLvMultiplier(gameMode, pawn, targetLv);
            var multiplier = _GameSettings.PawnCatchupMultiplier * targetMultiplier;

            return (uint)(multiplier * baseExpAmount);
        }
    }
}

using System.Linq;
using System.Collections.Generic;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Characters
{
    struct JobLevelUp
    {
        public ushort Lv;
        public ushort Atk;
        public ushort Def;
        public ushort MAtk;
        public ushort MDef;
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

        private static readonly Dictionary<JobId, JobLevelUp[]> LEVEL_UP_TABLE = new Dictionary<JobId, JobLevelUp[]>()
            {
                {
                    // Fighter growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 120(base lv1) +1.2 / Magi Atk 20(base lv1) +2 / Magi Def 83(base lv1) +0.8 / Blow 25(base lv1) +0
                    JobId.Fighter,
                    new JobLevelUp[]
                    {
                        new JobLevelUp() {
                            Lv = 0,
                            Atk = 27,
                            Def = (ushort)118.8,
                            MAtk = 18,
                            MDef = (ushort)82.2,
                        },
                        new JobLevelUp() {
                            Lv = 1,
                            Atk = 30,
                            Def = 120,
                            MAtk = 20,
                            MDef = 83,
                        },
                         new JobLevelUp() {
                            Lv = 11,
                            Atk = 60,
                            Def = 132,
                            MAtk = 40,
                            MDef = 91,
                        },
                        new JobLevelUp() {
                            Lv = 21,
                            Atk = 90,
                            Def = 144,
                            MAtk = 60,
                            MDef = 99,
                        },
                        new JobLevelUp() {
                            Lv = 31,
                            Atk = 120,
                            Def = 156,
                            MAtk = 80,
                            MDef = 107,
                        },
                        new JobLevelUp() {
                            Lv = 41,
                            Atk = 150,
                            Def = 168,
                            MAtk = 100,
                            MDef = 115,
                        },
                        new JobLevelUp() {
                            Lv = 51,
                            Atk = 180,
                            Def = 180,
                            MAtk = 120,
                            MDef = 123,
                        },
                        new JobLevelUp() {
                            Lv = 61,
                            Atk = 210,
                            Def = 192,
                            MAtk = 140,
                            MDef = 131,
                        },
                        new JobLevelUp() {
                            Lv = 71,
                            Atk = 240,
                            Def = 204,
                            MAtk = 160,
                            MDef = 139,
                        },
                        new JobLevelUp() {
                            Lv = 81,
                            Atk = 270,
                            Def = 216,
                            MAtk = 180,
                            MDef = 147,
                        },
                        new JobLevelUp() {
                            Lv = 91,
                            Atk = 300,
                            Def = 228,
                            MAtk = 200,
                            MDef = 155,
                        },
                        new JobLevelUp() {
                            Lv = 101,
                            Atk = 330,
                            Def = 240,
                            MAtk = 220,
                            MDef = 163,
                        },
                        new JobLevelUp() {
                            Lv = 111,
                            Atk = 360,
                            Def = 252,
                            MAtk = 240,
                            MDef = 171,
                        },
                        new JobLevelUp() {
                            Lv = 121,
                            Atk = 390,
                            Def = 264,
                            MAtk = 260,
                            MDef = 179,
                        },
                        // The below values are not adjusted
                        new JobLevelUp() {
                            Lv = 129,
                            Atk = 430,
                            Def = 480,
                            MAtk = 200,
                            MDef = 110,
                        },
                        new JobLevelUp() {
                            Lv = 139,
                            Atk = 440,
                            Def = 510,
                            MAtk = 200,
                            MDef = 110,
                        },
                        new JobLevelUp() {
                            Lv = 149,
                            Atk = 450,
                            Def = 540,
                            MAtk = 200,
                            MDef = 110,
                        },
                        new JobLevelUp() {
                            Lv = 159,
                            Atk = 460,
                            Def = 570,
                            MAtk = 200,
                            MDef = 110,
                        },
                        new JobLevelUp() {
                            Lv = 169,
                            Atk = 470,
                            Def = 600,
                            MAtk = 200,
                            MDef = 110,
                        },
                        new JobLevelUp() {
                            Lv = 179,
                            Atk = 480,
                            Def = 630,
                            MAtk = 200,
                            MDef = 110,
                        },
                        new JobLevelUp() {
                            Lv = 189,
                            Atk = 490,
                            Def = 660,
                            MAtk = 200,
                            MDef = 110,
                        },
                        new JobLevelUp() {
                            Lv = 199,
                            Atk = 500,
                            Def = 690,
                            MAtk = 200,
                            MDef = 110,
                        },
                        new JobLevelUp() {
                            Lv = 999,
                            Atk = 500,
                            Def = 690,
                            MAtk = 200,
                            MDef = 110,
                        }
                    }
                },
                {
                    // Seeker growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 83(base lv1) +0.8 / Magi Atk 20(base lv1) +2 / Magi Def 83(base lv1) +0.8 / Blow 10(base lv1) +0
                    JobId.Seeker,
                    new JobLevelUp[]
                    {
                        new JobLevelUp() {
                            Lv = 0,
                            Atk = 27,
                            Def = (ushort)82.2,
                            MAtk = 18,
                            MDef = (ushort)82.2
                        },
                        new JobLevelUp() {
                            Lv = 1,
                            Atk = 30,
                            Def = 83,
                            MAtk = 20,
                            MDef = 83
                        },
                        new JobLevelUp() {
                            Lv = 11,
                            Atk = 60,
                            Def = 91,
                            MAtk = 40,
                            MDef = 91
                        },
                        new JobLevelUp() {
                            Lv = 21,
                            Atk = 90,
                            Def = 99,
                            MAtk = 60,
                            MDef = 99
                        },
                        new JobLevelUp() {
                            Lv = 31,
                            Atk = 120,
                            Def = 107,
                            MAtk = 80,
                            MDef = 107
                        },
                        new JobLevelUp() {
                            Lv = 41,
                            Atk = 150,
                            Def = 115,
                            MAtk = 100,
                            MDef = 115
                        },
                        new JobLevelUp() {
                            Lv = 51,
                            Atk = 180,
                            Def = 123,
                            MAtk = 120,
                            MDef = 123
                        },
                        new JobLevelUp() {
                            Lv = 61,
                            Atk = 210,
                            Def = 131,
                            MAtk = 140,
                            MDef = 131
                        },
                        new JobLevelUp() {
                            Lv = 71,
                            Atk = 240,
                            Def = 139,
                            MAtk = 160,
                            MDef = 139
                        },
                        new JobLevelUp() {
                            Lv = 81,
                            Atk = 270,
                            Def = 147,
                            MAtk = 180,
                            MDef = 147
                        },
                        new JobLevelUp() {
                            Lv = 91,
                            Atk = 300,
                            Def = 155,
                            MAtk = 200,
                            MDef = 155
                        },
                        new JobLevelUp() {
                            Lv = 101,
                            Atk = 330,
                            Def = 163,
                            MAtk = 220,
                            MDef = 163
                        },
                        new JobLevelUp() {
                            Lv = 111,
                            Atk = 360,
                            Def = 171,
                            MAtk = 240,
                            MDef = 171
                        },
                        new JobLevelUp() {
                            Lv = 121,
                            Atk = 390,
                            Def = 179,
                            MAtk = 260,
                            MDef = 179
                        },
                        // The below values are not adjusted
                        new JobLevelUp() {
                            Lv = 129,
                            Atk = 330,
                            Def = 330,
                            MAtk = 330,
                            MDef = 330
                        },
                        new JobLevelUp() {
                            Lv = 139,
                            Atk = 340,
                            Def = 340,
                            MAtk = 340,
                            MDef = 340
                        },
                        new JobLevelUp() {
                            Lv = 149,
                            Atk = 350,
                            Def = 350,
                            MAtk = 350,
                            MDef = 350
                        },
                        new JobLevelUp() {
                            Lv = 159,
                            Atk = 360,
                            Def = 360,
                            MAtk = 360,
                            MDef = 360
                        },
                        new JobLevelUp() {
                            Lv = 169,
                            Atk = 370,
                            Def = 370,
                            MAtk = 370,
                            MDef = 370
                        },
                        new JobLevelUp() {
                            Lv = 179,
                            Atk = 380,
                            Def = 380,
                            MAtk = 380,
                            MDef = 380
                        },
                        new JobLevelUp() {
                            Lv = 189,
                            Atk = 390,
                            Def = 390,
                            MAtk = 390,
                            MDef = 390
                        },
                        new JobLevelUp() {
                            Lv = 199,
                            Atk = 400,
                            Def = 400,
                            MAtk = 400,
                            MDef = 400
                        },
                        new JobLevelUp() {
                            Lv = 999,
                            Atk = 400,
                            Def = 400,
                            MAtk = 400,
                            MDef = 400
                        }
                    }
                },
                {
                    // Hunter growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 83(base lv1) +0.8 / Magi Atk 20(base lv1) +2 / Magi Def 83(base lv1) +0.8 / Blow 25(base lv1) +0
                    JobId.Hunter,
                    new JobLevelUp[]
                    {
                        new JobLevelUp() {
                    Lv = 0,
                    Atk = 27,
                    Def = (ushort)82.2,
                    MAtk = 18,
                    MDef = (ushort)82.2
                },
                new JobLevelUp() {
                    Lv = 1,
                    Atk = 30,
                    Def = 83,
                    MAtk = 20,
                    MDef = 83
                },
                new JobLevelUp() {
                    Lv = 11,
                    Atk = 60,
                    Def = 91,
                    MAtk = 40,
                    MDef = 91
                },
                new JobLevelUp() {
                    Lv = 21,
                    Atk = 90,
                    Def = 99,
                    MAtk = 60,
                    MDef = 99
                },
                new JobLevelUp() {
                    Lv = 31,
                    Atk = 120,
                    Def = 107,
                    MAtk = 80,
                    MDef = 107
                },
                new JobLevelUp() {
                    Lv = 41,
                    Atk = 150,
                    Def = 115,
                    MAtk = 100,
                    MDef = 115
                },
                new JobLevelUp() {
                    Lv = 51,
                    Atk = 180,
                    Def = 123,
                    MAtk = 120,
                    MDef = 123
                },
                new JobLevelUp() {
                    Lv = 61,
                    Atk = 210,
                    Def = 131,
                    MAtk = 140,
                    MDef = 131
                },
                new JobLevelUp() {
                    Lv = 71,
                    Atk = 240,
                    Def = 139,
                    MAtk = 160,
                    MDef = 139
                },
                new JobLevelUp() {
                    Lv = 81,
                    Atk = 270,
                    Def = 147,
                    MAtk = 180,
                    MDef = 147
                },
                new JobLevelUp() {
                    Lv = 91,
                    Atk = 300,
                    Def = 155,
                    MAtk = 200,
                    MDef = 155
                },
                new JobLevelUp() {
                    Lv = 101,
                    Atk = 330,
                    Def = 163,
                    MAtk = 220,
                    MDef = 163
                },
                new JobLevelUp() {
                    Lv = 111,
                    Atk = 360,
                    Def = 171,
                    MAtk = 240,
                    MDef = 171
                },
                new JobLevelUp() {
                    Lv = 121,
                    Atk = 390,
                    Def = 179,
                    MAtk = 260,
                    MDef = 179
                },
                // The below values are not adjusted
                new JobLevelUp() {
                    Lv = 129,
                    Atk = 450,
                    Def = 240,
                    MAtk = 300,
                    MDef = 230
                },
                new JobLevelUp() {
                    Lv = 139,
                    Atk = 470,
                    Def = 250,
                    MAtk = 300,
                    MDef = 240
                },
                new JobLevelUp() {
                    Lv = 149,
                    Atk = 490,
                    Def = 260,
                    MAtk = 300,
                    MDef = 250
                },
                new JobLevelUp() {
                    Lv = 159,
                    Atk = 510,
                    Def = 270,
                    MAtk = 300,
                    MDef = 260
                },
                new JobLevelUp() {
                    Lv = 169,
                    Atk = 530,
                    Def = 280,
                    MAtk = 300,
                    MDef = 270
                },
                new JobLevelUp() {
                    Lv = 179,
                    Atk = 550,
                    Def = 290,
                    MAtk = 300,
                    MDef = 280
                },
                new JobLevelUp() {
                    Lv = 189,
                    Atk = 570,
                    Def = 300,
                    MAtk = 300,
                    MDef = 290
                },
                new JobLevelUp() {
                    Lv = 199,
                    Atk = 590,
                    Def = 310,
                    MAtk = 300,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 999,
                    Atk = 590,
                    Def = 310,
                    MAtk = 300,
                    MDef = 300
                }
                    }
                },
                {
                    // Priest growth per lv: Phys Atk 20(base lv1) +2 / Phys Def 58(base lv1) +0.4 / Magi Atk 30(base lv1) +3 / Magi Def 100(base lv1) +1 / Blow 18(base lv1) +0
                    JobId.Priest,
                    new JobLevelUp[]
                    {
                        new JobLevelUp() {
                    Lv = 0,
                    Atk = 18,
                    Def = (ushort)57.6,
                    MAtk = 27,
                    MDef = 99
                },
                new JobLevelUp() {
                    Lv = 1,
                    Atk = 20,
                    Def = 58,
                    MAtk = 30,
                    MDef = 100
                },
                new JobLevelUp() {
                    Lv = 11,
                    Atk = 40,
                    Def = 62,
                    MAtk = 60,
                    MDef = 110
                },
                new JobLevelUp() {
                    Lv = 21,
                    Atk = 60,
                    Def = 66,
                    MAtk = 90,
                    MDef = 120
                },
                new JobLevelUp() {
                    Lv = 31,
                    Atk = 80,
                    Def = 70,
                    MAtk = 120,
                    MDef = 130
                },
                new JobLevelUp() {
                    Lv = 41,
                    Atk = 100,
                    Def = 74,
                    MAtk = 150,
                    MDef = 140
                },
                new JobLevelUp() {
                    Lv = 51,
                    Atk = 120,
                    Def = 78,
                    MAtk = 180,
                    MDef = 150
                },
                new JobLevelUp() {
                    Lv = 61,
                    Atk = 140,
                    Def = 82,
                    MAtk = 210,
                    MDef = 160
                },
                new JobLevelUp() {
                    Lv = 71,
                    Atk = 160,
                    Def = 86,
                    MAtk = 240,
                    MDef = 170
                },
                new JobLevelUp() {
                    Lv = 81,
                    Atk = 180,
                    Def = 90,
                    MAtk = 270,
                    MDef = 180
                },
                new JobLevelUp() {
                    Lv = 91,
                    Atk = 200,
                    Def = 94,
                    MAtk = 300,
                    MDef = 190
                },
                new JobLevelUp() {
                    Lv = 101,
                    Atk = 220,
                    Def = 98,
                    MAtk = 330,
                    MDef = 200
                },
                new JobLevelUp() {
                    Lv = 111,
                    Atk = 240,
                    Def = 102,
                    MAtk = 360,
                    MDef = 210
                },
                new JobLevelUp() {
                    Lv = 121,
                    Atk = 260,
                    Def = 106,
                    MAtk = 390,
                    MDef = 220
                },
                // The below values are not adjusted
                new JobLevelUp() {
                    Lv = 129,
                    Atk = 200,
                    Def = 120,
                    MAtk = 460,
                    MDef = 450
                },
                new JobLevelUp() {
                    Lv = 139,
                    Atk = 200,
                    Def = 120,
                    MAtk = 480,
                    MDef = 470
                },
                new JobLevelUp() {
                    Lv = 149,
                    Atk = 200,
                    Def = 120,
                    MAtk = 500,
                    MDef = 490
                },
                new JobLevelUp() {
                    Lv = 159,
                    Atk = 200,
                    Def = 120,
                    MAtk = 520,
                    MDef = 510
                },
                new JobLevelUp() {
                    Lv = 169,
                    Atk = 200,
                    Def = 120,
                    MAtk = 540,
                    MDef = 530
                },
                new JobLevelUp() {
                    Lv = 179,
                    Atk = 200,
                    Def = 120,
                    MAtk = 560,
                    MDef = 550
                },
                new JobLevelUp() {
                    Lv = 189,
                    Atk = 200,
                    Def = 120,
                    MAtk = 580,
                    MDef = 570
                },
                new JobLevelUp() {
                    Lv = 199,
                    Atk = 200,
                    Def = 120,
                    MAtk = 600,
                    MDef = 590
                },
                new JobLevelUp() {
                    Lv = 999,
                    Atk = 200,
                    Def = 120,
                    MAtk = 600,
                    MDef = 590
                }
                    }
                },
                {
                    // ShieldSage growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 120(base lv1) +1.2 / Magi Atk 30(base lv1) +3 / Magi Def 120(base lv1) +1.2 / Blow 30(base lv1) +0
                    JobId.ShieldSage,
                    new JobLevelUp[]
                    {
                        new JobLevelUp() {
                    Lv = 0,
                    Atk = 27,
                    Def = (ushort)118.8,
                    MAtk = 27,
                    MDef = (ushort)118.8
                },
                new JobLevelUp() {
                    Lv = 1,
                    Atk = 30,
                    Def = 120,
                    MAtk = 30,
                    MDef = 120
                },
                new JobLevelUp() {
                    Lv = 11,
                    Atk = 60,
                    Def = 132,
                    MAtk = 60,
                    MDef = 132
                },
                new JobLevelUp() {
                    Lv = 21,
                    Atk = 90,
                    Def = 144,
                    MAtk = 90,
                    MDef = 144
                },
                new JobLevelUp() {
                    Lv = 31,
                    Atk = 120,
                    Def = 156,
                    MAtk = 120,
                    MDef = 156
                },
                new JobLevelUp() {
                    Lv = 41,
                    Atk = 150,
                    Def = 168,
                    MAtk = 150,
                    MDef = 168
                },
                new JobLevelUp() {
                    Lv = 51,
                    Atk = 180,
                    Def = 180,
                    MAtk = 180,
                    MDef = 180
                },
                new JobLevelUp() {
                    Lv = 61,
                    Atk = 210,
                    Def = 192,
                    MAtk = 210,
                    MDef = 192
                },
                new JobLevelUp() {
                    Lv = 71,
                    Atk = 240,
                    Def = 204,
                    MAtk = 240,
                    MDef = 204
                },
                new JobLevelUp() {
                    Lv = 81,
                    Atk = 270,
                    Def = 216,
                    MAtk = 270,
                    MDef = 216
                },
                new JobLevelUp() {
                    Lv = 91,
                    Atk = 300,
                    Def = 228,
                    MAtk = 300,
                    MDef = 228
                },
                new JobLevelUp() {
                    Lv = 101,
                    Atk = 330,
                    Def = 240,
                    MAtk = 330,
                    MDef = 240
                },
                new JobLevelUp() {
                    Lv = 111,
                    Atk = 360,
                    Def = 252,
                    MAtk = 360,
                    MDef = 252
                },
                new JobLevelUp() {
                    Lv = 121,
                    Atk = 390,
                    Def = 264,
                    MAtk = 390,
                    MDef = 264
                },
                // The below values are not adjusted
                new JobLevelUp() {
                    Lv = 129,
                    Atk = 110,
                    Def = 330,
                    MAtk = 230,
                    MDef = 550
                },
                new JobLevelUp() {
                    Lv = 139,
                    Atk = 110,
                    Def = 340,
                    MAtk = 240,
                    MDef = 570
                },
                new JobLevelUp() {
                    Lv = 149,
                    Atk = 110,
                    Def = 350,
                    MAtk = 250,
                    MDef = 590
                },
                new JobLevelUp() {
                    Lv = 159,
                    Atk = 110,
                    Def = 360,
                    MAtk = 260,
                    MDef = 610
                },
                new JobLevelUp() {
                    Lv = 169,
                    Atk = 110,
                    Def = 370,
                    MAtk = 270,
                    MDef = 630
                },
                new JobLevelUp() {
                    Lv = 179,
                    Atk = 110,
                    Def = 380,
                    MAtk = 280,
                    MDef = 650
                },
                new JobLevelUp() {
                    Lv = 189,
                    Atk = 110,
                    Def = 390,
                    MAtk = 290,
                    MDef = 670
                },
                new JobLevelUp() {
                    Lv = 199,
                    Atk = 110,
                    Def = 400,
                    MAtk = 300,
                    MDef = 690
                },
                new JobLevelUp() {
                    Lv = 999,
                    Atk = 110,
                    Def = 400,
                    MAtk = 300,
                    MDef = 690
                }
                    }
                },
                {
                    // Sorcerer growth per lv: Phys Atk 20(base lv1) +2 / Phys Def 58(base lv1) +0.4 / Magi Atk 30(base lv1) +3 / Magi Def 100(base lv1) +1 / Blow 30(base lv1) +0
                    JobId.Sorcerer,
                    new JobLevelUp[]
                    {
                        new JobLevelUp() {
                    Lv = 0,
                    Atk = 18,
                    Def = (ushort)57.6,
                    MAtk = 27,
                    MDef = 99
                },
                new JobLevelUp() {
                    Lv = 1,
                    Atk = 20,
                    Def = 58,
                    MAtk = 30,
                    MDef = 100
                },
                new JobLevelUp() {
                    Lv = 11,
                    Atk = 40,
                    Def = 62,
                    MAtk = 60,
                    MDef = 110
                },
                new JobLevelUp() {
                    Lv = 21,
                    Atk = 60,
                    Def = 66,
                    MAtk = 90,
                    MDef = 120
                },
                new JobLevelUp() {
                    Lv = 31,
                    Atk = 80,
                    Def = 70,
                    MAtk = 120,
                    MDef = 130
                },
                new JobLevelUp() {
                    Lv = 41,
                    Atk = 100,
                    Def = 74,
                    MAtk = 150,
                    MDef = 140
                },
                new JobLevelUp() {
                    Lv = 51,
                    Atk = 120,
                    Def = 78,
                    MAtk = 180,
                    MDef = 150
                },
                new JobLevelUp() {
                    Lv = 61,
                    Atk = 140,
                    Def = 82,
                    MAtk = 210,
                    MDef = 160
                },
                new JobLevelUp() {
                    Lv = 71,
                    Atk = 160,
                    Def = 86,
                    MAtk = 240,
                    MDef = 170
                },
                new JobLevelUp() {
                    Lv = 81,
                    Atk = 180,
                    Def = 90,
                    MAtk = 270,
                    MDef = 180
                },
                new JobLevelUp() {
                    Lv = 91,
                    Atk = 200,
                    Def = 94,
                    MAtk = 300,
                    MDef = 190
                },
                new JobLevelUp() {
                    Lv = 101,
                    Atk = 220,
                    Def = 98,
                    MAtk = 330,
                    MDef = 200
                },
                new JobLevelUp() {
                    Lv = 111,
                    Atk = 240,
                    Def = 102,
                    MAtk = 360,
                    MDef = 210
                },
                new JobLevelUp() {
                    Lv = 121,
                    Atk = 260,
                    Def = 106,
                    MAtk = 390,
                    MDef = 220
                },
                // The below values are not adjusted
                new JobLevelUp() {
                    Lv = 129,
                    Atk = 200,
                    Def = 120,
                    MAtk = 580,
                    MDef = 510
                },
                new JobLevelUp() {
                    Lv = 139,
                    Atk = 200,
                    Def = 120,
                    MAtk = 610,
                    MDef = 520
                },
                new JobLevelUp() {
                    Lv = 149,
                    Atk = 200,
                    Def = 120,
                    MAtk = 640,
                    MDef = 530
                },
                new JobLevelUp() {
                    Lv = 159,
                    Atk = 200,
                    Def = 120,
                    MAtk = 670,
                    MDef = 540
                },
                new JobLevelUp() {
                    Lv = 169,
                    Atk = 200,
                    Def = 120,
                    MAtk = 700,
                    MDef = 550
                },
                new JobLevelUp() {
                    Lv = 179,
                    Atk = 200,
                    Def = 120,
                    MAtk = 730,
                    MDef = 560
                },
                new JobLevelUp() {
                    Lv = 189,
                    Atk = 200,
                    Def = 120,
                    MAtk = 760,
                    MDef = 570
                },
                new JobLevelUp() {
                    Lv = 199,
                    Atk = 200,
                    Def = 120,
                    MAtk = 790,
                    MDef = 580
                },
                new JobLevelUp() {
                    Lv = 999,
                    Atk = 200,
                    Def = 120,
                    MAtk = 790,
                    MDef = 580
                }
                    }
                },
                {
                    // Warrior growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 120(base lv1) +1.2 / Magi Atk 20(base lv1) +2 / Magi Def 83(base lv1) +0.8 / Blow 35(base lv1) +0
                    JobId.Warrior,
                    new JobLevelUp[]
                    {
                        new JobLevelUp() {
                    Lv = 0,
                    Atk = 27,
                    Def = (ushort)118.8,
                    MAtk = 18,
                    MDef = (ushort)82.2
                },
                new JobLevelUp() {
                    Lv = 1,
                    Atk = 30,
                    Def = 120,
                    MAtk = 20,
                    MDef = 83
                },
                new JobLevelUp() {
                    Lv = 11,
                    Atk = 60,
                    Def = 132,
                    MAtk = 40,
                    MDef = 91
                },
                new JobLevelUp() {
                    Lv = 21,
                    Atk = 90,
                    Def = 144,
                    MAtk = 60,
                    MDef = 99
                },
                new JobLevelUp() {
                    Lv = 31,
                    Atk = 120,
                    Def = 156,
                    MAtk = 80,
                    MDef = 107
                },
                new JobLevelUp() {
                    Lv = 41,
                    Atk = 150,
                    Def = 168,
                    MAtk = 100,
                    MDef = 115
                },
                new JobLevelUp() {
                    Lv = 51,
                    Atk = 180,
                    Def = 180,
                    MAtk = 120,
                    MDef = 123
                },
                new JobLevelUp() {
                    Lv = 61,
                    Atk = 210,
                    Def = 192,
                    MAtk = 140,
                    MDef = 131
                },
                new JobLevelUp() {
                    Lv = 71,
                    Atk = 240,
                    Def = 204,
                    MAtk = 160,
                    MDef = 139
                },
                new JobLevelUp() {
                    Lv = 81,
                    Atk = 270,
                    Def = 216,
                    MAtk = 180,
                    MDef = 147
                },
                new JobLevelUp() {
                    Lv = 91,
                    Atk = 300,
                    Def = 228,
                    MAtk = 200,
                    MDef = 155
                },
                new JobLevelUp() {
                    Lv = 101,
                    Atk = 330,
                    Def = 240,
                    MAtk = 220,
                    MDef = 163
                },
                new JobLevelUp() {
                    Lv = 111,
                    Atk = 360,
                    Def = 252,
                    MAtk = 240,
                    MDef = 171
                },
                new JobLevelUp() {
                    Lv = 121,
                    Atk = 390,
                    Def = 264,
                    MAtk = 260,
                    MDef = 179
                },
                // The below values are not adjusted
                new JobLevelUp() {
                    Lv = 129,
                    Atk = 550,
                    Def = 360,
                    MAtk = 200,
                    MDef = 110
                },
                new JobLevelUp() {
                    Lv = 139,
                    Atk = 570,
                    Def = 380,
                    MAtk = 200,
                    MDef = 110
                },
                new JobLevelUp() {
                    Lv = 149,
                    Atk = 590,
                    Def = 400,
                    MAtk = 200,
                    MDef = 110
                },
                new JobLevelUp() {
                    Lv = 159,
                    Atk = 610,
                    Def = 420,
                    MAtk = 200,
                    MDef = 110
                },
                new JobLevelUp() {
                    Lv = 169,
                    Atk = 630,
                    Def = 440,
                    MAtk = 200,
                    MDef = 110
                },
                new JobLevelUp() {
                    Lv = 179,
                    Atk = 650,
                    Def = 460,
                    MAtk = 200,
                    MDef = 110
                },
                new JobLevelUp() {
                    Lv = 189,
                    Atk = 670,
                    Def = 480,
                    MAtk = 200,
                    MDef = 110
                },
                new JobLevelUp() {
                    Lv = 199,
                    Atk = 690,
                    Def = 500,
                    MAtk = 200,
                    MDef = 110
                },
                new JobLevelUp() {
                    Lv = 999,
                    Atk = 690,
                    Def = 500,
                    MAtk = 200,
                    MDef = 110
                }
                    }
                },
                {
                    // ElementArcher growth per lv: Phys Atk 20(base lv1) +2 / Phys Def 58(base lv1) +0.4 / Magi Atk 30(base lv1) +3 / Magi Def 100(base lv1) +1 / Blow 18(base lv1) +0
                    JobId.ElementArcher,
                    new JobLevelUp[]
                    {
                        new JobLevelUp() {
                    Lv = 0,
                    Atk = 18,
                    Def = (ushort)57.6,
                    MAtk = 27,
                    MDef = 99
                },
                new JobLevelUp() {
                    Lv = 1,
                    Atk = 20,
                    Def = 58,
                    MAtk = 30,
                    MDef = 100
                },
                new JobLevelUp() {
                    Lv = 11,
                    Atk = 40,
                    Def = 62,
                    MAtk = 60,
                    MDef = 110
                },
                new JobLevelUp() {
                    Lv = 21,
                    Atk = 60,
                    Def = 66,
                    MAtk = 90,
                    MDef = 120
                },
                new JobLevelUp() {
                    Lv = 31,
                    Atk = 80,
                    Def = 70,
                    MAtk = 120,
                    MDef = 130
                },
                new JobLevelUp() {
                    Lv = 41,
                    Atk = 100,
                    Def = 74,
                    MAtk = 150,
                    MDef = 140
                },
                new JobLevelUp() {
                    Lv = 51,
                    Atk = 120,
                    Def = 78,
                    MAtk = 180,
                    MDef = 150
                },
                new JobLevelUp() {
                    Lv = 61,
                    Atk = 140,
                    Def = 82,
                    MAtk = 210,
                    MDef = 160
                },
                new JobLevelUp() {
                    Lv = 71,
                    Atk = 160,
                    Def = 86,
                    MAtk = 240,
                    MDef = 170
                },
                new JobLevelUp() {
                    Lv = 81,
                    Atk = 180,
                    Def = 90,
                    MAtk = 270,
                    MDef = 180
                },
                new JobLevelUp() {
                    Lv = 91,
                    Atk = 200,
                    Def = 94,
                    MAtk = 300,
                    MDef = 190
                },
                new JobLevelUp() {
                    Lv = 101,
                    Atk = 220,
                    Def = 98,
                    MAtk = 330,
                    MDef = 200
                },
                new JobLevelUp() {
                    Lv = 111,
                    Atk = 240,
                    Def = 102,
                    MAtk = 360,
                    MDef = 210
                },
                new JobLevelUp() {
                    Lv = 121,
                    Atk = 260,
                    Def = 106,
                    MAtk = 390,
                    MDef = 220
                },
                // The below values are not adjusted
                new JobLevelUp() {
                    Lv = 129,
                    Atk = 230,
                    Def = 300,
                    MAtk = 340,
                    MDef = 450
                },
                new JobLevelUp() {
                    Lv = 139,
                    Atk = 240,
                    Def = 300,
                    MAtk = 350,
                    MDef = 470
                },
                new JobLevelUp() {
                    Lv = 149,
                    Atk = 250,
                    Def = 300,
                    MAtk = 360,
                    MDef = 490
                },
                new JobLevelUp() {
                    Lv = 159,
                    Atk = 260,
                    Def = 300,
                    MAtk = 370,
                    MDef = 510
                },
                new JobLevelUp() {
                    Lv = 169,
                    Atk = 270,
                    Def = 300,
                    MAtk = 380,
                    MDef = 530
                },
                new JobLevelUp() {
                    Lv = 179,
                    Atk = 280,
                    Def = 300,
                    MAtk = 390,
                    MDef = 550
                },
                new JobLevelUp() {
                    Lv = 189,
                    Atk = 290,
                    Def = 300,
                    MAtk = 400,
                    MDef = 570
                },
                new JobLevelUp() {
                    Lv = 199,
                    Atk = 300,
                    Def = 300,
                    MAtk = 410,
                    MDef = 590
                },
                new JobLevelUp() {
                    Lv = 999,
                    Atk = 300,
                    Def = 300,
                    MAtk = 410,
                    MDef = 590
                }
                    }
                },
                {
                    // Alchemist growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 100(base lv1) +1 / Magi Atk 20(base lv1) +2 / Magi Def 100(base lv1) +1 / Blow 25(base lv1) +0
                    JobId.Alchemist,
                    new JobLevelUp[]
                    {
                        new JobLevelUp() {
                    Lv = 0,
                    Atk = 27,
                    Def = 99,
                    MAtk = 18,
                    MDef = 99
                },
                new JobLevelUp() {
                    Lv = 1,
                    Atk = 30,
                    Def = 100,
                    MAtk = 20,
                    MDef = 100
                },
                new JobLevelUp() {
                    Lv = 11,
                    Atk = 60,
                    Def = 110,
                    MAtk = 40,
                    MDef = 110
                },
                new JobLevelUp() {
                    Lv = 21,
                    Atk = 90,
                    Def = 120,
                    MAtk = 60,
                    MDef = 120
                },
                new JobLevelUp() {
                    Lv = 31,
                    Atk = 120,
                    Def = 130,
                    MAtk = 80,
                    MDef = 130
                },
                new JobLevelUp() {
                    Lv = 41,
                    Atk = 150,
                    Def = 140,
                    MAtk = 100,
                    MDef = 140
                },
                new JobLevelUp() {
                    Lv = 51,
                    Atk = 180,
                    Def = 150,
                    MAtk = 120,
                    MDef = 150
                },
                new JobLevelUp() {
                    Lv = 61,
                    Atk = 210,
                    Def = 160,
                    MAtk = 140,
                    MDef = 160
                },
                new JobLevelUp() {
                    Lv = 71,
                    Atk = 240,
                    Def = 170,
                    MAtk = 160,
                    MDef = 170
                },
                new JobLevelUp() {
                    Lv = 81,
                    Atk = 270,
                    Def = 180,
                    MAtk = 180,
                    MDef = 180
                },
                new JobLevelUp() {
                    Lv = 91,
                    Atk = 300,
                    Def = 190,
                    MAtk = 200,
                    MDef = 190
                },
                new JobLevelUp() {
                    Lv = 101,
                    Atk = 330,
                    Def = 200,
                    MAtk = 220,
                    MDef = 200
                },
                new JobLevelUp() {
                    Lv = 111,
                    Atk = 360,
                    Def = 210,
                    MAtk = 240,
                    MDef = 210
                },
                new JobLevelUp() {
                    Lv = 121,
                    Atk = 390,
                    Def = 220,
                    MAtk = 260,
                    MDef = 220
                },
                // The below values are not adjusted
                new JobLevelUp() {
                    Lv = 129,
                    Atk = 230,
                    Def = 550,
                    MAtk = 140,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 139,
                    Atk = 240,
                    Def = 570,
                    MAtk = 150,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 149,
                    Atk = 250,
                    Def = 590,
                    MAtk = 160,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 159,
                    Atk = 260,
                    Def = 610,
                    MAtk = 170,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 169,
                    Atk = 270,
                    Def = 630,
                    MAtk = 180,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 179,
                    Atk = 280,
                    Def = 650,
                    MAtk = 190,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 189,
                    Atk = 290,
                    Def = 670,
                    MAtk = 200,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 199,
                    Atk = 300,
                    Def = 690,
                    MAtk = 210,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 999,
                    Atk = 300,
                    Def = 690,
                    MAtk = 210,
                    MDef = 300
                }
                    }
                },
                {
                    // Status confirmed
                    // SpiritLancer growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 83(base lv1) +0.8 / Magi Atk 30(base lv1) +3 / Magi Def 83(base lv1) +0.8 / Blow 25(base lv1) +0
                    JobId.SpiritLancer,
                    new JobLevelUp[]
                    {
                        new JobLevelUp() {
                    Lv = 0,
                    Atk = 27,
                    Def = (ushort)82.2,
                    MAtk = 27,
                    MDef = (ushort)82.2
                },
                new JobLevelUp() {
                    Lv = 1,
                    Atk = 30,
                    Def = 83,
                    MAtk = 30,
                    MDef = 83
                },
                new JobLevelUp() {
                    Lv = 11,
                    Atk = 60,
                    Def = 91,
                    MAtk = 60,
                    MDef = 91
                },
                new JobLevelUp() {
                    Lv = 21,
                    Atk = 90,
                    Def = 99,
                    MAtk = 90,
                    MDef = 99
                },
                new JobLevelUp() {
                    Lv = 31,
                    Atk = 120,
                    Def = 107,
                    MAtk = 120,
                    MDef = 107
                },
                new JobLevelUp() {
                    Lv = 41,
                    Atk = 150,
                    Def = 115,
                    MAtk = 150,
                    MDef = 115
                },
                new JobLevelUp() {
                    Lv = 51,
                    Atk = 180,
                    Def = 123,
                    MAtk = 180,
                    MDef = 123
                },
                new JobLevelUp() {
                    Lv = 61,
                    Atk = 210,
                    Def = 131,
                    MAtk = 210,
                    MDef = 131
                },
                new JobLevelUp() {
                    Lv = 71,
                    Atk = 240,
                    Def = 139,
                    MAtk = 240,
                    MDef = 139
                },
                new JobLevelUp() {
                    Lv = 81,
                    Atk = 270,
                    Def = 147,
                    MAtk = 270,
                    MDef = 147
                },
                new JobLevelUp() {
                    Lv = 91,
                    Atk = 300,
                    Def = 155,
                    MAtk = 300,
                    MDef = 155
                },
                new JobLevelUp() {
                    Lv = 101,
                    Atk = 330,
                    Def = 163,
                    MAtk = 330,
                    MDef = 163
                },
                new JobLevelUp() {
                    Lv = 111,
                    Atk = 360,
                    Def = 171,
                    MAtk = 360,
                    MDef = 171
                },
                new JobLevelUp() {
                    Lv = 121,
                    Atk = 390,
                    Def = 179,
                    MAtk = 390,
                    MDef = 179
                },
                // The below values are not adjusted
                new JobLevelUp() {
                    Lv = 129,
                    Atk = 230,
                    Def = 300,
                    MAtk = 340,
                    MDef = 450
                },
                new JobLevelUp() {
                    Lv = 139,
                    Atk = 240,
                    Def = 300,
                    MAtk = 350,
                    MDef = 470
                },
                new JobLevelUp() {
                    Lv = 149,
                    Atk = 250,
                    Def = 300,
                    MAtk = 360,
                    MDef = 490
                },
                new JobLevelUp() {
                    Lv = 159,
                    Atk = 260,
                    Def = 300,
                    MAtk = 370,
                    MDef = 510
                },
                new JobLevelUp() {
                    Lv = 169,
                    Atk = 270,
                    Def = 300,
                    MAtk = 380,
                    MDef = 530
                },
                new JobLevelUp() {
                    Lv = 179,
                    Atk = 280,
                    Def = 300,
                    MAtk = 390,
                    MDef = 550
                },
                new JobLevelUp() {
                    Lv = 189,
                    Atk = 290,
                    Def = 300,
                    MAtk = 400,
                    MDef = 570
                },
                new JobLevelUp() {
                    Lv = 199,
                    Atk = 300,
                    Def = 300,
                    MAtk = 410,
                    MDef = 590
                },
                new JobLevelUp() {
                    Lv = 999,
                    Atk = 300,
                    Def = 300,
                    MAtk = 410,
                    MDef = 590
                }
                    }
                },
                // HS have placeholder level up values.
                // TODO: Find out where the correct stats for these two last classes are stored.
                // They're apparently missing from the files in base.arc obj/pl/pl000000/param/jobleveluptbl files
                // It may be that Season 3 onwards they stopped updating this file
                {
                    // HighScepter growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 120(base lv1) +1.2 / Magi Atk 30(base lv1) +3 / Magi Def 120(base lv1) +1.2 / Blow 25(base lv1) +0
                    JobId.HighScepter,
                    new JobLevelUp[]
                    {
                        new JobLevelUp() {
                    Lv = 0,
                    Atk = 27,
                    Def = (ushort)118.8,
                    MAtk = 27,
                    MDef = (ushort)118.8
                },
                new JobLevelUp() {
                    Lv = 1,
                    Atk = 30,
                    Def = 120,
                    MAtk = 30,
                    MDef = 120
                },
                new JobLevelUp() {
                    Lv = 11,
                    Atk = 60,
                    Def = 132,
                    MAtk = 60,
                    MDef = 132
                },
                new JobLevelUp() {
                    Lv = 21,
                    Atk = 90,
                    Def = 144,
                    MAtk = 90,
                    MDef = 144
                },
                new JobLevelUp() {
                    Lv = 31,
                    Atk = 120,
                    Def = 156,
                    MAtk = 120,
                    MDef = 156
                },
                new JobLevelUp() {
                    Lv = 41,
                    Atk = 150,
                    Def = 168,
                    MAtk = 150,
                    MDef = 168
                },
                new JobLevelUp() {
                    Lv = 51,
                    Atk = 180,
                    Def = 180,
                    MAtk = 180,
                    MDef = 180
                },
                new JobLevelUp() {
                    Lv = 61,
                    Atk = 210,
                    Def = 192,
                    MAtk = 210,
                    MDef = 192
                },
                new JobLevelUp() {
                    Lv = 71,
                    Atk = 240,
                    Def = 204,
                    MAtk = 240,
                    MDef = 204
                },
                new JobLevelUp() {
                    Lv = 81,
                    Atk = 270,
                    Def = 216,
                    MAtk = 270,
                    MDef = 216
                },
                new JobLevelUp() {
                    Lv = 91,
                    Atk = 300,
                    Def = 228,
                    MAtk = 300,
                    MDef = 228
                },
                new JobLevelUp() {
                    Lv = 101,
                    Atk = 330,
                    Def = 240,
                    MAtk = 330,
                    MDef = 240
                },
                new JobLevelUp() {
                    Lv = 111,
                    Atk = 360,
                    Def = 252,
                    MAtk = 360,
                    MDef = 252
                },
                new JobLevelUp() {
                    Lv = 121,
                    Atk = 390,
                    Def = 264,
                    MAtk = 390,
                    MDef = 264
                },
                // The below values are not adjusted
                new JobLevelUp() {
                    Lv = 129,
                    Atk = 230,
                    Def = 550,
                    MAtk = 140,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 139,
                    Atk = 240,
                    Def = 570,
                    MAtk = 150,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 149,
                    Atk = 250,
                    Def = 590,
                    MAtk = 160,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 159,
                    Atk = 260,
                    Def = 610,
                    MAtk = 170,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 169,
                    Atk = 270,
                    Def = 630,
                    MAtk = 180,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 179,
                    Atk = 280,
                    Def = 650,
                    MAtk = 190,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 189,
                    Atk = 290,
                    Def = 670,
                    MAtk = 200,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 199,
                    Atk = 300,
                    Def = 690,
                    MAtk = 210,
                    MDef = 300
                },
                new JobLevelUp() {
                    Lv = 999,
                    Atk = 300,
                    Def = 690,
                    MAtk = 210,
                    MDef = 300
                }
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
            CDataCharacterJobData activeCharacterJobData = characterToAddExpTo.ActiveCharacterJobData;
            if (activeCharacterJobData.Lv < ExpManager.LV_CAP)
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

                while (targetLevel < LV_CAP && activeCharacterJobData.Exp >= this.TotalExpToLevelUpTo(targetLevel + 1))
                {
                    targetLevel++;
                    addJobPoint+=LEVEL_UP_JOB_POINTS_EARNED[targetLevel];
                }

                if (currentLevel != targetLevel)
                {
                    activeCharacterJobData.Lv = targetLevel;
                    activeCharacterJobData.JobPoint += addJobPoint;
                    JobLevelUp jobLevelUp = GetJobLevelUp(activeCharacterJobData.Job, activeCharacterJobData.Lv);

                    // Growth sources: https://wiki.famitsu.com/dd-online/%E3%82%B8%E3%83%A7%E3%83%96/%E3%83%95%E3%82%A1%E3%82%A4%E3%82%BF%E3%83%BC
                    // Fighter growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 120(base lv1) +1.2 / Magi Atk 20(base lv1) +2 / Magi Def 83(base lv1) +0.8 / Blow 25(base lv1) +0
                    // Check class Id and cap level
                    if (activeCharacterJobData.Job == JobId.Fighter && targetLevel < LV_CAP)
                    {
                        // Loop to increase statuses per level
                        for (int i = 0; i < LEVEL_UP_TABLE[JobId.Fighter].Length; i++)
                        {
                            if (i > 0)
                            {
                                // Use the previous values and increase
                                LEVEL_UP_TABLE[JobId.Fighter][i].Atk = (ushort)(LEVEL_UP_TABLE[JobId.Fighter][i-1].Atk + 3);
                                LEVEL_UP_TABLE[JobId.Fighter][i].Def = (ushort)(LEVEL_UP_TABLE[JobId.Fighter][i-1].Def + 1.2);
                                LEVEL_UP_TABLE[JobId.Fighter][i].MAtk = (ushort)(LEVEL_UP_TABLE[JobId.Fighter][i-1].MAtk + 2);
                                LEVEL_UP_TABLE[JobId.Fighter][i].MDef = (ushort)(LEVEL_UP_TABLE[JobId.Fighter][i-1].MDef + 0.8);
                            }
                        }
                        // Add the values to activeCharacterJobData if they are greater or equal than the previous ones
                        if (LEVEL_UP_TABLE[JobId.Fighter][activeCharacterJobData.Lv].Atk >= activeCharacterJobData.Atk)
                        {
                            activeCharacterJobData.Atk = LEVEL_UP_TABLE[JobId.Fighter][activeCharacterJobData.Lv].Atk;
                        }
                        if (LEVEL_UP_TABLE[JobId.Fighter][activeCharacterJobData.Lv].Def >= activeCharacterJobData.Def)
                        {
                            activeCharacterJobData.Def = LEVEL_UP_TABLE[JobId.Fighter][activeCharacterJobData.Lv].Def;
                        }
                        if (LEVEL_UP_TABLE[JobId.Fighter][activeCharacterJobData.Lv].MAtk >= activeCharacterJobData.MAtk)
                        {
                            activeCharacterJobData.MAtk = LEVEL_UP_TABLE[JobId.Fighter][activeCharacterJobData.Lv].MAtk;
                        }
                        if (LEVEL_UP_TABLE[JobId.Fighter][activeCharacterJobData.Lv].MDef >= activeCharacterJobData.MDef)
                        {
                            activeCharacterJobData.MDef = LEVEL_UP_TABLE[JobId.Fighter][activeCharacterJobData.Lv].MDef;
                        }
                    }
                    // Seeker growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 83(base lv1) +0.8 / Magi Atk 20(base lv1) +2 / Magi Def 83(base lv1) +0.8 / Blow 10(base lv1) +0
                    // Check class Id and cap level
                    else if (activeCharacterJobData.Job == JobId.Seeker && targetLevel < LV_CAP)
                    {
                        // Loop to increase statuses per level
                        for (int i = 0; i < LEVEL_UP_TABLE[JobId.Seeker].Length; i++)
                        {
                            if (i > 0)
                            {
                                // Use the previous values and increase
                                LEVEL_UP_TABLE[JobId.Seeker][i].Atk = (ushort)(LEVEL_UP_TABLE[JobId.Seeker][i-1].Atk + 3);
                                LEVEL_UP_TABLE[JobId.Seeker][i].Def = (ushort)(LEVEL_UP_TABLE[JobId.Seeker][i-1].Def + 0.8);
                                LEVEL_UP_TABLE[JobId.Seeker][i].MAtk = (ushort)(LEVEL_UP_TABLE[JobId.Seeker][i-1].MAtk + 2);
                                LEVEL_UP_TABLE[JobId.Seeker][i].MDef = (ushort)(LEVEL_UP_TABLE[JobId.Seeker][i-1].MDef + 0.8);
                            }
                        }
                        // Add the values to activeCharacterJobData if they are greater or equal than the previous ones
                        if (LEVEL_UP_TABLE[JobId.Seeker][activeCharacterJobData.Lv].Atk >= activeCharacterJobData.Atk)
                        {
                            activeCharacterJobData.Atk = LEVEL_UP_TABLE[JobId.Seeker][activeCharacterJobData.Lv].Atk;
                        }
                        if (LEVEL_UP_TABLE[JobId.Seeker][activeCharacterJobData.Lv].Def >= activeCharacterJobData.Def)
                        {
                            activeCharacterJobData.Def = LEVEL_UP_TABLE[JobId.Seeker][activeCharacterJobData.Lv].Def;
                        }
                        if (LEVEL_UP_TABLE[JobId.Seeker][activeCharacterJobData.Lv].MAtk >= activeCharacterJobData.MAtk)
                        {
                            activeCharacterJobData.MAtk = LEVEL_UP_TABLE[JobId.Seeker][activeCharacterJobData.Lv].MAtk;
                        }
                        if (LEVEL_UP_TABLE[JobId.Seeker][activeCharacterJobData.Lv].MDef >= activeCharacterJobData.MDef)
                        {
                            activeCharacterJobData.MDef = LEVEL_UP_TABLE[JobId.Seeker][activeCharacterJobData.Lv].MDef;
                        }
                    }
                    // Hunter growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 83(base lv1) +0.8 / Magi Atk 20(base lv1) +2 / Magi Def 83(base lv1) +0.8 / Blow 25(base lv1) +0
                    // Check class Id and cap level
                    else if (activeCharacterJobData.Job == JobId.Hunter && targetLevel < LV_CAP)
                    {
                        // Loop to increase statuses per level
                        for (int i = 0; i < LEVEL_UP_TABLE[JobId.Hunter].Length; i++)
                        {
                            if (i > 0)
                            {
                                // Use the previous values and increase
                                LEVEL_UP_TABLE[JobId.Hunter][i].Atk = (ushort)(LEVEL_UP_TABLE[JobId.Hunter][i-1].Atk + 3);
                                LEVEL_UP_TABLE[JobId.Hunter][i].Def = (ushort)(LEVEL_UP_TABLE[JobId.Hunter][i-1].Def + 0.8);
                                LEVEL_UP_TABLE[JobId.Hunter][i].MAtk = (ushort)(LEVEL_UP_TABLE[JobId.Hunter][i-1].MAtk + 2);
                                LEVEL_UP_TABLE[JobId.Hunter][i].MDef = (ushort)(LEVEL_UP_TABLE[JobId.Hunter][i-1].MDef + 0.8);
                            }
                        }
                        // Add the values to activeCharacterJobData if they are greater or equal than the previous ones
                        if (LEVEL_UP_TABLE[JobId.Hunter][activeCharacterJobData.Lv].Atk >= activeCharacterJobData.Atk)
                        {
                            activeCharacterJobData.Atk = LEVEL_UP_TABLE[JobId.Hunter][activeCharacterJobData.Lv].Atk;
                        }
                        if (LEVEL_UP_TABLE[JobId.Hunter][activeCharacterJobData.Lv].Def >= activeCharacterJobData.Def)
                        {
                            activeCharacterJobData.Def = LEVEL_UP_TABLE[JobId.Hunter][activeCharacterJobData.Lv].Def;
                        }
                        if (LEVEL_UP_TABLE[JobId.Hunter][activeCharacterJobData.Lv].MAtk >= activeCharacterJobData.MAtk)
                        {
                            activeCharacterJobData.MAtk = LEVEL_UP_TABLE[JobId.Hunter][activeCharacterJobData.Lv].MAtk;
                        }
                        if (LEVEL_UP_TABLE[JobId.Hunter][activeCharacterJobData.Lv].MDef >= activeCharacterJobData.MDef)
                        {
                            activeCharacterJobData.MDef = LEVEL_UP_TABLE[JobId.Hunter][activeCharacterJobData.Lv].MDef;
                        }
                    }
                    // Priest growth per lv: Phys Atk 20(base lv1) +2 / Phys Def 58(base lv1) +0.4 / Magi Atk 30(base lv1) +3 / Magi Def 100(base lv1) +1 / Blow 18(base lv1) +0
                    // Check class Id and cap level
                    else if (activeCharacterJobData.Job == JobId.Priest && targetLevel < LV_CAP)
                    {
                        // Loop to increase statuses per level
                        for (int i = 0; i < LEVEL_UP_TABLE[JobId.Priest].Length; i++)
                        {
                            if (i > 0)
                            {
                                // Use the previous values and increase
                                LEVEL_UP_TABLE[JobId.Priest][i].Atk = (ushort)(LEVEL_UP_TABLE[JobId.Priest][i-1].Atk + 2);
                                LEVEL_UP_TABLE[JobId.Priest][i].Def = (ushort)(LEVEL_UP_TABLE[JobId.Priest][i-1].Def + 0.4);
                                LEVEL_UP_TABLE[JobId.Priest][i].MAtk = (ushort)(LEVEL_UP_TABLE[JobId.Priest][i-1].MAtk + 3);
                                LEVEL_UP_TABLE[JobId.Priest][i].MDef = (ushort)(LEVEL_UP_TABLE[JobId.Priest][i-1].MDef + 1);
                            }
                        }
                        // Add the values to activeCharacterJobData if they are greater or equal than the previous ones
                        if (LEVEL_UP_TABLE[JobId.Priest][activeCharacterJobData.Lv].Atk >= activeCharacterJobData.Atk)
                        {
                            activeCharacterJobData.Atk = LEVEL_UP_TABLE[JobId.Priest][activeCharacterJobData.Lv].Atk;
                        }
                        if (LEVEL_UP_TABLE[JobId.Priest][activeCharacterJobData.Lv].Def >= activeCharacterJobData.Def)
                        {
                            activeCharacterJobData.Def = LEVEL_UP_TABLE[JobId.Priest][activeCharacterJobData.Lv].Def;
                        }
                        if (LEVEL_UP_TABLE[JobId.Priest][activeCharacterJobData.Lv].MAtk >= activeCharacterJobData.MAtk)
                        {
                            activeCharacterJobData.MAtk = LEVEL_UP_TABLE[JobId.Priest][activeCharacterJobData.Lv].MAtk;
                        }
                        if (LEVEL_UP_TABLE[JobId.Priest][activeCharacterJobData.Lv].MDef >= activeCharacterJobData.MDef)
                        {
                            activeCharacterJobData.MDef = LEVEL_UP_TABLE[JobId.Priest][activeCharacterJobData.Lv].MDef;
                        }
                    }
                    // ShieldSage growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 120(base lv1) +1.2 / Magi Atk 30(base lv1) +3 / Magi Def 120(base lv1) +1.2 / Blow 30(base lv1) +0
                    // Check class Id and cap level
                    else if (activeCharacterJobData.Job == JobId.ShieldSage && targetLevel < LV_CAP)
                    {
                        // Loop to increase statuses per level
                        for (int i = 0; i < LEVEL_UP_TABLE[JobId.ShieldSage].Length; i++)
                        {
                            if (i > 0)
                            {
                                // Use the previous values and increase
                                LEVEL_UP_TABLE[JobId.ShieldSage][i].Atk = (ushort)(LEVEL_UP_TABLE[JobId.ShieldSage][i-1].Atk + 3);
                                LEVEL_UP_TABLE[JobId.ShieldSage][i].Def = (ushort)(LEVEL_UP_TABLE[JobId.ShieldSage][i-1].Def + 1.2);
                                LEVEL_UP_TABLE[JobId.ShieldSage][i].MAtk = (ushort)(LEVEL_UP_TABLE[JobId.ShieldSage][i-1].MAtk + 3);
                                LEVEL_UP_TABLE[JobId.ShieldSage][i].MDef = (ushort)(LEVEL_UP_TABLE[JobId.ShieldSage][i-1].MDef + 1.2);
                            }
                        }
                        // Add the values to activeCharacterJobData if they are greater or equal than the previous ones
                        if (LEVEL_UP_TABLE[JobId.ShieldSage][activeCharacterJobData.Lv].Atk >= activeCharacterJobData.Atk)
                        {
                            activeCharacterJobData.Atk = LEVEL_UP_TABLE[JobId.ShieldSage][activeCharacterJobData.Lv].Atk;
                        }
                        if (LEVEL_UP_TABLE[JobId.ShieldSage][activeCharacterJobData.Lv].Def >= activeCharacterJobData.Def)
                        {
                            activeCharacterJobData.Def = LEVEL_UP_TABLE[JobId.ShieldSage][activeCharacterJobData.Lv].Def;
                        }
                        if (LEVEL_UP_TABLE[JobId.ShieldSage][activeCharacterJobData.Lv].MAtk >= activeCharacterJobData.MAtk)
                        {
                            activeCharacterJobData.MAtk = LEVEL_UP_TABLE[JobId.ShieldSage][activeCharacterJobData.Lv].MAtk;
                        }
                        if (LEVEL_UP_TABLE[JobId.ShieldSage][activeCharacterJobData.Lv].MDef >= activeCharacterJobData.MDef)
                        {
                            activeCharacterJobData.MDef = LEVEL_UP_TABLE[JobId.ShieldSage][activeCharacterJobData.Lv].MDef;
                        }
                    }
                    // Sorcerer growth per lv: Phys Atk 20(base lv1) +2 / Phys Def 58(base lv1) +0.4 / Magi Atk 30(base lv1) +3 / Magi Def 100(base lv1) +1 / Blow 30(base lv1) +0
                    // Check class Id and cap level
                    else if (activeCharacterJobData.Job == JobId.Sorcerer && targetLevel < LV_CAP)
                    {
                        // Loop to increase statuses per level
                        for (int i = 0; i < LEVEL_UP_TABLE[JobId.Sorcerer].Length; i++)
                        {
                            if (i > 0)
                            {
                                // Use the previous values and increase
                                LEVEL_UP_TABLE[JobId.Sorcerer][i].Atk = (ushort)(LEVEL_UP_TABLE[JobId.Sorcerer][i-1].Atk + 2);
                                LEVEL_UP_TABLE[JobId.Sorcerer][i].Def = (ushort)(LEVEL_UP_TABLE[JobId.Sorcerer][i-1].Def + 0.4);
                                LEVEL_UP_TABLE[JobId.Sorcerer][i].MAtk = (ushort)(LEVEL_UP_TABLE[JobId.Sorcerer][i-1].MAtk + 3);
                                LEVEL_UP_TABLE[JobId.Sorcerer][i].MDef = (ushort)(LEVEL_UP_TABLE[JobId.Sorcerer][i-1].MDef + 1);
                            }
                        }
                        // Add the values to activeCharacterJobData if they are greater or equal than the previous ones
                        if (LEVEL_UP_TABLE[JobId.Sorcerer][activeCharacterJobData.Lv].Atk >= activeCharacterJobData.Atk)
                        {
                            activeCharacterJobData.Atk = LEVEL_UP_TABLE[JobId.Sorcerer][activeCharacterJobData.Lv].Atk;
                        }
                        if (LEVEL_UP_TABLE[JobId.Sorcerer][activeCharacterJobData.Lv].Def >= activeCharacterJobData.Def)
                        {
                            activeCharacterJobData.Def = LEVEL_UP_TABLE[JobId.Sorcerer][activeCharacterJobData.Lv].Def;
                        }
                        if (LEVEL_UP_TABLE[JobId.Sorcerer][activeCharacterJobData.Lv].MAtk >= activeCharacterJobData.MAtk)
                        {
                            activeCharacterJobData.MAtk = LEVEL_UP_TABLE[JobId.Sorcerer][activeCharacterJobData.Lv].MAtk;
                        }
                        if (LEVEL_UP_TABLE[JobId.Sorcerer][activeCharacterJobData.Lv].MDef >= activeCharacterJobData.MDef)
                        {
                            activeCharacterJobData.MDef = LEVEL_UP_TABLE[JobId.Sorcerer][activeCharacterJobData.Lv].MDef;
                        }
                    }
                    // Warrior growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 120(base lv1) +1.2 / Magi Atk 20(base lv1) +2 / Magi Def 83(base lv1) +0.8 / Blow 35(base lv1) +0
                    // Check class Id and cap level
                    else if (activeCharacterJobData.Job == JobId.Warrior && targetLevel < LV_CAP)
                    {
                        // Loop to increase statuses per level
                        for (int i = 0; i < LEVEL_UP_TABLE[JobId.Warrior].Length; i++)
                        {
                            if (i > 0)
                            {
                                // Use the previous values and increase
                                LEVEL_UP_TABLE[JobId.Warrior][i].Atk = (ushort)(LEVEL_UP_TABLE[JobId.Warrior][i-1].Atk + 3);
                                LEVEL_UP_TABLE[JobId.Warrior][i].Def = (ushort)(LEVEL_UP_TABLE[JobId.Warrior][i-1].Def + 1.2);
                                LEVEL_UP_TABLE[JobId.Warrior][i].MAtk = (ushort)(LEVEL_UP_TABLE[JobId.Warrior][i-1].MAtk + 2);
                                LEVEL_UP_TABLE[JobId.Warrior][i].MDef = (ushort)(LEVEL_UP_TABLE[JobId.Warrior][i-1].MDef + 0.8);
                            }
                        }
                        // Add the values to activeCharacterJobData if they are greater or equal than the previous ones
                        if (LEVEL_UP_TABLE[JobId.Warrior][activeCharacterJobData.Lv].Atk >= activeCharacterJobData.Atk)
                        {
                            activeCharacterJobData.Atk = LEVEL_UP_TABLE[JobId.Warrior][activeCharacterJobData.Lv].Atk;
                        }
                        if (LEVEL_UP_TABLE[JobId.Warrior][activeCharacterJobData.Lv].Def >= activeCharacterJobData.Def)
                        {
                            activeCharacterJobData.Def = LEVEL_UP_TABLE[JobId.Warrior][activeCharacterJobData.Lv].Def;
                        }
                        if (LEVEL_UP_TABLE[JobId.Warrior][activeCharacterJobData.Lv].MAtk >= activeCharacterJobData.MAtk)
                        {
                            activeCharacterJobData.MAtk = LEVEL_UP_TABLE[JobId.Warrior][activeCharacterJobData.Lv].MAtk;
                        }
                        if (LEVEL_UP_TABLE[JobId.Warrior][activeCharacterJobData.Lv].MDef >= activeCharacterJobData.MDef)
                        {
                            activeCharacterJobData.MDef = LEVEL_UP_TABLE[JobId.Warrior][activeCharacterJobData.Lv].MDef;
                        }
                    }
                    // ElementArcher growth per lv: Phys Atk 20(base lv1) +2 / Phys Def 58(base lv1) +0.4 / Magi Atk 30(base lv1) +3 / Magi Def 100(base lv1) +1 / Blow 18(base lv1) +0
                    // Check class Id and cap level
                    else if (activeCharacterJobData.Job == JobId.ElementArcher && targetLevel < LV_CAP)
                    {
                        // Loop to increase statuses per level
                        for (int i = 0; i < LEVEL_UP_TABLE[JobId.ElementArcher].Length; i++)
                        {
                            if (i > 0)
                            {
                                // Use the previous values and increase
                                LEVEL_UP_TABLE[JobId.ElementArcher][i].Atk = (ushort)(LEVEL_UP_TABLE[JobId.ElementArcher][i-1].Atk + 2);
                                LEVEL_UP_TABLE[JobId.ElementArcher][i].Def = (ushort)(LEVEL_UP_TABLE[JobId.ElementArcher][i-1].Def + 0.4);
                                LEVEL_UP_TABLE[JobId.ElementArcher][i].MAtk = (ushort)(LEVEL_UP_TABLE[JobId.ElementArcher][i-1].MAtk + 3);
                                LEVEL_UP_TABLE[JobId.ElementArcher][i].MDef = (ushort)(LEVEL_UP_TABLE[JobId.ElementArcher][i-1].MDef + 1);
                            }
                        }
                        // Add the values to activeCharacterJobData if they are greater or equal than the previous ones
                        if (LEVEL_UP_TABLE[JobId.ElementArcher][activeCharacterJobData.Lv].Atk >= activeCharacterJobData.Atk)
                        {
                            activeCharacterJobData.Atk = LEVEL_UP_TABLE[JobId.ElementArcher][activeCharacterJobData.Lv].Atk;
                        }
                        if (LEVEL_UP_TABLE[JobId.ElementArcher][activeCharacterJobData.Lv].Def >= activeCharacterJobData.Def)
                        {
                            activeCharacterJobData.Def = LEVEL_UP_TABLE[JobId.ElementArcher][activeCharacterJobData.Lv].Def;
                        }
                        if (LEVEL_UP_TABLE[JobId.ElementArcher][activeCharacterJobData.Lv].MAtk >= activeCharacterJobData.MAtk)
                        {
                            activeCharacterJobData.MAtk = LEVEL_UP_TABLE[JobId.ElementArcher][activeCharacterJobData.Lv].MAtk;
                        }
                        if (LEVEL_UP_TABLE[JobId.ElementArcher][activeCharacterJobData.Lv].MDef >= activeCharacterJobData.MDef)
                        {
                            activeCharacterJobData.MDef = LEVEL_UP_TABLE[JobId.ElementArcher][activeCharacterJobData.Lv].MDef;
                        }
                    }
                    // Growth sources: https://dogmaonlineyyy.wiki.fc2.com/wiki/%E3%82%A2%E3%83%AB%E3%82%B1%E3%83%9F%E3%82%B9%E3%83%88
                    // Growth not actually there, it's just following the above logic and patterns
                    // Alchemist growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 100(base lv1) +1 / Magi Atk 20(base lv1) +2 / Magi Def 100(base lv1) +1 / Blow 25(base lv1) +0
                    // Check class Id and cap level
                    else if (activeCharacterJobData.Job == JobId.Alchemist && targetLevel < LV_CAP)
                    {
                        // Loop to increase statuses per level
                        for (int i = 0; i < LEVEL_UP_TABLE[JobId.Alchemist].Length; i++)
                        {
                            if (i > 0)
                            {
                                // Use the previous values and increase
                                LEVEL_UP_TABLE[JobId.Alchemist][i].Atk = (ushort)(LEVEL_UP_TABLE[JobId.Alchemist][i-1].Atk + 3);
                                LEVEL_UP_TABLE[JobId.Alchemist][i].Def = (ushort)(LEVEL_UP_TABLE[JobId.Alchemist][i-1].Def + 1);
                                LEVEL_UP_TABLE[JobId.Alchemist][i].MAtk = (ushort)(LEVEL_UP_TABLE[JobId.Alchemist][i-1].MAtk + 2);
                                LEVEL_UP_TABLE[JobId.Alchemist][i].MDef = (ushort)(LEVEL_UP_TABLE[JobId.Alchemist][i-1].MDef + 1);
                            }
                        }
                        // Add the values to activeCharacterJobData if they are greater or equal than the previous ones
                        if (LEVEL_UP_TABLE[JobId.Alchemist][activeCharacterJobData.Lv].Atk >= activeCharacterJobData.Atk)
                        {
                            activeCharacterJobData.Atk = LEVEL_UP_TABLE[JobId.Alchemist][activeCharacterJobData.Lv].Atk;
                        }
                        if (LEVEL_UP_TABLE[JobId.Alchemist][activeCharacterJobData.Lv].Def >= activeCharacterJobData.Def)
                        {
                            activeCharacterJobData.Def = LEVEL_UP_TABLE[JobId.Alchemist][activeCharacterJobData.Lv].Def;
                        }
                        if (LEVEL_UP_TABLE[JobId.Alchemist][activeCharacterJobData.Lv].MAtk >= activeCharacterJobData.MAtk)
                        {
                            activeCharacterJobData.MAtk = LEVEL_UP_TABLE[JobId.Alchemist][activeCharacterJobData.Lv].MAtk;
                        }
                        if (LEVEL_UP_TABLE[JobId.Alchemist][activeCharacterJobData.Lv].MDef >= activeCharacterJobData.MDef)
                        {
                            activeCharacterJobData.MDef = LEVEL_UP_TABLE[JobId.Alchemist][activeCharacterJobData.Lv].MDef;
                        }
                    }
                    // Confirmed statuses and added proper growth rate
                    // SpiritLancer growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 83(base lv1) +0.8 / Magi Atk 30(base lv1) +3 / Magi Def 83(base lv1) +0.8 / Blow 25(base lv1) +0
                    // Check class Id and cap level
                    else if (activeCharacterJobData.Job == JobId.SpiritLancer && targetLevel < LV_CAP)
                    {
                        // Loop to increase statuses per level
                        for (int i = 0; i < LEVEL_UP_TABLE[JobId.SpiritLancer].Length; i++)
                        {
                            if (i > 0)
                            {
                                // Use the previous values and increase
                                LEVEL_UP_TABLE[JobId.SpiritLancer][i].Atk = (ushort)(LEVEL_UP_TABLE[JobId.SpiritLancer][i-1].Atk + 3);
                                LEVEL_UP_TABLE[JobId.SpiritLancer][i].Def = (ushort)(LEVEL_UP_TABLE[JobId.SpiritLancer][i-1].Def + 0.8);
                                LEVEL_UP_TABLE[JobId.SpiritLancer][i].MAtk = (ushort)(LEVEL_UP_TABLE[JobId.SpiritLancer][i-1].MAtk + 3);
                                LEVEL_UP_TABLE[JobId.SpiritLancer][i].MDef = (ushort)(LEVEL_UP_TABLE[JobId.SpiritLancer][i-1].MDef + 0.8);
                            }
                        }
                        // Add the values to activeCharacterJobData if they are greater or equal than the previous ones
                        if (LEVEL_UP_TABLE[JobId.SpiritLancer][activeCharacterJobData.Lv].Atk >= activeCharacterJobData.Atk)
                        {
                            activeCharacterJobData.Atk = LEVEL_UP_TABLE[JobId.SpiritLancer][activeCharacterJobData.Lv].Atk;
                        }
                        if (LEVEL_UP_TABLE[JobId.SpiritLancer][activeCharacterJobData.Lv].Def >= activeCharacterJobData.Def)
                        {
                            activeCharacterJobData.Def = LEVEL_UP_TABLE[JobId.SpiritLancer][activeCharacterJobData.Lv].Def;
                        }
                        if (LEVEL_UP_TABLE[JobId.SpiritLancer][activeCharacterJobData.Lv].MAtk >= activeCharacterJobData.MAtk)
                        {
                            activeCharacterJobData.MAtk = LEVEL_UP_TABLE[JobId.SpiritLancer][activeCharacterJobData.Lv].MAtk;
                        }
                        if (LEVEL_UP_TABLE[JobId.SpiritLancer][activeCharacterJobData.Lv].MDef >= activeCharacterJobData.MDef)
                        {
                            activeCharacterJobData.MDef = LEVEL_UP_TABLE[JobId.SpiritLancer][activeCharacterJobData.Lv].MDef;
                        }
                    }
                    // Based on the placeholder statues and above logic patterns
                    // HighScepter growth per lv: Phys Atk 30(base lv1) +3 / Phys Def 120(base lv1) +1.2 / Magi Atk 30(base lv1) +3 / Magi Def 120(base lv1) +1.2 / Blow 25(base lv1) +0
                    // Check class Id and cap level
                    else if (activeCharacterJobData.Job == JobId.HighScepter && targetLevel < LV_CAP)
                    {
                        // Loop to increase statuses per level
                        for (int i = 0; i < LEVEL_UP_TABLE[JobId.HighScepter].Length; i++)
                        {
                            if (i > 0)
                            {
                                // Use the previous values and increase
                                LEVEL_UP_TABLE[JobId.HighScepter][i].Atk = (ushort)(LEVEL_UP_TABLE[JobId.HighScepter][i-1].Atk + 3);
                                LEVEL_UP_TABLE[JobId.HighScepter][i].Def = (ushort)(LEVEL_UP_TABLE[JobId.HighScepter][i-1].Def + 1.2);
                                LEVEL_UP_TABLE[JobId.HighScepter][i].MAtk = (ushort)(LEVEL_UP_TABLE[JobId.HighScepter][i-1].MAtk + 3);
                                LEVEL_UP_TABLE[JobId.HighScepter][i].MDef = (ushort)(LEVEL_UP_TABLE[JobId.HighScepter][i-1].MDef + 1.2);
                            }
                        }
                        // Add the values to activeCharacterJobData if they are greater or equal than the previous ones
                        if (LEVEL_UP_TABLE[JobId.HighScepter][activeCharacterJobData.Lv].Atk >= activeCharacterJobData.Atk)
                        {
                            activeCharacterJobData.Atk = LEVEL_UP_TABLE[JobId.HighScepter][activeCharacterJobData.Lv].Atk;
                        }
                        if (LEVEL_UP_TABLE[JobId.HighScepter][activeCharacterJobData.Lv].Def >= activeCharacterJobData.Def)
                        {
                            activeCharacterJobData.Def = LEVEL_UP_TABLE[JobId.HighScepter][activeCharacterJobData.Lv].Def;
                        }
                        if (LEVEL_UP_TABLE[JobId.HighScepter][activeCharacterJobData.Lv].MAtk >= activeCharacterJobData.MAtk)
                        {
                            activeCharacterJobData.MAtk = LEVEL_UP_TABLE[JobId.HighScepter][activeCharacterJobData.Lv].MAtk;
                        }
                        if (LEVEL_UP_TABLE[JobId.HighScepter][activeCharacterJobData.Lv].MDef >= activeCharacterJobData.MDef)
                        {
                            activeCharacterJobData.MDef = LEVEL_UP_TABLE[JobId.HighScepter][activeCharacterJobData.Lv].MDef;
                        }
                    }
                    else
                    {
                        // Using Math.Max to prevent lvl up from lowering your stats if you happen to have higher ones
                        activeCharacterJobData.Atk = Math.Max(activeCharacterJobData.Atk, jobLevelUp.Atk);
                        activeCharacterJobData.Def = Math.Max(activeCharacterJobData.Def, jobLevelUp.Def);
                        activeCharacterJobData.MAtk = Math.Max(activeCharacterJobData.MAtk, jobLevelUp.MAtk);
                        activeCharacterJobData.MDef = Math.Max(activeCharacterJobData.MDef, jobLevelUp.MDef);
                        // TODO: Figure out the values for all other job data fields
                        
                    }

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

                // PERSIST CHANGES IN DB
                this._database.UpdateCharacterJobData(characterToAddExpTo.CommonId, activeCharacterJobData);
            }
        }

        private uint TotalExpToLevelUpTo(uint level)
        {
            uint totalExp = 0;
            for (int i = 1; i < level; i++)
            {
                totalExp += EXP_UNTIL_NEXT_LV[i];
            }
            return totalExp;
        }

        private JobLevelUp GetJobLevelUp(JobId jobId, uint level)
        {
            JobLevelUp[] levelUps = LEVEL_UP_TABLE[jobId];
            return levelUps.Where(x => x.Lv < level)
                        .OrderByDescending(x => x.Lv)
                        .First();
        }
    }
}

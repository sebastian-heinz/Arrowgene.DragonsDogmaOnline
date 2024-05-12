using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetAcquirableSkillListHandler : StructurePacketHandler<GameClient, C2SSkillGetAcquirableSkillListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetCurrentSetSkillListHandler));

        public static readonly List<CDataSkillParam> AllSkills = new List<CDataSkillParam>() {
            // Fighter
            new CDataSkillParam() {SkillNo = 1, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 0,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 9,
                    RequireJobPoint = 1000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 15,
                    RequireJobPoint = 2300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7200,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 2, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 3, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 4, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 9,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 5, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 20,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 22,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 25,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 27,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 30,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 6, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 25,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 27,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 30,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 35,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 38,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 7, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 8, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 9, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 10, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 11, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 37,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 39,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 42,
                    RequireJobPoint = 3700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 44,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 46,
                    RequireJobPoint = 5700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 12, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 43,
                    RequireJobPoint = 2800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 47,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 50,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 53,
                    RequireJobPoint = 5000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 56,
                    RequireJobPoint = 6500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 13500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 13, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 45,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 50,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 56,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 62,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 67,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 14, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 60,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 62,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 64,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 68,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 72,
                    RequireJobPoint = 6000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 75,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 12000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 104, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 204, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 102, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 202, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 108, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 208, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 103, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 203, Job = JobId.Fighter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            // Seeker
            new CDataSkillParam() {SkillNo = 1, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 0,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 9,
                    RequireJobPoint = 1000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 15,
                    RequireJobPoint = 2300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7200,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 2, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 9,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 3, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 4, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 5, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 6, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 20,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 22,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 25,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 27,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 30,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 7, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 8, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 25,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 27,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 30,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 35,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 38,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 9, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 10, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 11, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 37,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 39,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 42,
                    RequireJobPoint = 3700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 44,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 46,
                    RequireJobPoint = 5700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 12, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 43,
                    RequireJobPoint = 2800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 47,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 50,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 53,
                    RequireJobPoint = 5000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 56,
                    RequireJobPoint = 6500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 13500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 13, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 45,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 50,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 56,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 62,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 67,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 14, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 60,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 62,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 64,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 68,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 72,
                    RequireJobPoint = 6000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 75,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 12000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 112, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 212, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 113, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 213, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 107, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 207, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 102, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 202, Job = JobId.Seeker, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            // Hunter
            new CDataSkillParam() {SkillNo = 1, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 0,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 9,
                    RequireJobPoint = 1000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 15,
                    RequireJobPoint = 2300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7200,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 2, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 4, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 3, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 9,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 5, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 6, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 7, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 20,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 22,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 25,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 27,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 30,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 8, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 25,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 27,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 30,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 35,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 38,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 9, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 10, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 11, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 37,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 39,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 42,
                    RequireJobPoint = 3700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 44,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 46,
                    RequireJobPoint = 5700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 12, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 43,
                    RequireJobPoint = 2800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 47,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 50,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 53,
                    RequireJobPoint = 5000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 56,
                    RequireJobPoint = 6500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 13500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 13, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 45,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 50,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 56,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 62,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 67,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 14, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 60,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 62,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 64,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 68,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 72,
                    RequireJobPoint = 6000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 75,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 12000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 101, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 201, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 106, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 206, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 108, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 208, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 110, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 210, Job = JobId.Hunter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            // Priest
            new CDataSkillParam() {SkillNo = 1, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 0,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 9,
                    RequireJobPoint = 1000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 15,
                    RequireJobPoint = 2300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7200,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 2, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 9,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 3, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 4, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 5, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 6, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 20,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 22,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 25,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 27,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 30,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 7, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 8, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 25,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 27,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 30,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 35,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 38,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 9, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 10, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 11, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 37,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 39,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 42,
                    RequireJobPoint = 3700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 44,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 46,
                    RequireJobPoint = 5700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 12, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 43,
                    RequireJobPoint = 2800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 47,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 50,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 53,
                    RequireJobPoint = 5000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 56,
                    RequireJobPoint = 6500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 13500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 13, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 45,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 50,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 56,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 62,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 67,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14000,
                    IsRelease = true
                }
            }},
			new CDataSkillParam() {SkillNo = 14, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 60,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 62,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 64,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 68,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 72,
                    RequireJobPoint = 6000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 75,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 12000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 113, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 213, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 105, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 205, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 104, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 204, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 102, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 202, Job = JobId.Priest, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            // Shield Sage
            new CDataSkillParam() {SkillNo = 1, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 0,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 9,
                    RequireJobPoint = 1000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 15,
                    RequireJobPoint = 2300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7200,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 2, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 3, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 9,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 4, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 5, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 6, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 7, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 25,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 27,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 30,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 35,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 38,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 8, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 20,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 22,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 25,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 27,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 30,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 9, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 10, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 11, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 37,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 39,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 42,
                    RequireJobPoint = 3700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 44,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 46,
                    RequireJobPoint = 5700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 12, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 43,
                    RequireJobPoint = 2800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 47,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 50,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 53,
                    RequireJobPoint = 5000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 56,
                    RequireJobPoint = 6500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 13500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 13, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 60,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 62,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 64,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 68,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 72,
                    RequireJobPoint = 6000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 75,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 12000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 14, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 45,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 50,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 56,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 62,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 67,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 106, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 206, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 101, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 201, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 105, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 205, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 107, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 207, Job = JobId.ShieldSage, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            // Sorcerer
            new CDataSkillParam() {SkillNo = 1, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 0,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 9,
                    RequireJobPoint = 1000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 15,
                    RequireJobPoint = 2300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7200,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 2, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 9,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 3, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 4, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 5, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 6, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 25,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 27,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 30,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 35,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 38,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 7, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 8, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 20,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 22,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 25,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 27,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 30,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 9, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 10, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 11, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 37,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 39,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 42,
                    RequireJobPoint = 3700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 44,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 46,
                    RequireJobPoint = 5700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 12, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 43,
                    RequireJobPoint = 2800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 47,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 50,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 53,
                    RequireJobPoint = 5000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 56,
                    RequireJobPoint = 6500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 13500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 13, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 45,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 50,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 56,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 62,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 67,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 14, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 60,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 62,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 64,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 68,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 72,
                    RequireJobPoint = 6000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 75,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 12000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 110, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 210, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 102, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 202, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 104, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 204, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 105, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 205, Job = JobId.Sorcerer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            // Warrior
            new CDataSkillParam() {SkillNo = 1, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 0,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 9,
                    RequireJobPoint = 1000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 15,
                    RequireJobPoint = 2300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7200,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 3, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 9,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 2, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 4, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 5, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 6, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 8, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 20,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 22,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 25,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 27,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 30,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 7, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 25,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 27,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 30,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 35,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 38,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 9, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 10, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 12, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 37,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 39,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 42,
                    RequireJobPoint = 3700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 44,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 46,
                    RequireJobPoint = 5700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 11, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 43,
                    RequireJobPoint = 2800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 47,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 50,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 53,
                    RequireJobPoint = 5000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 56,
                    RequireJobPoint = 6500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 13500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 13, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 45,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 50,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 56,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 62,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 67,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 14, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 60,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 62,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 64,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 68,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 72,
                    RequireJobPoint = 6000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 75,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 12000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 103, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 203, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 102, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 202, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 104, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 204, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 105, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 205, Job = JobId.Warrior, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            // Element Archer
            new CDataSkillParam() {SkillNo = 1, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 9,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 2, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 3, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 0,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 9,
                    RequireJobPoint = 1000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 15,
                    RequireJobPoint = 2300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7200,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 4, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 25,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 27,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 30,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 35,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 38,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 5, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 6, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 7, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 8, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 20,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 22,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 25,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 27,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 30,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 9, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 10, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 30,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 32,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 38,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 42,
                    RequireJobPoint = 4900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 11, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 35,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 37,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 39,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 42,
                    RequireJobPoint = 3700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 44,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 46,
                    RequireJobPoint = 5700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 12, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 43,
                    RequireJobPoint = 2800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 47,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 50,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 53,
                    RequireJobPoint = 5000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 56,
                    RequireJobPoint = 6500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 13500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 13, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 45,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 50,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 56,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 62,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 67,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 14, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 60,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 62,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 64,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 68,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 72,
                    RequireJobPoint = 6000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 75,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 12000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 105, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 205, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 110, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 210, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 113, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 213, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 108, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 208, Job = JobId.ElementArcher, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            // Alchemist
            new CDataSkillParam() {SkillNo = 2, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 0,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 9,
                    RequireJobPoint = 1000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 15,
                    RequireJobPoint = 2300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7200,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 1, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 9,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 5, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 6, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 4, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 3, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 7, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 20,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 22,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 25,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 27,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 30,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 8, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 25,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 27,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 30,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 35,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 38,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 9, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 45,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 50,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 56,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 62,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 67,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 10, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 60,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 62,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 64,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 68,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 72,
                    RequireJobPoint = 6000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 75,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 12000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 11, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 60,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 62,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 64,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 68,
                    RequireJobPoint = 5700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 72,
                    RequireJobPoint = 6700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 75,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 12500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 15000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 12, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 64,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 66,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 68,
                    RequireJobPoint = 5700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 71,
                    RequireJobPoint = 6700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 74,
                    RequireJobPoint = 7700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 78,
                    RequireJobPoint = 9000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 12000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 13500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 16000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 103, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 203, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 102, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 202, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 101, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 201, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 105, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 205, Job = JobId.Alchemist, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            // Spirit Lance
            new CDataSkillParam() {SkillNo = 1, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 0,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 3,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 6,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 9,
                    RequireJobPoint = 1000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 12,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 25,
                    RequireJobPoint = 2300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7200,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 2, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 6,
                    RequireJobPoint = 500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 30,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 3, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 20,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 22,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 25,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 27,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 30,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 40,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 4, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 35,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 5, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 25,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 27,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 30,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 35,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 45,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 6, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 60,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 62,
                    RequireJobPoint = 3300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 64,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 68,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 72,
                    RequireJobPoint = 6000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 75,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 12000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14500,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 7, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 35,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 8, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 45,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 50,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 56,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 62,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 67,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 9, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 60,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 62,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 64,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 68,
                    RequireJobPoint = 5700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 72,
                    RequireJobPoint = 6700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 75,
                    RequireJobPoint = 8000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 12500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 15000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 10, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 64,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 66,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 68,
                    RequireJobPoint = 5700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 71,
                    RequireJobPoint = 6700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 74,
                    RequireJobPoint = 7700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 78,
                    RequireJobPoint = 9000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 12000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 13500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 16000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 107, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 207, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 101, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 201, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 104, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 204, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 108, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            new CDataSkillParam() {SkillNo = 208, Job = JobId.SpiritLancer, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                }
			}},
            // High Scepter
            new CDataSkillParam() {SkillNo = 1, Job = JobId.HighScepter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 1,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 9,
                    RequireJobPoint = 800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 13,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 16,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 18,
                    RequireJobPoint = 2100,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 20,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8400,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 2, Job = JobId.HighScepter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 20,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 22,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 25,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 27,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 30,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 4, Job = JobId.HighScepter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 0,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 0,
                    RequireJobPoint = 300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 0,
                    RequireJobPoint = 600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 0,
                    RequireJobPoint = 1000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 0,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 0,
                    RequireJobPoint = 2300,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 3200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7200,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 3, Job = JobId.HighScepter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 60,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 66,
                    RequireJobPoint = 4800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 68,
                    RequireJobPoint = 5700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 71,
                    RequireJobPoint = 6700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 74,
                    RequireJobPoint = 7700,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 78,
                    RequireJobPoint = 9000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 12000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 13500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 16000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 5, Job = JobId.HighScepter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 6, Job = JobId.HighScepter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 13,
                    RequireJobPoint = 900,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 15,
                    RequireJobPoint = 1200,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 18,
                    RequireJobPoint = 1600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 20,
                    RequireJobPoint = 2000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 23,
                    RequireJobPoint = 2500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 27,
                    RequireJobPoint = 3400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 4400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8800,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 7, Job = JobId.HighScepter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 40,
                    RequireJobPoint = 0,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 45,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 50,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 56,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 62,
                    RequireJobPoint = 5500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 67,
                    RequireJobPoint = 7000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 8500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 10000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 11500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 14000,
                    IsRelease = true
                }
            }},
            new CDataSkillParam() {SkillNo = 8, Job = JobId.HighScepter, Params = new List<CDataSkillLevelParam>()
            {
                new CDataSkillLevelParam()
                {
                    Lv = 1,
                    RequireJobLevel = 25,
                    RequireJobPoint = 1500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 2,
                    RequireJobLevel = 27,
                    RequireJobPoint = 1800,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 3,
                    RequireJobLevel = 30,
                    RequireJobPoint = 2400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 4,
                    RequireJobLevel = 32,
                    RequireJobPoint = 3000,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 5,
                    RequireJobLevel = 35,
                    RequireJobPoint = 3600,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 6,
                    RequireJobLevel = 38,
                    RequireJobPoint = 4500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 7,
                    RequireJobLevel = 0,
                    RequireJobPoint = 5400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 8,
                    RequireJobLevel = 0,
                    RequireJobPoint = 6400,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 9,
                    RequireJobLevel = 0,
                    RequireJobPoint = 7500,
                    IsRelease = true
                },
                new CDataSkillLevelParam()
                {
                    Lv = 10,
                    RequireJobLevel = 0,
                    RequireJobPoint = 9500,
                    IsRelease = true
                }
            }}
        };
        
        public SkillGetAcquirableSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetAcquirableSkillListReq> packet)
        {
            client.Send(new S2CSkillGetAcquirableSkillListRes(){
                SkillParamList = AllSkills
                    .Where(x => x.Job == packet.Structure.Job).ToList()
            });
        }
    }
}
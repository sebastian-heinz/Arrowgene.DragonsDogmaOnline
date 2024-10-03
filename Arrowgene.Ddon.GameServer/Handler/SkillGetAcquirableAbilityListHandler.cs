using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetAcquirableAbilityListHandler : StructurePacketHandler<GameClient, C2SSkillGetAcquirableAbilityListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetAcquirableAbilityListHandler));

        public static readonly List<CDataAbilityParam> AllAbilities = new List<CDataAbilityParam>() {
            new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x4, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                           RequireJobLevel = 3,
                           RequireJobPoint = 300,
                           IsRelease = true
                    },
                    new CDataAbilityLevelParam()
                    {
                        Lv = 2,
                        RequireJobLevel = 6,
                        RequireJobPoint = 400,
                        IsRelease = true
                    },
                    new CDataAbilityLevelParam()
                    {
                        Lv = 3,
                        RequireJobLevel = 9,
                        RequireJobPoint = 500,
                        IsRelease = true
                    },
                    new CDataAbilityLevelParam()
                    {
                        Lv = 4,
                        RequireJobLevel = 18,
                        RequireJobPoint = 800,
                        IsRelease = true
                    },
                    new CDataAbilityLevelParam()
                    {
                        Lv = 5,
                        RequireJobLevel = 0,
                        RequireJobPoint = 1000,
                        IsRelease = true
                    },
                    new CDataAbilityLevelParam()
                    {
                        Lv = 6,
                        RequireJobLevel = 0,
                        RequireJobPoint = 2000,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0xF, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x5, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 9,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 13,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 17,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 23,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x1, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x9, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x10, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x6, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x19, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x17, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x18, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x16, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0xA, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 33,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 43,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x2, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 33,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 43,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0xC, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x11, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x13, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x8, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x12, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x3, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0xD, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x15, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0xB, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x7, Cost = 12, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0xE, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x14, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x128, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x129, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x12B, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x12A, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x12C, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x12D, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x12E, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x12F, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x131, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x130, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x132, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x133, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 60,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 65,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 70,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 77,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},

                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x6F, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x74, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x6D, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 9,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 13,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 17,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 23,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x65, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x7B, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x7C, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x6A, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x66, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x7D, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x78, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x79, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x75, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 33,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 43,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x68, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 33,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 43,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x70, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x77, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x7A, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x69, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x67, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x71, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x6E, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x73, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x76, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x6B, Cost = 11, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x6C, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x72, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x158, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x159, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x15C, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x15A, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x15B, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x15D, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x15E, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x15F, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x162, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x160, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x161, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x163, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 60,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 65,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 70,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 77,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},

                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x1B, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x22, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x2A, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 9,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 13,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 17,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 23,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x1A, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x1E, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x24, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x21, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x32, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x30, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x31, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x2F, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x1F, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 33,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 43,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x1D, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 33,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 43,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x23, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x27, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x2D, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x29, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x20, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x2B, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x1C, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x2E, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x26, Cost = 12, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x25, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x28, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x2C, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x134, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x135, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x137, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x136, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x138, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x139, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x13A, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x13B, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x13D, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x13C, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x13E, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x13F, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 60,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 65,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 70,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 77,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},

                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x3E, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x43, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x39, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 13,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 17,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 23,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x34, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x3F, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x44, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x3A, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x40, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x45, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x4B, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x49, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x4A, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x48, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x33, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 33,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 43,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x3D, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x41, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x42, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x37, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x38, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x46, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x47, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x35, Cost = 12, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x3C, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x36, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x3B, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x140, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x141, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x143, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x142, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x144, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x145, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x146, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x147, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x149, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x148, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x14A, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x14B, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 60,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 65,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 70,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 77,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},

                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x52, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x55, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x4F, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 9,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 13,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 17,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 23,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x51, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x59, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x5D, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x63, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x54, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x57, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x56, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x58, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x4D, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 33,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 43,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x50, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 33,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 43,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x5F, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x60, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x61, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x5C, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x4C, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x53, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x5A, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x5E, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x62, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x5B, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x4E, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x64, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x14C, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x14D, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x150, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x14E, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x14F, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x151, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x152, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x153, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x156, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x154, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x155, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x157, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 60,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 65,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 70,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 77,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},

                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x7E, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x8E, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 9,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 13,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 17,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 23,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x89, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x81, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x82, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x83, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x84, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x92, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x94, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x91, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x93, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x7F, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 33,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 43,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x80, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 33,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 43,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x86, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x8A, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x8F, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x95, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x85, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x88, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x8C, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x90, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x8D, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x87, Cost = 12, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x8B, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x96, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x164, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x165, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x16B, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x166, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x168, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x169, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x167, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x16A, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x16E, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x16C, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x16D, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x16F, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 60,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 65,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 70,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 77,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},

                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB5, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xBF, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB4, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 9,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 13,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 17,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 23,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC0, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 9,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 13,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 17,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 23,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB1, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB8, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xBC, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB2, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB9, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC8, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xBD, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC1, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC7, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB0, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB6, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 33,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 43,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB7, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xBB, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xBA, Cost = 11, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC3, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC4, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC5, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 7500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xBE, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB3, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC2, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC6, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x17C, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x17D, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x180, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x17E, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x17F, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x181, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x182, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x183, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x186, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x184, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x185, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x187, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},

                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x9F, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA6, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 9,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 13,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 17,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 23,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x9A, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA7, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA8, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA0, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA1, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA9, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xAD, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xAF, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA5, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xAC, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xAE, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x9C, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 33,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 35,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 38,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 43,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xAA, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xAB, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x97, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x99, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA3, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x9D, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA4, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x98, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA2, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x9B, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x9E, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x170, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x171, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x173, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x172, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x174, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x175, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x176, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x177, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x179, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x178, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x17A, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x17B, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},

                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xCD, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xCE, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 9,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 13,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 17,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 23,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD6, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 9,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 13,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 17,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 23,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xDA, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 13,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 17,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 23,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xCB, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD0, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xDB, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xCC, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD3, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xDE, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xCF, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD8, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xDF, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xCA, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD1, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD4, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD2, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 33,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 35,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 38,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 43,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD9, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xDC, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xC9, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD5, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xE0, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD7, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xDD, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xE1, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x188, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x189, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x18C, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x18A, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x18B, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x18D, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x18E, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x18F, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x192, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x190, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x191, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x193, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 60,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 65,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 70,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 77,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},

                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x195, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x19F, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 13,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 17,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 23,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A2, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x196, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x194, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x19B, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A1, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x19D, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x198, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A0, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x199, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x19E, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x19A, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B1, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A6, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A7, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A8, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1AB, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x19C, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A9, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1AA, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x197, Cost = 11, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 53,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A4, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 53,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A5, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 53,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1AC, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 53,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1AD, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1AE, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B0, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B2, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1AF, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B7, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B3, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B4, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B6, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A3, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B5, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B8, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 60,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 65,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 70,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 77,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},

                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D3, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 3,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 6,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 18,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D9, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 9,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 13,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 17,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 23,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1DE, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 13,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 15,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 27,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D5, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 20,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 22,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 25,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D4, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E3, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E5, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D8, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 25,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 27,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 30,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 35,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1DF, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D2, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 300,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E4, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 30,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 32,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 40,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E6, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D6, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D7, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 35,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 38,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 43,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 45,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E2, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E0, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E9, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E1, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 43,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E8, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1EA, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1DD, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 48,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 51,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 56,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1DB, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 53,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E7, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 53,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1DA, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 53,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 56,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F3, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F4, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 45,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 50,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 57,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F6, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F5, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F1, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F8, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 50,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 55,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 62,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1ED, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1EE, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 50,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 55,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 67,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1EF, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F0, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F2, Cost = 9, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F7, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 55,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 60,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 65,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 72,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 8000,
                            IsRelease = true
                        }
                    }}
        };

        public static readonly List<CDataAbilityParam> AllSecretAbilities = new List<CDataAbilityParam>() {
            new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE7, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                           RequireJobLevel = 1,
                           RequireJobPoint = 0,
                           IsRelease = true
                    },
                    new CDataAbilityLevelParam()
                    {
                        Lv = 2,
                        RequireJobLevel = 5,
                        RequireJobPoint = 400,
                        IsRelease = true
                    },
                    new CDataAbilityLevelParam()
                    {
                        Lv = 3,
                        RequireJobLevel = 10,
                        RequireJobPoint = 500,
                        IsRelease = true
                    },
                    new CDataAbilityLevelParam()
                    {
                        Lv = 4,
                        RequireJobLevel = 20,
                        RequireJobPoint = 800,
                        IsRelease = true
                    },
                    new CDataAbilityLevelParam()
                    {
                        Lv = 5,
                        RequireJobLevel = 30,
                        RequireJobPoint = 1000,
                        IsRelease = true
                    },
                    new CDataAbilityLevelParam()
                    {
                        Lv = 6,
                        RequireJobLevel = 40,
                        RequireJobPoint = 2000,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE8, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xEC, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE4, Cost = 6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xEB, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xED, Cost = 7, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF0, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF1, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF4, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF5, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE5, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE6, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE9, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF2, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF3, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE2, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE3, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xEE, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xEF, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xEA, Cost = 8, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF6, Cost = 10, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 0,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 0,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 0,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 0,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 0,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF7, Cost = 0, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 0,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 0,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 0,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 0,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 0,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 0,
                            RequireJobPoint = 0,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF8, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF9, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xFA, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xFB, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xFC, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xFD, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xFE, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xFF, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x100, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x101, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x102, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x103, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x104, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x105, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x106, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 600,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 3500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x107, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x108, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x109, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x10A, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x10B, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x10C, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x10D, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x10E, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x10F, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x110, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x111, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x112, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x113, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x114, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x115, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x116, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x117, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x118, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x119, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x11A, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x11B, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x11C, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 400,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 800,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x11D, Cost = 15, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 300,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x11E, Cost = 10, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 300,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x11F, Cost = 8, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 300,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x120, Cost = 10, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 300,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x121, Cost = 15, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 100,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x122, Cost = 10, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 100,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x123, Cost = 1, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x124, Cost = 1, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x125, Cost = 1, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x126, Cost = 1, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x127, Cost = 1, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1B9, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1BA, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1BB, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1BC, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1BD, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1BE, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1BF, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C0, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C1, Cost = 8, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C2, Cost = 5, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C3, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C4, Cost = 3, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C5, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 1500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 4000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 5000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C6, Cost = 3, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C7, Cost = 2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C8, Cost = 1, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C9, Cost = 1, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1CA, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1CB, Cost = 5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1CC, Cost = 4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 2,
                            RequireJobLevel = 5,
                            RequireJobPoint = 500,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 3,
                            RequireJobLevel = 10,
                            RequireJobPoint = 1000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 4,
                            RequireJobLevel = 20,
                            RequireJobPoint = 2000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 5,
                            RequireJobLevel = 30,
                            RequireJobPoint = 3000,
                            IsRelease = true
                        },
                        new CDataAbilityLevelParam()
                        {
                            Lv = 6,
                            RequireJobLevel = 40,
                            RequireJobPoint = 4500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1CD, Cost = 12, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 0,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1CE, Cost = 12, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 0,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1CF, Cost = 12, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 0,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1D0, Cost = 12, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 0,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }}
        };

        public static CDataAbilityParam GetAbilityFromId(uint abilityId)
        {
            var abilities = AllAbilities.Concat(AllSecretAbilities);
            return abilities.Where(x => x.AbilityNo == abilityId).FirstOrDefault();
        }

        private IDatabase _Database;

        public SkillGetAcquirableAbilityListHandler(DdonGameServer server) : base(server)
        {
            _Database = server.Database;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetAcquirableAbilityListReq> packet)
        {
            S2CSkillGetAcquirableAbilityListRes Response = new S2CSkillGetAcquirableAbilityListRes();
            if (packet.Structure.Job != 0)
            {
                Response.AbilityParamList = AllAbilities
                    .Where(x => x.Job == packet.Structure.Job).ToList();
            }
            else if (packet.Structure.CharacterId == 0)
            {
                // Player characters come in as CharacterId == 0.
                // Pawns seem to not need the information from this query. The UI still is populated by the skills
                // acquired by the player character (is this intended?).
                List<SecretAbility> UnlockedAbilities = _Database.SelectAllUnlockedSecretAbilities(client.Character.CommonId);
                Response.AbilityParamList = AllSecretAbilities.Where(x => UnlockedAbilities.Contains((SecretAbility)x.AbilityNo)).ToList();
            }

            client.Send(Response);
        }
    }
}

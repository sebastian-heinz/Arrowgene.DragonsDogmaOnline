using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetAcquirableAbilityListHandler : StructurePacketHandler<GameClient, C2SSkillGetAcquirableAbilityListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetAcquirableAbilityListHandler));

        public static readonly List<CDataAbilityParam> AllAbilities = new List<CDataAbilityParam>() {
            new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x4, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0xF, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x5, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x1, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x9, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x10, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x6, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x19, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x17, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x18, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x16, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0xA, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x2, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0xC, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x11, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x13, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x8, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x12, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x3, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0xD, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x15, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0xB, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x7, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0xE, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x14, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x128, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x129, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x12B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x12A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x12C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x12D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x12E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x12F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x131, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x130, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x132, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Fighter, AbilityNo=0x133, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x6F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x74, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x6D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x65, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x7B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x7C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x6A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x66, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x7D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x78, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x79, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x75, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x68, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x70, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x77, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x7A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x69, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x67, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x71, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x6E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x73, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x76, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x6B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x6C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x72, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x158, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x159, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x15C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x15A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x15B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x15D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x15E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x15F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x162, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x160, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x161, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Seeker, AbilityNo=0x163, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x1B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x22, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x2A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x1A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x1E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x24, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x21, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x32, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x30, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x31, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x2F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x1F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x1D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x23, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x27, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x2D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x29, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x20, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x2B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x1C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x2E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x26, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x25, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x28, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x2C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x134, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x135, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x137, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x136, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x138, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x139, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x13A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x13B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x13D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x13C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x13E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Hunter, AbilityNo=0x13F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x3E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x43, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x39, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x34, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x3F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x44, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x3A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x40, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x45, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x4B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x49, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x4A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x48, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x33, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x3D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x41, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x42, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x37, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x38, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x46, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x47, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x35, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x3C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x36, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x3B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x140, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x141, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x143, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x142, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x144, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x145, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x146, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x147, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x149, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x148, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x14A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Priest, AbilityNo=0x14B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x52, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x55, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x4F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x51, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x59, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x5D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x63, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x54, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x57, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x56, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x58, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x4D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x50, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x5F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x60, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x61, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x5C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x4C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x53, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x5A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x5E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x62, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x5B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x4E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x64, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x14C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x14D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x150, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x14E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x14F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x151, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x152, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x153, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x156, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x154, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x155, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ShieldSage, AbilityNo=0x157, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x7E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x8E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x89, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x81, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x82, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x83, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x84, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x92, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x94, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x91, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x93, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x7F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x80, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x86, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x8A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x8F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x95, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x85, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x88, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x8C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x90, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x8D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x87, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x8B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x96, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x164, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x165, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x16B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x166, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x168, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x169, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x167, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x16A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x16E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x16C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x16D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Sorcerer, AbilityNo=0x16F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB5, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xBF, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB4, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC0, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB1, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB8, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xBC, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB2, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB9, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC8, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xBD, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC1, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC7, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB0, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB6, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB7, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xBB, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xBA, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC3, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC4, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC5, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 40,
                            RequireJobPoint = 7500,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xBE, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xB3, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC2, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0xC6, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 45,
                            RequireJobPoint = 6000,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x17C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x17D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x180, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x17E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x17F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x181, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x182, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x183, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x186, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x184, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x185, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Warrior, AbilityNo=0x187, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x9F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA6, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x9A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA7, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA8, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA0, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA1, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA9, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xAD, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xAF, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA5, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xAC, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xAE, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x9C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xAA, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xAB, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x97, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x99, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA3, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x9D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA4, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x98, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0xA2, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x9B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x9E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x170, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x171, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x173, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x172, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x174, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x175, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x176, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x177, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x179, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x178, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x17A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.ElementArcher, AbilityNo=0x17B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xCD, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xCE, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD6, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xDA, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xCB, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD0, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xDB, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xCC, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD3, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xDE, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xCF, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD8, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xDF, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xCA, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD1, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD4, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD2, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD9, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xDC, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xC9, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD5, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xE0, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xD7, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xDD, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0xE1, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x188, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x189, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x18C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x18A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x18B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x18D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x18E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x18F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x192, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x190, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x191, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.Alchemist, AbilityNo=0x193, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x195, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x19F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A2, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x196, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x194, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x19B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A1, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x19D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x198, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A0, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x199, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x19E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x19A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B1, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A6, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A7, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A8, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1AB, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x19C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A9, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1AA, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x197, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A4, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A5, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1AC, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1AD, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1AE, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B0, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B2, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1AF, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B7, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B3, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B4, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B6, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1A3, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B5, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.SpiritLancer, AbilityNo=0x1B8, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D3, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D9, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1DE, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D5, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D4, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E3, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E5, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D8, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1DF, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D2, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E4, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E6, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D6, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1D7, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E2, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E0, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E9, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E1, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E8, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1EA, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1DD, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1DB, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1E7, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1DA, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F3, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F4, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F6, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F5, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F1, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F8, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1ED, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1EE, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1EF, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F0, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F2, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=JobId.HighScepter, AbilityNo=0x1F7, Params = new List<CDataAbilityLevelParam>()
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
            new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE7, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE8, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xEC, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE4, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xEB, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xED, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF0, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF1, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF4, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF5, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE5, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE6, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE9, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF2, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF3, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE2, Params = new List<CDataAbilityLevelParam>()
                    {
                        new CDataAbilityLevelParam()
                        {
                            Lv = 1,
                            RequireJobLevel = 1,
                            RequireJobPoint = 0,
                            IsRelease = true
                        }
                    }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xE3, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xEE, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xEF, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xEA, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF6, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF7, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF8, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xF9, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xFA, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xFB, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xFC, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xFD, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xFE, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0xFF, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x100, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x101, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x102, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x103, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x104, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x105, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x106, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x107, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x108, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x109, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x10A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x10B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x10C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x10D, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x10E, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x10F, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x110, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x111, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x112, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x113, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x114, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x115, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x116, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x117, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x118, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x119, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x11A, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x11B, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x11C, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x11D, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 300,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x11E, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 300,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x11F, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 300,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x120, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 300,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x121, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 100,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x122, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 100,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x123, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x124, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x125, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x126, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x127, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1B9, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1BA, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1BB, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1BC, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1BD, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1BE, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1BF, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C0, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C1, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C2, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C3, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C4, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C5, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C6, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C7, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C8, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1C9, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 1,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1CA, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1CB, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1CC, Params = new List<CDataAbilityLevelParam>()
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
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1CD, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 0,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1CE, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 0,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1CF, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 0,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }},
                new CDataAbilityParam() {Type=1, Job=0, AbilityNo=0x1D0, Params = new List<CDataAbilityLevelParam>() {
                    new CDataAbilityLevelParam()
                    {
                        Lv = 1,
                        RequireJobLevel = 0,
                        RequireJobPoint = 0,
                        IsRelease = true
                    }
                }}
        };

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
            else
            {
                uint commonId = 0;
                uint characterId = packet.Structure.CharacterId;

                // If the CharacterId is 0, it is the current player, otherwise it is a PawnId
                if (characterId == 0)
                {
                    commonId = client.Character.CommonId;
                }
                else
                {
                    Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == characterId).Single();
                    commonId = pawn.CommonId;
                }

                List<SecretAbility> UnlockedAbilities = _Database.SelectAllUnlockedSecretAbilities(commonId);
                Response.AbilityParamList = AllSecretAbilities.Where(x => UnlockedAbilities.Contains((SecretAbility)x.AbilityNo)).ToList();
            }

            client.Send(Response);
        }
    }
}

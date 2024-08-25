using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler;

public class AchievementGetProgressListHandler : GameRequestPacketHandler<C2SAchievementGetProgressListReq, S2CAchievementGetProgressListRes>
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AchievementGetProgressListHandler));

    private static readonly List<CDataAchievementProgress> AchievementProgressList = new()
    {
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 450,
                Index = 0
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 583,
                Index = 29
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 519,
                Index = 1
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 983,
                Index = 187
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 584,
                Index = 30
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 919,
                Index = 138
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 520,
                Index = 2
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 585,
                Index = 31
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 521,
                Index = 3
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 522,
                Index = 4
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 523,
                Index = 5
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 524,
                Index = 6
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 525,
                Index = 7
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2532,
                Index = 980
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 981,
                Index = 185
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 590,
                Index = 34
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 917,
                Index = 136
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2989,
                Index = 1857
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 526,
                Index = 8
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2856,
                Index = 1725
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 591,
                Index = 35
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 527,
                Index = 9
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 863,
                Index = 40
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 592,
                Index = 36
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2313,
                Index = 976
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 528,
                Index = 10
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 529,
                Index = 11
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2536,
                Index = 1019
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 530, // Bounty Hunter, furniture-related, Mini Table
                Index = 12
            },
            CurrentNum = 100,
            Sequence = 0,
            CompleteDate = 1550409326
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 531,
                Index = 13
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1927,
                Index = 387
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 532,
                Index = 14
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 915,
                Index = 116
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2987,
                Index = 1855
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 533,
                Index = 15
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 534,
                Index = 16
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 535,
                Index = 17
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 999,
                Index = 194
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 536,
                Index = 18
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 537,
                Index = 19
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1260,
                Index = 363
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 865,
                Index = 42
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2889,
                Index = 1765
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2296,
                Index = 952
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 538,
                Index = 20
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 539,
                Index = 21
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1935,
                Index = 397
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 540,
                Index = 22
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 641,
                Index = 64
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1163,
                Index = 319
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2328,
                Index = 994
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1292,
                Index = 23
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 541,
                Index = 24
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1929,
                Index = 389
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 542,
                Index = 25
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 997,
                Index = 192
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 543,
                Index = 26
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 608,
                Index = 37
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 544,
                Index = 27
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 973,
                Index = 177
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2339,
                Index = 1005
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 582,
                Index = 28
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1295,
                Index = 32
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 637,
                Index = 63
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1296,
                Index = 33
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 609,
                Index = 38
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 873,
                Index = 44
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1001,
                Index = 196
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 610,
                Index = 39
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 623,
                Index = 52
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 864,
                Index = 41
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2888,
                Index = 1764
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 631,
                Index = 57
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 872,
                Index = 43
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 625,
                Index = 54
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 753,
                Index = 125
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 874,
                Index = 45
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2898,
                Index = 1774
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1276,
                Index = 371
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 881,
                Index = 46
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 633,
                Index = 59
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 882,
                Index = 47
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1250,
                Index = 353
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 883,
                Index = 48
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 899,
                Index = 49
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2907,
                Index = 1783
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1081,
                Index = 400
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 900,
                Index = 50
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 901,
                Index = 51
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2909,
                Index = 1785
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1023,
                Index = 215
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 624,
                Index = 53
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2345,
                Index = 1010
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 629,
                Index = 55
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1021,
                Index = 213
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 630,
                Index = 56
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 967,
                Index = 171
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 632,
                Index = 58
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 961,
                Index = 165
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2343,
                Index = 1008
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 634,
                Index = 60
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 635,
                Index = 61
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1019,
                Index = 211
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 636,
                Index = 62
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1299,
                Index = 147
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 642,
                Index = 65
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 643,
                Index = 66
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 650,
                Index = 67
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 651,
                Index = 68
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 652,
                Index = 69
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 774,
                Index = 128
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 653,
                Index = 70
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 654,
                Index = 71
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 655,
                Index = 72
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 662,
                Index = 73
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 663,
                Index = 74
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 664,
                Index = 75
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2305,
                Index = 961
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1297,
                Index = 145
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 668,
                Index = 76
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 669,
                Index = 77
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 670,
                Index = 78
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 665,
                Index = 79
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 666,
                Index = 80
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 796,
                Index = 123
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 667,
                Index = 81
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 671,
                Index = 82
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 672,
                Index = 83
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2329,
                Index = 995
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1846,
                Index = 377
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 673,
                Index = 84
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 677,
                Index = 85
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2307,
                Index = 963
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 678,
                Index = 86
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 679,
                Index = 87
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 683,
                Index = 88
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 684,
                Index = 89
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 685,
                Index = 90
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 689,
                Index = 91
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 690,
                Index = 92
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 820,
                Index = 132
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 691,
                Index = 93
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 692,
                Index = 94
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 693,
                Index = 95
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2323,
                Index = 989
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 694,
                Index = 96
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 836,
                Index = 133
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 707,
                Index = 97
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 708,
                Index = 98
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 709,
                Index = 99
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 713,
                Index = 100
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 714,
                Index = 101
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 715,
                Index = 102
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 719,
                Index = 103
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 720,
                Index = 104
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 721,
                Index = 105
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1086,
                Index = 248
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 728,
                Index = 106
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 729,
                Index = 107
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 730,
                Index = 108
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 725,
                Index = 109
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 726,
                Index = 110
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1077,
                Index = 390
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 727,
                Index = 111
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 731,
                Index = 112
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 732,
                Index = 113
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 733,
                Index = 114
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1123,
                Index = 282
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 914,
                Index = 115
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1033,
                Index = 231
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 916,
                Index = 117
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 794,
                Index = 121
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2882,
                Index = 1758
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 737,
                Index = 118
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 738,
                Index = 119
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 739,
                Index = 120
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1162,
                Index = 318
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 795,
                Index = 122
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 752,
                Index = 124
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 754,
                Index = 126
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1112,
                Index = 271
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 773,
                Index = 127
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 775,
                Index = 129
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 818,
                Index = 130
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 819,
                Index = 131
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2525,
                Index = 965
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1176,
                Index = 332
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 837,
                Index = 134
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 838,
                Index = 135
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1087,
                Index = 249
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 918,
                Index = 137
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 929,
                Index = 139
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 930,
                Index = 140
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1234,
                Index = 340
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 931,
                Index = 141
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 935,
                Index = 142
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1924,
                Index = 383
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1045,
                Index = 237
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 936,
                Index = 143
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1925,
                Index = 384
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 937,
                Index = 144
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2881,
                Index = 1757
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2288,
                Index = 941
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1177,
                Index = 333
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2310,
                Index = 973
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1298,
                Index = 146
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1926,
                Index = 386
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1083,
                Index = 245
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 938,
                Index = 148
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 939,
                Index = 149
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 940,
                Index = 150
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1932,
                Index = 393
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 944,
                Index = 151
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1933,
                Index = 394
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 945,
                Index = 152
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1934,
                Index = 396
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1027,
                Index = 222
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 946,
                Index = 153
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 947,
                Index = 154
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 948,
                Index = 155
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 949,
                Index = 156
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2893,
                Index = 1769
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2300,
                Index = 956
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1941,
                Index = 404
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 953,
                Index = 157
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1035,
                Index = 233
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 954,
                Index = 158
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 955,
                Index = 159
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1073,
                Index = 375
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 956,
                Index = 160
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1232,
                Index = 338
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 957,
                Index = 161
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2902,
                Index = 1778
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1063,
                Index = 240
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 958,
                Index = 162
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1931,
                Index = 392
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 959,
                Index = 163
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 960,
                Index = 164
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1107,
                Index = 266
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 962,
                Index = 166
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2971,
                Index = 1839
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 963,
                Index = 167
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1145,
                Index = 301
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 964,
                Index = 168
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2973,
                Index = 1841
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 965,
                Index = 169
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1135,
                Index = 293
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 966,
                Index = 170
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 968,
                Index = 172
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 969,
                Index = 173
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1115,
                Index = 274
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 970,
                Index = 174
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2344,
                Index = 1009
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 971,
                Index = 175
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 972,
                Index = 176
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1143,
                Index = 299
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 974,
                Index = 178
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 975,
                Index = 179
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 976,
                Index = 180
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 977,
                Index = 181
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2359,
                Index = 1024
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 978,
                Index = 182
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 979,
                Index = 183
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1097,
                Index = 256
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 980,
                Index = 184
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1151,
                Index = 307
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 982,
                Index = 186
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 984,
                Index = 188
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1157,
                Index = 313
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 985,
                Index = 189
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 995,
                Index = 190
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1928,
                Index = 388
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 996,
                Index = 191
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1930,
                Index = 391
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 998,
                Index = 193
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1109,
                Index = 268
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1000,
                Index = 195
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1147,
                Index = 303
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1002,
                Index = 197
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1003,
                Index = 198
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1936,
                Index = 398
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1121,
                Index = 280
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1004,
                Index = 199
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1937,
                Index = 399
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1005,
                Index = 200
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1938,
                Index = 401
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1006,
                Index = 201
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1091,
                Index = 250
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1010,
                Index = 202
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1011,
                Index = 203
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2955,
                Index = 1823
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1129,
                Index = 288
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1012,
                Index = 204
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1013,
                Index = 205
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2957,
                Index = 1825
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1014,
                Index = 206
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1015,
                Index = 207
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1016,
                Index = 208
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1017,
                Index = 209
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1099,
                Index = 258
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1018,
                Index = 210
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1137,
                Index = 295
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2350,
                Index = 1015
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1020,
                Index = 212
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2966,
                Index = 1834
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1127,
                Index = 286
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1022,
                Index = 214
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1024,
                Index = 216
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1171,
                Index = 327
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1300,
                Index = 217
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1301,
                Index = 218
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1302,
                Index = 219
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1025,
                Index = 220
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1026,
                Index = 221
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1303,
                Index = 223
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1255,
                Index = 358
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2316,
                Index = 983
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1304,
                Index = 224
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1305,
                Index = 225
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1028,
                Index = 226
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1029,
                Index = 227
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1030,
                Index = 228
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1922,
                Index = 381
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2998,
                Index = 1866
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1031,
                Index = 229
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1032,
                Index = 230
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1034,
                Index = 232
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1036,
                Index = 234
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2535,
                Index = 1018
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1043,
                Index = 235
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1044,
                Index = 236
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1061,
                Index = 238
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1062,
                Index = 239
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1070,
                Index = 241
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1071,
                Index = 242
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2547,
                Index = 1316
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1072,
                Index = 243
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1082,
                Index = 244
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1084,
                Index = 246
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1085,
                Index = 247
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1092,
                Index = 251
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1093,
                Index = 252
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1094,
                Index = 253
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2521,
                Index = 942
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1095,
                Index = 254
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1096,
                Index = 255
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1098,
                Index = 257
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1100,
                Index = 259
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1101,
                Index = 260
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1102,
                Index = 261
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1103,
                Index = 262
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2308,
                Index = 971
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1104,
                Index = 263
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1105,
                Index = 264
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2318,
                Index = 984
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1106,
                Index = 265
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1108,
                Index = 267
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2855,
                Index = 1724
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1110,
                Index = 269
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1111,
                Index = 270
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1113,
                Index = 272
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1114,
                Index = 273
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1116,
                Index = 275
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1117,
                Index = 276
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2338,
                Index = 1004
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1118,
                Index = 277
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2529,
                Index = 969
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1119,
                Index = 278
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2324,
                Index = 990
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1120,
                Index = 279
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1122,
                Index = 281
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1124,
                Index = 283
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1940,
                Index = 403
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1125,
                Index = 284
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1126,
                Index = 285
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1128,
                Index = 287
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1130,
                Index = 289
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1132,
                Index = 290
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1133,
                Index = 291
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1134,
                Index = 292
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1136,
                Index = 294
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2541,
                Index = 1310
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1138,
                Index = 296
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1139,
                Index = 297
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1140,
                Index = 298
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2539,
                Index = 1308
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1144,
                Index = 300
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1146,
                Index = 302
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1148,
                Index = 304
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1149,
                Index = 305
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1150,
                Index = 306
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1152,
                Index = 308
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1153,
                Index = 309
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1154,
                Index = 310
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1155,
                Index = 311
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1156,
                Index = 312
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1158,
                Index = 314
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1159,
                Index = 315
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2348,
                Index = 1013
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1160,
                Index = 316
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1161,
                Index = 317
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2358,
                Index = 1023
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1164,
                Index = 320
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1165,
                Index = 321
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2322,
                Index = 988
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1166,
                Index = 322
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1167,
                Index = 323
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1168,
                Index = 324
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1169,
                Index = 325
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1170,
                Index = 326
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1172,
                Index = 328
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2527,
                Index = 967
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1173,
                Index = 329
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2919,
                Index = 1795
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1174,
                Index = 330
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1175,
                Index = 331
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1228,
                Index = 334
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2386,
                Index = 1045
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1229,
                Index = 335
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1230,
                Index = 336
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1231,
                Index = 337
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1233,
                Index = 339
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1235,
                Index = 341
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2320,
                Index = 986
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1236,
                Index = 342
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1237,
                Index = 343
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2314,
                Index = 977
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2983,
                Index = 1851
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1238,
                Index = 344
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2299,
                Index = 955
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1239,
                Index = 345
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1243,
                Index = 346
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1244,
                Index = 347
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1245,
                Index = 348
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1246,
                Index = 349
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1247,
                Index = 350
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1248,
                Index = 351
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1249,
                Index = 352
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1251,
                Index = 354
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2336,
                Index = 1002
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1252,
                Index = 355
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1253,
                Index = 356
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2935,
                Index = 1814
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1254,
                Index = 357
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1256,
                Index = 359
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1257,
                Index = 360
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2326,
                Index = 992
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1258,
                Index = 361
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1259,
                Index = 362
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1261,
                Index = 364
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2863,
                Index = 1732
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1262,
                Index = 365
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1263,
                Index = 366
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1264,
                Index = 367
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1265,
                Index = 368
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1266,
                Index = 369
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1267,
                Index = 370
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2352,
                Index = 1017
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2370,
                Index = 1031
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1277,
                Index = 372
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2879,
                Index = 1755
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1278,
                Index = 373
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1279,
                Index = 374
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1845,
                Index = 376
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1847,
                Index = 378
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1848,
                Index = 379
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1074,
                Index = 380
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1923,
                Index = 382
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1076,
                Index = 385
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2982,
                Index = 1850
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1079,
                Index = 395
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1939,
                Index = 402
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1291,
                Index = 405
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2293,
                Index = 948
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2291,
                Index = 945
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2301,
                Index = 957
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2850,
                Index = 1719
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2534,
                Index = 982
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2355,
                Index = 1022
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2297,
                Index = 953
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2285,
                Index = 937
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2287,
                Index = 940
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2289,
                Index = 943
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2303,
                Index = 959
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2857,
                Index = 1728
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2873,
                Index = 1749
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2866,
                Index = 1735
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2530,
                Index = 970
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2526,
                Index = 966
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2542,
                Index = 1311
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2874,
                Index = 1750
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2904,
                Index = 1780
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2319,
                Index = 985
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2984,
                Index = 1852
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2978,
                Index = 1846
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2335,
                Index = 1001
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2315,
                Index = 978
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2848,
                Index = 1717
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2936,
                Index = 1815
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2930,
                Index = 1809
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2351,
                Index = 1016
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2331,
                Index = 997
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2864,
                Index = 1731
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2858,
                Index = 1727
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2375,
                Index = 1036
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2347,
                Index = 1012
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2327,
                Index = 993
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2522,
                Index = 946
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2920,
                Index = 1796
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2914,
                Index = 1790
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2872,
                Index = 1748
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2311,
                Index = 974
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2290,
                Index = 944
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2528,
                Index = 968
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2286,
                Index = 938
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2524,
                Index = 964
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2520,
                Index = 939
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2292,
                Index = 947
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2885,
                Index = 1761
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2294,
                Index = 949
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2523,
                Index = 950
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2295,
                Index = 951
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2298,
                Index = 954
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2302,
                Index = 958
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2304,
                Index = 960
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2306,
                Index = 962
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2309,
                Index = 972
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2312,
                Index = 975
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2531,
                Index = 979
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2533,
                Index = 981
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2321,
                Index = 987
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2325,
                Index = 991
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2330,
                Index = 996
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2332,
                Index = 998
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2333,
                Index = 999
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2334,
                Index = 1000
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2337,
                Index = 1003
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2341,
                Index = 1006
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2342,
                Index = 1007
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2346,
                Index = 1011
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2349,
                Index = 1014
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2353,
                Index = 1020
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2354,
                Index = 1021
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2360,
                Index = 1025
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2362,
                Index = 1026
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2363,
                Index = 1027
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2364,
                Index = 1028
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2368,
                Index = 1029
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2369,
                Index = 1030
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2371,
                Index = 1032
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2372,
                Index = 1033
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2373,
                Index = 1034
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2374,
                Index = 1035
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2376,
                Index = 1037
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2377,
                Index = 1038
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2378,
                Index = 1039
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2379,
                Index = 1040
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2380,
                Index = 1041
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2381,
                Index = 1042
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2384,
                Index = 1043
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2385,
                Index = 1044
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2387,
                Index = 1046
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2992,
                Index = 1860
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2996,
                Index = 1864
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2944,
                Index = 1745
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2948,
                Index = 1806
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2952,
                Index = 1820
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2956,
                Index = 1824
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2896,
                Index = 1772
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2900,
                Index = 1776
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2908,
                Index = 1784
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2976,
                Index = 1844
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2980,
                Index = 1848
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2988,
                Index = 1856
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2865,
                Index = 1734
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2877,
                Index = 1753
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2897,
                Index = 1773
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2901,
                Index = 1777
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2905,
                Index = 1781
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2977,
                Index = 1845
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2981,
                Index = 1849
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2985,
                Index = 1853
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2929,
                Index = 1808
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2933,
                Index = 1812
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2937,
                Index = 1816
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2537,
                Index = 1306
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2538,
                Index = 1307
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2540,
                Index = 1309
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2543,
                Index = 1312
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2544,
                Index = 1313
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2545,
                Index = 1314
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2546,
                Index = 1315
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2548,
                Index = 1317
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2849,
                Index = 1718
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2851,
                Index = 1720
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2854,
                Index = 1721
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2853,
                Index = 1722
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2852,
                Index = 1723
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2859,
                Index = 1726
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2860,
                Index = 1729
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2861,
                Index = 1730
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2862,
                Index = 1733
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2868,
                Index = 1736
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2867,
                Index = 1737
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2870,
                Index = 1738
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2871,
                Index = 1739
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2939,
                Index = 1740
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2940,
                Index = 1741
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2941,
                Index = 1742
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2942,
                Index = 1743
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2943,
                Index = 1744
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2945,
                Index = 1746
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2946,
                Index = 1747
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2875,
                Index = 1751
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2876,
                Index = 1752
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2878,
                Index = 1754
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2880,
                Index = 1756
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2883,
                Index = 1759
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2884,
                Index = 1760
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2886,
                Index = 1762
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2887,
                Index = 1763
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2890,
                Index = 1766
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2891,
                Index = 1767
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2892,
                Index = 1768
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2894,
                Index = 1770
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2895,
                Index = 1771
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2899,
                Index = 1775
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2903,
                Index = 1779
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2906,
                Index = 1782
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2910,
                Index = 1786
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2911,
                Index = 1787
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2912,
                Index = 1788
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2913,
                Index = 1789
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2915,
                Index = 1791
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2916,
                Index = 1792
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2917,
                Index = 1793
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2918,
                Index = 1794
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2921,
                Index = 1797
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2922,
                Index = 1798
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2923,
                Index = 1799
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2924,
                Index = 1800
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2925,
                Index = 1801
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2926,
                Index = 1802
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2927,
                Index = 1803
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2928,
                Index = 1804
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2947,
                Index = 1805
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2949,
                Index = 1807
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2931,
                Index = 1810
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2932,
                Index = 1811
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2934,
                Index = 1813
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2938,
                Index = 1817
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2950,
                Index = 1818
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2951,
                Index = 1819
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2953,
                Index = 1821
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2954,
                Index = 1822
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2958,
                Index = 1826
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2959,
                Index = 1827
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2960,
                Index = 1828
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2961,
                Index = 1829
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2962,
                Index = 1830
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2963,
                Index = 1831
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2964,
                Index = 1832
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2965,
                Index = 1833
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2967,
                Index = 1835
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2968,
                Index = 1836
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2969,
                Index = 1837
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2970,
                Index = 1838
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2972,
                Index = 1840
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2974,
                Index = 1842
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2975,
                Index = 1843
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2979,
                Index = 1847
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2986,
                Index = 1854
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2990,
                Index = 1858
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2991,
                Index = 1859
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2993,
                Index = 1861
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2994,
                Index = 1862
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2995,
                Index = 1863
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 2997,
                Index = 1865
            },
            CurrentNum = 0,
            Sequence = 0,
            CompleteDate = 0
        }
    };

    public AchievementGetProgressListHandler(DdonGameServer server) : base(server)
    {
    }

    public override S2CAchievementGetProgressListRes Handle(GameClient client, C2SAchievementGetProgressListReq request)
    {
        S2CAchievementGetProgressListRes res = new S2CAchievementGetProgressListRes();

        // TODO: given an asset with all achievements, for each character store the progress in a DB table and retrieve progress here
        res.AchievementProgressList = AchievementProgressList;

        return res;
    }
}

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
                UId = 450, // Hunter became Job Level 11, incomplete
                Index = 0
            },
            CurrentNum = 1,
            Sequence = 0,
            CompleteDate = 0
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 1267, // Master Artisan's Touch furniture-related, complete, reward not received
                Index = 370
            },
            CurrentNum = 10,
            Sequence = 0,
            CompleteDate = 1561743012
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 530, // Bounty Hunter furniture-related, complete, reward received
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
                UId = 2532, // furniture-related, All You Need Is Love, complete, reward received
                Index = 980
            },
            CurrentNum = 1,
            Sequence = 0,
            CompleteDate = 1570993256
        },
        new()
        {
            AchieveIdentifier = new CDataAchievementIdentifier
            {
                UId = 981, // 羽ばたく悪夢Ⅱ / Flapping Wings Nightmare II
                Index = 185
            },
            CurrentNum = 9,
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

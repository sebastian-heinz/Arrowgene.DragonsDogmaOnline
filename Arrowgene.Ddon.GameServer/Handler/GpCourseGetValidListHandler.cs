#nullable enable
using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler;

public class GpCourseGetValidListHandler : GameRequestPacketHandler<C2SGpCourseGetValidListReq, S2CGpCourseGetValidListRes>
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpCourseGetValidListHandler));

    public GpCourseGetValidListHandler(DdonGameServer server) : base(server)
    {
    }

    public override S2CGpCourseGetValidListRes Handle(GameClient client, C2SGpCourseGetValidListReq request)
    {
        var res = new S2CGpCourseGetValidListRes();

        // TODO: track active courses in DB
        var offset = DateTimeOffset.UtcNow;
        res.Items.Add(new CDataGPCourseValid
        {
            Id = 1,
            CourseId = 1,
            Name = "Adventure Passport (active)",
            ImageAddr = "http://localhost:52099/shop/img/payment/icon_course1.png",
            StartTime = (ulong)offset.ToUnixTimeSeconds(),
            EndTime = (ulong)offset.AddMonths(12).ToUnixTimeSeconds()
        });

        return res;
    }
}

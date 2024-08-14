#nullable enable
using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler;

public class GpCourseUseFromAvailableHandler : GameRequestPacketHandler<C2SGpCourseUseFromAvailableReq, S2CGpCourseUseFromAvailableRes>
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpCourseUseFromAvailableHandler));

    public GpCourseUseFromAvailableHandler(DdonGameServer server) : base(server)
    {
    }

    public override S2CGpCourseUseFromAvailableRes Handle(GameClient client, C2SGpCourseUseFromAvailableReq request)
    {
        var res = new S2CGpCourseUseFromAvailableRes();

        // TODO: remove requested available course ID from available courses

        // TODO: get period from requested available course ID 
        res.FinishDateTime = (ulong)DateTimeOffset.UtcNow.AddMonths(12).ToUnixTimeSeconds();

        return res;
    }
}

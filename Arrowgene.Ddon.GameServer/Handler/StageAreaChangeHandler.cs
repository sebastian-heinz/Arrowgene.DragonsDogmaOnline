using System;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageAreaChangeHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(StageAreaChangeHandler));


        public StageAreaChangeHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_STAGE_AREA_CHANGE_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(GameFull.Dump_109);
        }
    }
}

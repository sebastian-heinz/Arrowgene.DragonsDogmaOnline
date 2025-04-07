using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MyRoomOtherRoomLayoutUpdateHandler : GameRequestPacketHandler<C2SMyRoomOtherRoomLayoutUpdateReq, S2CMyRoomOtherRoomLayoutUpdateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MyRoomOtherRoomLayoutUpdateHandler));

        public MyRoomOtherRoomLayoutUpdateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMyRoomOtherRoomLayoutUpdateRes Handle(GameClient client, C2SMyRoomOtherRoomLayoutUpdateReq request)
        {
            foreach(var otherClient in client.Party.Clients)
            {
                if (client == otherClient)
                {
                    continue;
                }

                otherClient.Send(new S2CMyRoomOtherRoomLayoutUpdateNtc()
                {
                    UpdateList = request.UpdateList
                });
            }

            return new();
        }
    }
}

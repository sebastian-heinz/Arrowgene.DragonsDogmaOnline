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
    public class MyRoomFurnitureLayoutHandler : GameRequestPacketHandler<C2SMyRoomFurnitureLayoutReq, S2CMyRoomFurnitureLayoutRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MyRoomFurnitureLayoutHandler));

        public MyRoomFurnitureLayoutHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMyRoomFurnitureLayoutRes Handle(GameClient client, C2SMyRoomFurnitureLayoutReq request)
        {
            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var layout in request.FurnitureLayoutData)
                {
                    if (layout.LayoutId == 0)
                    {
                        Server.Database.DeleteMyRoomCustomization(client.Character.CharacterId, layout.ItemID, connection);
                    }
                    else
                    {
                        Server.Database.UpsertMyRoomCustomization(client.Character.CharacterId, layout.LayoutId, layout.ItemID, connection);
                    }
                }
            });

            return new();
        }
    }
}

using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanSetFurnitureHandler : GameRequestPacketHandler<C2SClanSetFurnitureReq, S2CClanSetFurnitureRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanSetFurnitureHandler));

        public ClanSetFurnitureHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanSetFurnitureRes Handle(GameClient client, C2SClanSetFurnitureReq request)
        {
            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var update in request.FurnitureLayoutData)
                {
                    if (update.LayoutId == 0)
                    {
                        // Only one slot makes this possible, so we can just hardcode it.
                        Server.Database.DeleteClanBaseCustomization(client.Character.ClanId, ClanFurnitureType.LoungeBoard, connection);
                    }
                    else
                    {
                        Server.Database.InsertOrUpdateClanBaseCustomization(client.Character.ClanId, update.LayoutId, update.ItemID, connection);
                    }
                }
            });
            
            return new();
        }
    }
}

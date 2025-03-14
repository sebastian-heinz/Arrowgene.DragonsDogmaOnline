using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SetShortcutHandler : GameRequestPacketHandler<C2SSetShortcutReq, S2CSetShortcutRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpGetWarpPointListHandler));

        public SetShortcutHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSetShortcutRes Handle(GameClient client, C2SSetShortcutReq request)
        {
            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (CDataShortCut shortcut in request.ShortCutList)
                {
                    Server.Database.ReplaceShortcut(client.Character.ContentCharacterId, shortcut, connection);
                }
            });

            client.Character.ShortCutList = request.ShortCutList;

            return new();
        }
    }
}

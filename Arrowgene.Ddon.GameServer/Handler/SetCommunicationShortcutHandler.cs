using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SetCommunicationShortcutHandler : GameRequestPacketHandler<C2SSetCommunicationShortcutReq, S2CSetCommunicationShortcutRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SetCommunicationShortcutHandler));

        public SetCommunicationShortcutHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSetCommunicationShortcutRes Handle(GameClient client, C2SSetCommunicationShortcutReq request)
        {
            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (CDataCommunicationShortCut shortcut in request.CommunicationShortCutList)
                {
                    Server.Database.ReplaceCommunicationShortcut(client.Character.ContentCharacterId, shortcut, connection);
                }
            });

            client.Character.CommunicationShortCutList = request.CommunicationShortCutList;
            return new();
        }
    }
}

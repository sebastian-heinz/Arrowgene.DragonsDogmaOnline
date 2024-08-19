using Arrowgene.Ddon.Database.Deferred;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SetCommunicationShortcutHandler : StructurePacketHandler<GameClient, C2SSetCommunicationShortcutReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SetCommunicationShortcutHandler));


        public SetCommunicationShortcutHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSetCommunicationShortcutReq> request)
        {
            S2CSetCommunicationShortcutRes response = new S2CSetCommunicationShortcutRes();
            
            client.Character.CommunicationShortCutList = request.Structure.CommunicationShortCutList;
            client.Send(response);

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (CDataCommunicationShortCut shortcut in request.Structure.CommunicationShortCutList)
                {
                    Server.Database.ReplaceCommunicationShortcut(client.Character.CharacterId, shortcut, connection);
                }
            });
        }
    }
}

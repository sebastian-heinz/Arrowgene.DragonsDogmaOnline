using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterSetOnlineStatusHandler : StructurePacketHandler<GameClient, C2SCharacterSetOnlineStatusReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterSetOnlineStatusHandler));

        public CharacterSetOnlineStatusHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterSetOnlineStatusReq> packet)
        {
            client.Character.OnlineStatus = packet.Structure.OnlineStatus;

            // TODO: Figure out packet.Structure.IsSaveSetting

            client.Send(new S2CCharacterSetOnlineStatusRes() {
                OnlineStatus = packet.Structure.OnlineStatus
            });
            
            Database.UpdateCharacterOnlineStatus(client.Character);
        }
    }
}

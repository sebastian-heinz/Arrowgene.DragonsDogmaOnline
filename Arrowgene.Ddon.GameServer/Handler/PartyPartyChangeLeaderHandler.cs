using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyChangeLeaderHandler : GameStructurePacketHandler<C2SPartyPartyChangeLeaderReq>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(PartyPartyChangeLeaderHandler));

        public PartyPartyChangeLeaderHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyChangeLeaderReq> packet)
        {
            S2CPartyPartyChangeLeaderRes res = new S2CPartyPartyChangeLeaderRes();
            uint newLeaderCharacterId = packet.Structure.CharacterId;

            PartyGroup party = client.Party;
            if (party == null)
            {
                Logger.Error(client, "Could not leave party, does not exist");
                // todo return error
                return;
            }

            var previousLeader = client.Party.Leader;

            ErrorRes<PlayerPartyMember> newLeader = party.ChangeLeader(client, newLeaderCharacterId);
            if (newLeader.HasError)
            {
                Logger.Info(client, $"error during new leader");
                res.Error = (uint)newLeader.ErrorCode;
                client.Send(res);
                return;
            }

            S2CPartyPartyChangeLeaderNtc ntc = new S2CPartyPartyChangeLeaderNtc();
            ntc.CharacterId = newLeaderCharacterId;
            party.SendToAll(ntc);

            client.Send(res);

            PlayerPartyMember currentLeader = party.Leader;
            if (previousLeader != null)
            {
                Server.CharacterManager.UpdateOnlineStatus(previousLeader.Client, previousLeader.Client.Character, OnlineStatus.PtMember);
                Logger.Info(client, $"Party leader changed from {previousLeader.Client.Character.CharacterId} to {currentLeader.Client.Character.CharacterId} for PartyId:{party.Id}");
            }
            else
            {
                Logger.Info(client, $"The character {currentLeader.Client.Character.CharacterId} has been promoted to leader for PartyId:{party.Id}");
            }

            if (party.MemberCount() == 1)
            {
                Server.CharacterManager.UpdateOnlineStatus(currentLeader.Client, currentLeader.Client.Character, OnlineStatus.Online);
            }
            else
            {
                Server.CharacterManager.UpdateOnlineStatus(currentLeader.Client, currentLeader.Client.Character, OnlineStatus.PtLeader);
            }
        }
    }
}

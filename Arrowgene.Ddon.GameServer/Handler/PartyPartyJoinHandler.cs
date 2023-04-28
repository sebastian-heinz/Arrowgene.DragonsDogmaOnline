using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyJoinHandler : GameStructurePacketHandler<C2SPartyPartyJoinReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyJoinHandler));

        public PartyPartyJoinHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyJoinReq> packet)
        {
            S2CPartyPartyJoinRes res = new S2CPartyPartyJoinRes();

            PartyGroup party = Server.PartyManager.GetParty(packet.Structure.PartyId);
            if (party == null)
            {
                Logger.Error(client, "failed to find party (Server.PartyManager.GetParty() == null)");
                res.Error = (uint)ErrorCode.ERROR_CODE_FAIL;
                client.Send(res);
                return;
            }

            ErrorRes<PlayerPartyMember> join = party.Join(client);
            if (join.HasError)
            {
                Logger.Error(client, "failed to join party");
                res.Error = (uint)join.ErrorCode;
                client.Send(res);
                return;
            }

            res.PartyId = party.Id;
            client.Send(res);

            S2CPartyPartyJoinNtc ntc = new S2CPartyPartyJoinNtc();
            ntc.HostCharacterId = party.Host.Client.Character.Id;
            ntc.LeaderCharacterId = party.Leader.Client.Character.Id;
            foreach (PartyMember member in party.Members)
            {
                ntc.PartyMembers.Add(member.GetCDataPartyMember());
            }

            party.SendToAll(ntc);


            // Send party player context NTCs to the new member
            foreach (PartyMember member in party.Members)
            {
                party.SendToAll(member.GetS2CContextGetParty_ContextNtc());
                // TODO only new member or all ?
            }

            Logger.Info(client, $"joined PartyId:{party.Id}");
        }
    }
}

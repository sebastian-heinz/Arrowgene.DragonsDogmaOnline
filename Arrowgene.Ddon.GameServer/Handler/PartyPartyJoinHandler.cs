using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.GameServer.Quests;
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

            res.ContentNumber = party.ContentId;
            if (res.ContentNumber != 0)
            {
                Server.CharacterManager.UpdateOnlineStatus(client, client.Character, OnlineStatus.Contents);
            }

            var characterContentId = Server.ExmManager.GetContentIdForCharacter(client.Character);
            if (party.ContentId != 0 && characterContentId != party.ContentId)
            {
                Server.ExmManager.AddCharacterToContentGroup(party.ContentId, client.Character);
            }

            var partyLeader = party.Leader.Client.Character;

            S2CPartyPartyJoinNtc ntc = new S2CPartyPartyJoinNtc();
            ntc.HostCharacterId = party.Host.Client.Character.CharacterId;
            ntc.LeaderCharacterId = partyLeader.CharacterId;
            foreach (PartyMember member in party.Members)
            {
                ntc.PartyMembers.Add(member.GetCDataPartyMember());
            }

            party.SendToAll(ntc);

            S2CContextGetPartyPlayerContextNtc newMemberContext = join.Value.GetS2CContextGetParty_ContextNtcEx();
            if (partyLeader.CommonId != client.Character.CommonId)
            {
                // Update player position when joining from a different stage
                client.Character.StageNo = partyLeader.StageNo;
                client.Character.Stage = partyLeader.Stage;
                newMemberContext.Context.Base.StageNo = (int) partyLeader.StageNo;
            }

            if (party.Clients.Count > 0)
            {
                // Send existing party player context NTCs to the new member
                foreach (PartyMember member in party.Members)
                {
                    if (member.MemberIndex == join.Value.MemberIndex)
                    {
                        // Skip ourselves
                        continue;
                    }
                    client.Send(member.GetS2CContextGetParty_ContextNtc());
                }

                // Send new members to all existing party members
                // client.Party.SendToAllExcept(newMemberContext, client);
                client.Party.SendToAll(newMemberContext);
            }

            res.PartyId = party.Id;
            client.Send(res);

            Logger.Info(client, $"joined PartyId:{party.Id}");
        }
    }
}

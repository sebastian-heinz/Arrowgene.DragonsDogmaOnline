using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemReadyHandler : GameStructurePacketHandler<C2SEntryBoardEntryBoardItemReadyReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemReadyHandler));

        public EntryBoardEntryBoardItemReadyHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SEntryBoardEntryBoardItemReadyReq> request)
        {
            var data = Server.BoardManager.GetGroupDataForCharacter(client.Character);

            Server.BoardManager.SetGroupMemberReadyState(client.Character, true);

            var ntc = new S2CEntryBoardEntryBoardItemReserveNtc()
            {
                NowMember = Server.BoardManager.NumGroupMembersReady(data.EntryItem.Id),
                MaxMember = (uint) data.Members.Count,
            };
            client.Send(ntc);

            client.Send(new S2CEntryBoardEntryBoardItemReadyRes());

            if (Server.BoardManager.AllGroupMembersReady(data.EntryItem.Id))
            {
                Server.BoardManager.CancelReadyUpTimer(data.EntryItem.Id);
                
                // If some sort of party recreation was taking place, it is now over
                data.IsInRecreate = false;

                // Label content is in progress so other can't join after group starts
                data.ContentInProgress = true;

                PartyGroup party = Server.PartyManager.NewParty(data.BoardId);

                var hostClient = Server.ClientLookup.GetClientByCharacterId(data.PartyLeaderCharacterId);
                party.AddHost(hostClient);

                S2CPartyPartyInviteAcceptNtc inviteAcceptNtc = new S2CPartyPartyInviteAcceptNtc();
                inviteAcceptNtc.ServerId = (ushort)Server.Id;
                inviteAcceptNtc.PartyId = party.Id;
                inviteAcceptNtc.StageId = hostClient.Character.Stage.Id;
                inviteAcceptNtc.PositionId = 0;
                inviteAcceptNtc.MemberIndex = 0;

                hostClient.Send(inviteAcceptNtc);

                var members = data.Members.ToList();
                for (byte i = 1; i < data.Members.Count; i++)
                {
                    var characterId = members[i];
                    var memberClient = Server.ClientLookup.GetClientByCharacterId(characterId);

                    var invitedMember = party.Invite(memberClient, hostClient);
                    if (invitedMember.HasError)
                    {
                        Logger.Error($"(EntryBoard) Failed to invite CharacterId={memberClient.Character.CharacterId} to PartyId={party.Id}");
                        continue;
                    }

                    var partyMember = party.Accept(memberClient);
                    if (partyMember.HasError)
                    {
                        Logger.Error($"(EntryBoard) CharacterId={memberClient.Character.CharacterId} failed to accept invite for PartyId={party.Id}");
                        continue;
                    }

                    inviteAcceptNtc.MemberIndex = i;
                    memberClient.Send(inviteAcceptNtc);
                }
            }
        }
    }
}

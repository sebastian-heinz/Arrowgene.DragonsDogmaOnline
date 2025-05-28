using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemReadyHandler : GameRequestPacketQueueHandler<C2SEntryBoardEntryBoardItemReadyReq, S2CEntryBoardEntryBoardItemReadyRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemReadyHandler));

        public EntryBoardEntryBoardItemReadyHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SEntryBoardEntryBoardItemReadyReq request)
        {
            PacketQueue packetQueue = new PacketQueue();

            var data = Server.BoardManager.GetGroupDataForCharacter(client.Character);

            Server.BoardManager.SetGroupMemberReadyState(client.Character, true);

            var ntc = new S2CEntryBoardEntryBoardItemReserveNtc()
            {
                NowMember = Server.BoardManager.NumGroupMembersReady(data.EntryItem.Id),
                MaxMember = (uint) data.Members.Count,
            };
            client.Enqueue(ntc, packetQueue);

            client.Enqueue(new S2CEntryBoardEntryBoardItemReadyRes(), packetQueue);

            if (Server.BoardManager.AllGroupMembersReady(data.EntryItem.Id))
            {
                Server.BoardManager.CancelReadyUpTimer(data.EntryItem.Id);
                
                // If some sort of party recreation was taking place, it is now over
                data.IsInRecreate = false;

                // Label content is in progress so other can't join after group starts
                data.ContentStatus = ContentStatus.Started;

                PartyGroup party = Server.PartyManager.NewParty(data.BoardId);

                var hostClient = Server.ClientLookup.GetClientByCharacterId(data.PartyLeaderCharacterId);
                party.AddHost(hostClient);

                S2CPartyPartyInviteAcceptNtc inviteAcceptNtc = new S2CPartyPartyInviteAcceptNtc();
                inviteAcceptNtc.ServerId = (ushort)Server.Id;
                inviteAcceptNtc.PartyId = party.Id;
                inviteAcceptNtc.StageId = hostClient.Character.Stage.Id;
                inviteAcceptNtc.PositionId = 0;
                inviteAcceptNtc.MemberIndex = 0;

                hostClient.Enqueue(inviteAcceptNtc, packetQueue);

                var members = data.Members.ToList();
                for (byte i = 1; i < data.Members.Count; i++)
                {
                    var characterId = members[i];
                    var memberClient = Server.ClientLookup.GetClientByCharacterId(characterId);
                    try
                    {
                        var invitedMember = party.Invite(memberClient, hostClient, false) ??
                            throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_INTERNAL_ERROR, $"Failed to invite member to EXM party ({characterId}:{party.Id}:{data.EntryItem.Id})");
                        var partyMember = party.Accept(memberClient) ??
                            throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_INTERNAL_ERROR, $"Failed to accept EXM party invite ({characterId}:{party.Id}:{data.EntryItem.Id})");

                        inviteAcceptNtc.MemberIndex = i;
                        memberClient.Enqueue(inviteAcceptNtc, packetQueue);
                    }
                    catch (ResponseErrorException ex)
                    {
                        // This is an internal party error, try and continue gracefully.
                        if (ErrorCode.ERROR_CODE_PARTY_INTERNAL_ERROR <= ex.ErrorCode && ex.ErrorCode <= ErrorCode.ERROR_CODE_PARTY_IS_NOT_PAWN_OWNER)
                        {
                            Logger.Error($"(EntryBoard) Failure to handle CharacterId={memberClient?.Character?.CharacterId ?? uint.MaxValue}, PartyId={party.Id}: {ex.Message}");
                            continue;
                        }
                        throw;
                    }
                }
            }

            return packetQueue;
        }
    }
}

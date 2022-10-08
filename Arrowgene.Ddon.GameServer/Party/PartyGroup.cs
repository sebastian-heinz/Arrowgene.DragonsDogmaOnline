using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Party
{
    public class PartyGroup
    {
        public const uint MaxPartyMember = 4;
        public const int InvalidSlotIndex = -1;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyGroup));

        private readonly object _lock;
        private readonly PartyMember[] _slots;
        private readonly PartyManager _partyManager;

        private PlayerPartyMember _leader;
        private PlayerPartyMember _host;
        private bool _isBreakup;

        public PartyGroup(uint id, PartyManager partyManager)
        {
            MaxSlots = MaxPartyMember;
            _lock = new object();
            _slots = new PartyMember[MaxSlots];
            _partyManager = partyManager;
            _isBreakup = false;

            Id = id;

            // TODO 
            Contexts = new Dictionary<ulong, Tuple<CDataContextSetBase, CDataContextSetAdditional>>();
        }

        public Dictionary<ulong, Tuple<CDataContextSetBase, CDataContextSetAdditional>> Contexts { get; set; }

        public uint MaxSlots { get; }
        public uint Id { get; }

        public PlayerPartyMember Host
        {
            get
            {
                lock (_lock)
                {
                    return _host;
                }
            }
        }

        public PlayerPartyMember Leader
        {
            get
            {
                lock (_lock)
                {
                    return _leader;
                }
            }
        }

        public List<GameClient> Clients
        {
            get
            {
                List<GameClient> clients = new List<GameClient>();
                lock (_lock)
                {
                    for (int i = 0; i < MaxSlots; i++)
                    {
                        if (_slots[i] is PlayerPartyMember member)
                        {
                            clients.Add(member.Client);
                        }
                    }
                }

                return clients;
            }
        }

        public List<PartyMember> Members
        {
            get
            {
                List<PartyMember> members = new List<PartyMember>();
                lock (_lock)
                {
                    for (int i = 0; i < MaxSlots; i++)
                    {
                        if (_slots[i] != null)
                        {
                            members.Add(_slots[i]);
                        }
                    }
                }

                return members;
            }
        }

        /// <summary>
        /// Player has been invited and will be holding a slot for them.
        /// However they are not assigned to this party until joined.
        /// </summary>
        /// <returns>PlayerPartyMember if a slot could be secured, or null on failure</returns>
        public PlayerPartyMember Invite(GameClient invitee, GameClient host)
        {
            if (invitee == null)
            {
                return null;
            }

            PlayerPartyMember partyMember = CreatePartyMember(invitee);
            lock (_lock)
            {
                if (!_partyManager.InviteParty(invitee, host, this))
                {
                    Logger.Error(invitee, $"[PartyId:{Id}][Invite] could not be invited");
                    return null;
                }

                int slotIndex = TakeSlot(partyMember);
                if (slotIndex <= InvalidSlotIndex)
                {
                    Logger.Error(invitee, $"[PartyId:{Id}][Invite] no free slot available");
                    return null;
                }

                partyMember.JoinState = JoinState.Prepare;
                return partyMember;
            }
        }

        public ErrorRes<PartyInvitation> RefuseInvite(GameClient client)
        {
            if (client == null)
            {
                return ErrorRes<PartyInvitation>.Fail;
            }

            lock (_lock)
            {
                PartyInvitation invitation = _partyManager.RemovePartyInvitation(client);
                if (invitation == null)
                {
                    Logger.Error(client, $"[PartyId:{Id}][RefuseInvite] was not invited");
                    return ErrorRes<PartyInvitation>.Fail;
                }

                PlayerPartyMember partyMember = GetPlayerPartyMember(client);
                if (partyMember == null)
                {
                    Logger.Error(client, $"[PartyId:{Id}][RefuseInvite] has no slot");
                    return ErrorRes<PartyInvitation>.Fail;
                }

                FreeSlot(partyMember.MemberIndex);
                return ErrorRes<PartyInvitation>.Success(invitation);
            }
        }


        /// <summary>
        /// Player has accepted the invitation and will progress to joining the party.
        /// </summary>
        /// <param name="client"></param>
        /// <returns>PlayerPartyMember on joining, or null on failure</returns>
        public ErrorRes<PlayerPartyMember> Accept(GameClient client)
        {
            if (client == null)
            {
                return ErrorRes<PlayerPartyMember>.Fail;
            }

            lock (_lock)
            {
                PartyInvitation invitation = _partyManager.RemovePartyInvitation(client);
                if (invitation == null)
                {
                    Logger.Error(client, $"[PartyId:{Id}][Accept] not invited");
                    return ErrorRes<PlayerPartyMember>.Fail;
                }

                if (invitation.Party != this)
                {
                    Logger.Error(client, $"[PartyId:{Id}][Accept] not invited to this party");
                    return ErrorRes<PlayerPartyMember>.Fail;
                }

                TimeSpan invitationAge = DateTime.Now - invitation.Date;
                if (invitationAge > TimeSpan.FromSeconds(PartyManager.InvitationTimeoutSec))
                {
                    Logger.Error(client, $"[PartyId:{Id}][Accept] invitation expired");
                    return ErrorRes<PlayerPartyMember>.Error(ErrorCode.ERROR_CODE_PARTY_INVITE_FAIL_REASON_TIMEOUT);
                }

                PlayerPartyMember partyMember = GetPlayerPartyMember(client);
                if (partyMember == null)
                {
                    Logger.Error(client, $"[PartyId:{Id}][Accept] has no slot");
                    return ErrorRes<PlayerPartyMember>.Fail;
                }

                return ErrorRes<PlayerPartyMember>.Success(partyMember);
            }
        }

        public PlayerPartyMember Join(GameClient client)
        {
            if (client == null)
            {
                return null;
            }

            lock (_lock)
            {
                PlayerPartyMember partyMember = GetPlayerPartyMember(client);
                if (partyMember == null)
                {
                    Logger.Error(client, $"[PartyId:{Id}][Join(GameClient)] has no slot");
                    return null;
                }

                client.Party = this;
                if (_leader == null && _host == null)
                {
                    // first to join the party
                    partyMember.IsLeader = true;
                    _leader = partyMember;
                    _host = partyMember;
                }

                partyMember.JoinState = JoinState.On;

                return partyMember;
            }
        }

        public PawnPartyMember Join(Pawn pawn)
        {
            if (pawn == null)
            {
                return null;
            }

            PawnPartyMember partyMember = CreatePartyMember(pawn);
            lock (_lock)
            {
                int slotIndex = TakeSlot(partyMember);
                if (slotIndex <= InvalidSlotIndex)
                {
                    Logger.Error($"[PartyId:{Id}][Join(Pawn)] no slot available");
                    return null;
                }


                partyMember.JoinState = JoinState.On;

                return partyMember;
            }
        }

        public void Leave(GameClient client)
        {
            if (client == null)
            {
                return;
            }

            Logger.Info(client, $"[PartyId:{Id}][Leave(GameClient)]");
            lock (_lock)
            {
                if (client.Party != this)
                {
                    Logger.Error(client, $"[PartyId:{Id}][Leave(GameClient)] not part of this party");
                    return;
                }

                if (_isBreakup)
                {
                    return;
                }

                PlayerPartyMember partyMember = GetPlayerPartyMember(client);
                if (partyMember == null)
                {
                    Logger.Error(client, $"[PartyId:{Id}][Leave(GameClient)] has no slot");
                    return;
                }

                FreeSlot(partyMember.MemberIndex);


                if (Clients.Count <= 0)
                {
                    Logger.Info(client, $"[PartyId:{Id}][Leave(GameClient)] was the last person, disband");
                    _partyManager.DisbandParty(Id);
                    return;
                }

                if (partyMember.IsLeader)
                {
                    _leader = null;
                    Logger.Info(client, $"[PartyId:{Id}][Leave(GameClient)] leader left");
                }
            }
        }

        public PartyMember Kick(byte memberIndex)
        {
            lock (_lock)
            {
                PartyMember member = GetSlot(memberIndex);
                if (member == null)
                {
                    Logger.Error($"[PartyId:{Id}][Kick] memberIndex:{memberIndex} not occupied");
                    return null;
                }

                FreeSlot(member.MemberIndex);

                return member;
            }
        }

        /// <summary>
        /// Changes to a new leader, returns new leader as value
        /// </summary>
        /// <param name="changeRequester"></param>
        /// <param name="leaderCharacterId"></param>
        /// <returns></returns>
        public ErrorRes<PlayerPartyMember> ChangeLeader(GameClient changeRequester, uint leaderCharacterId)
        {
            if (changeRequester == null)
            {
                return ErrorRes<PlayerPartyMember>.Fail;
            }

            lock (_lock)
            {
                PlayerPartyMember changeRequestMember = GetPlayerPartyMember(changeRequester);
                if (changeRequestMember == null)
                {
                    return ErrorRes<PlayerPartyMember>.Error(ErrorCode.ERROR_CODE_PARTY_NOT_PARTY_JOIN);
                }

                if (_leader == null)
                {
                    // no leader at the moment, allow change
                }
                else if (_leader != changeRequestMember)
                {
                    // person who requested change is not the leader
                    return ErrorRes<PlayerPartyMember>.Error(ErrorCode.ERROR_CODE_PARTY_IS_NOT_LEADER);
                }

                PlayerPartyMember newLeader = GetByCharacterId(leaderCharacterId);
                if (newLeader == null)
                {
                    return ErrorRes<PlayerPartyMember>.Error(ErrorCode.ERROR_CODE_PARTY_NOT_PARTY_JOIN);
                }

                if (_leader != null)
                {
                    _leader.IsLeader = false;
                }

                newLeader.IsLeader = true;
                _leader = newLeader;
                return ErrorRes<PlayerPartyMember>.Success(newLeader);
            }
        }

        public ErrorRes<List<PartyMember>> Breakup(GameClient client)
        {
            if (client == null)
            {
                return ErrorRes<List<PartyMember>>.Fail;
            }

            lock (_lock)
            {
                PlayerPartyMember currentLeader = GetPlayerPartyMember(client);
                if (currentLeader == null)
                {
                    return ErrorRes<List<PartyMember>>.Error(ErrorCode.ERROR_CODE_PARTY_NOT_PARTY_JOIN);
                }

                if (!currentLeader.IsLeader)
                {
                    return ErrorRes<List<PartyMember>>.Error(ErrorCode.ERROR_CODE_PARTY_IS_NOT_LEADER);
                }

                if (!_partyManager.DisbandParty(Id))
                {
                    return ErrorRes<List<PartyMember>>.Error(ErrorCode.ERROR_CODE_FAIL);
                }

                List<PartyMember> members = Members;
                for (int i = 0; i < MaxSlots; i++)
                {
                    FreeSlot(i);
                }

                _leader = null;
                _host = null;
                _isBreakup = true;

                return ErrorRes<List<PartyMember>>.Success(members);
            }
        }

        /// <summary>
        /// Returns PlayerPartyMember for a given GameClient
        /// </summary>
        /// <param name="client"></param>
        /// <returns>PlayerPartyMember or null on failure</returns>
        public PlayerPartyMember GetPlayerPartyMember(GameClient client)
        {
            lock (_lock)
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (_slots[i] is PlayerPartyMember member && member.Client == client)
                    {
                        return member;
                    }
                }
            }

            return null;
        }

        public void SendToAll<TResStruct>(TResStruct res) where TResStruct : class, IPacketStructure, new()
        {
            StructurePacket<TResStruct> packet = new StructurePacket<TResStruct>(res);
            SendToAll(packet);
        }

        public void SendToAll(Packet packet)
        {
            foreach (GameClient client in Clients)
            {
                client.Send(packet);
            }
        }

        public int MemberCount()
        {
            lock (_lock)
            {
                int count = 0;
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (_slots[i] != null)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        private PlayerPartyMember GetByCharacterId(uint characterId)
        {
            lock (_lock)
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (_slots[i] is PlayerPartyMember member)
                    {
                        Character character = member.Character;
                        if (character == null)
                        {
                            continue;
                        }

                        if (character.Id == characterId)
                        {
                            return member;
                        }
                    }
                }
            }

            return null;
        }

        private int TakeSlot(PartyMember partyMember)
        {
            if (partyMember == null)
            {
                return InvalidSlotIndex;
            }

            int slotIndex = InvalidSlotIndex;
            lock (_lock)
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (_slots[i] == null)
                    {
                        slotIndex = i;
                        break;
                    }
                }

                partyMember.MemberIndex = slotIndex;
                _slots[slotIndex] = partyMember;
            }

            return slotIndex;
        }

        private void FreeSlot(int slotIndex)
        {
            lock (_lock)
            {
                _slots[slotIndex] = null;
            }
        }

        private PartyMember GetSlot(uint index)
        {
            if (index >= MaxSlots)
            {
                Logger.Error($"[PartyId:{Id}][GetSlot] index:{index} >= MaxSlots:{MaxSlots}, out of bounds");
                return null;
            }

            lock (_lock)
            {
                return _slots[index];
            }
        }

        private PlayerPartyMember CreatePartyMember(GameClient client)
        {
            PlayerPartyMember partyMember = new PlayerPartyMember();
            partyMember.Client = client;
            partyMember.Character = client.Character;
            partyMember.IsPawn = false;
            partyMember.MemberType = 1;
            partyMember.PawnId = 0;
            partyMember.IsPlayEntry = false;
            partyMember.AnyValueList = new byte[8];
            partyMember.IsLeader = false;
            partyMember.JoinState = JoinState.None;
            partyMember.SessionStatus = 0;
            partyMember.MemberIndex = InvalidSlotIndex;
            return partyMember;
        }

        private PawnPartyMember CreatePartyMember(Pawn pawn)
        {
            PawnPartyMember partyMember = new PawnPartyMember();
            partyMember.Pawn = pawn;
            partyMember.Character = pawn.Character;
            partyMember.IsPawn = true;
            partyMember.MemberType = 2;
            partyMember.PawnId = pawn.Id;
            partyMember.IsPlayEntry = false;
            partyMember.AnyValueList = new byte[8];
            partyMember.IsLeader = false;
            partyMember.JoinState = JoinState.None;
            partyMember.SessionStatus = 0;
            partyMember.MemberIndex = InvalidSlotIndex;
            return partyMember;
        }
    }
}

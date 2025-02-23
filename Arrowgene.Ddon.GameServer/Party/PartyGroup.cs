using Arrowgene.Ddon.GameServer.Context;
using Arrowgene.Ddon.GameServer.Instance;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Party
{
    public class PartyGroup
    {
        public const uint MaxPartyMember = 8; // TODO: Different max sizes per party type
        public const int InvalidSlotIndex = -1;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyGroup));

        private readonly object _lock;
        private readonly PartyMember[] _slots;
        private readonly PartyManager _partyManager;

        private PlayerPartyMember _leader;
        private PlayerPartyMember _host;
        private bool _isBreakup;

        public readonly ulong ContentId;
        public bool ExmInProgress;

        public InstanceEnemyManager InstanceEnemyManager { get; }

        public SharedQuestStateManager QuestState { get; }

        public Dictionary<uint, Dictionary<ulong, uint>> InstanceOmData { get; }

        public PartyGroup(uint id, PartyManager partyManager, ulong contentId)
        {
            MaxSlots = MaxPartyMember;
            _lock = new object();
            _slots = new PartyMember[MaxSlots];
            _partyManager = partyManager;
            _isBreakup = false;
            ContentId = contentId;

            Id = id;

            // TODO
            Contexts = new Dictionary<ulong, Tuple<CDataContextSetBase, CDataContextSetAdditional>>();

            InstanceEnemyManager = new InstanceEnemyManager(_partyManager.Server);

            InstanceOmData = new Dictionary<uint, Dictionary<ulong, uint>>();

            QuestState = new SharedQuestStateManager(this, partyManager.Server);
        }

        // Contexts[UID] = ContextData
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

        public bool IsSolo
        {
            get
            {
                return Clients.Count <= 1;
            }
        }

        public PlayerPartyMember Invite(GameClient invitee, GameClient host)
        {
            if (invitee == null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_INVITE_FAIL_REASON_MEMBER_NOT_FOUND, $"[PartyId:{Id}][Invite] (invitee == null)");
            }

            if (host == null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_INVITE_FAIL_REASON_NO_LEADER, $"[PartyId:{Id}][Invite] (host == null)");
            }

            PlayerPartyMember partyMember = CreatePartyMember(invitee);
            lock (_lock)
            {
                _partyManager.InviteParty(invitee, host, this);
                int slotIndex = TakeSlot(partyMember);
                Logger.Info(host, $"[PartyId:{Id}][Invite] invited {invitee.Identity}");
                partyMember.JoinState = JoinState.Prepare;
                return partyMember;
            }
        }

        public PartyInvitation RefuseInvite(GameClient client)
        {
            if (client == null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_MEMBER_NOT_FOUND, $"[PartyId:{Id}][RefuseInvite] (client == null)");
            }

            lock (_lock)
            {
                PartyInvitation invitation = _partyManager.RemovePartyInvitation(client)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_INVITED_PARTY,
                    $"[PartyId:{Id}][RefuseInvite] was not invited");

                PlayerPartyMember partyMember = GetPlayerPartyMember(client)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_INVITE_CANCEL_REASON_LOBBY_NUM_OVER,
                    $"[PartyId:{Id}][RefuseInvite] has no slot");
                
                FreeSlot(partyMember.MemberIndex);
                Logger.Info(client, $"[PartyId:{Id}][RefuseInvite] refused invite");
                return invitation;
            }
        }

        public PlayerPartyMember Accept(GameClient client)
        {
            if (client == null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_MEMBER_NOT_FOUND, $"[PartyId:{Id}][Accept] (client == null)");
            }

            lock (_lock)
            {
                PartyInvitation invitation = _partyManager.RemovePartyInvitation(client);
                if (invitation == null)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_INVITE_FAIL_REASON_INVITE_MISSING,
                        $"[PartyId:{Id}][Accept] not invited");
                }

                if (invitation.Party != this)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_INVITE_FAIL_REASON_WRONG_PARTY, 
                        $"[PartyId:{Id}][Accept] not invited to this party");
                }

                TimeSpan invitationAge = DateTime.UtcNow - invitation.Date;
                if (invitationAge > TimeSpan.FromSeconds(PartyManager.InvitationTimeoutSec))
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_INVITE_FAIL_REASON_TIMEOUT, 
                        $"[PartyId:{Id}][Accept] invitation expired");
                }

                PlayerPartyMember partyMember = GetPlayerPartyMember(client);
                if (partyMember == null)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_INVITE_FAIL_REASON_LOBBY_NUM_OVER,
                        $"[PartyId:{Id}][Accept] has no slot");
                }

                Logger.Info(client, $"[PartyId:{Id}][Accept] accepted invite");
                return partyMember;
            }
        }

        public PlayerPartyMember AddHost(GameClient client)
        {
            if (client == null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_MEMBER_NOT_FOUND, $"[PartyId:{Id}][AddHost(GameClient)] (client == null)");
            }

            PlayerPartyMember partyMember = CreatePartyMember(client);
            lock (_lock)
            {
                int slotIndex = TakeSlot(partyMember);  
                partyMember.JoinState = JoinState.Prepare;
            }

            return partyMember;
        }

        public PlayerPartyMember Join(GameClient client)
        {
            if (client == null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_MEMBER_NOT_FOUND, $"[PartyId:{Id}][Join(GameClient)] (client == null)");
            }

            lock (_lock)
            {
                PlayerPartyMember partyMember = GetPlayerPartyMember(client);
                if (partyMember == null && MemberCount() > 0)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_MEMBER_NOT_FOUND, $"[PartyId:{Id}][Join(GameClient)] has no slot");
                }

                if (_leader == null && _host == null)
                {
                    // first to join the party
                    partyMember.IsLeader = true;
                    _leader = partyMember;
                    _host = partyMember;
                }
                client.Party = this;

                partyMember.JoinState = JoinState.On;
                Logger.Info(client, $"[PartyId:{Id}][Join(GameClient)] joined");
                return partyMember;
            }
        }

        public PawnPartyMember Join(Pawn pawn)
        {
            if (pawn == null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_NOT_FOUNDED, $"[PartyId:{Id}][Join(Pawn)] (pawn == null)");
            }

            PawnPartyMember partyMember = CreatePartyMember(pawn);
            lock (_lock)
            {
                int slotIndex = TakeSlot(partyMember);

                partyMember.JoinState = JoinState.On;
                Logger.Info($"[PartyId:{Id}][Join(Pawn)Id:{pawn.PawnId}] joined");
                return partyMember;
            }
        }

        public void Leave(GameClient client)
        {
            if (client == null)
            {
                Logger.Error($"[PartyId:{Id}][Leave(GameClient)] (client == null)");
                return;
            }

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

                //Hand off any enemy groups they're responsible for.
                ContextManager.DelegateAllMasters(client);

                // We need to get rid of pawn players associated with the person who left
                foreach (var member in client.Party.Members)
                {
                    if (!member.IsPawn)
                    {
                        continue;
                    }

                    PawnPartyMember pawnMember = (PawnPartyMember)member;
                    foreach (var pawn in client.Character.Pawns)
                    {
                        if (pawn.CommonId == pawnMember.Pawn.CommonId)
                        {
                            FreeSlot(pawnMember.MemberIndex);
                            break;
                        }
                    }
                }

                FreeSlot(partyMember.MemberIndex);
                Logger.Info(client, $"[PartyId:{Id}][Leave(GameClient)] left");

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

        public PartyMember Kick(GameClient client, byte memberIndex)
        {
            if (client == null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_MEMBER_NOT_FOUND, $"[PartyId:{Id}][Kick] (client == null)");
            }

            lock (_lock)
            {
                PlayerPartyMember changeRequester = GetPlayerPartyMember(client)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_MEMBER_NOT_FOUND, 
                    $"[PartyId:{Id}][Kick] has no slot");

                PartyMember member = GetSlot(memberIndex)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_PARTY_JOIN, 
                    $"[PartyId:{Id}][Kick] memberIndex:{memberIndex} not occupied");

                if (member is PlayerPartyMember player)
                {
                    if (!changeRequester.IsLeader)
                    {
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_IS_NOT_LEADER, $"[PartyId:{Id}][Kick] is not authorized (not leader)");
                    }

                    //Hand off any enemy groups they're responsible for.
                    ContextManager.DelegateAllMasters(player.Client);

                    FreeSlot(member.MemberIndex);
                    Logger.Info(client, $"[PartyId:{Id}][Kick] kicked player {player.Client.Identity}");
                    return member;
                }

                if (member is PawnPartyMember pawn)
                {
                    if (pawn.Pawn.CharacterId != changeRequester.Client.Character.CharacterId)
                    {
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_IS_NOT_PAWN_OWNER, $"[PartyId:{Id}][Kick] is not authorized (not pawn owner)");
                    }

                    FreeSlot(member.MemberIndex);
                    Logger.Info(client, $"[PartyId:{Id}][Kick] kicked pawnId: {pawn.PawnId}");
                    return member;
                }

                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_INTERNAL_ERROR, $"[PartyId:{Id}][Kick] unknown object {member}");
            }
        }

        /// <summary>
        /// Changes to a new leader, returns new leader as value
        /// </summary>
        /// <param name="changeRequester"></param>
        /// <param name="leaderCharacterId"></param>
        /// <returns></returns>
        public PlayerPartyMember ChangeLeader(GameClient changeRequester, uint leaderCharacterId)
        {
            if (changeRequester == null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_MEMBER_NOT_FOUND, $"[PartyId:{Id}][ChangeLeader] (changeRequester == null)");
            }

            lock (_lock)
            {
                PlayerPartyMember changeRequestMember = GetPlayerPartyMember(changeRequester)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_PARTY_JOIN, 
                    $"[PartyId:{Id}][ChangeLeader] has no slot");

                if (_leader == null)
                {
                    Logger.Info(changeRequester, $"[PartyId:{Id}][ChangeLeader] has no leader, allow to change");
                }
                else if (_leader != changeRequestMember)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_IS_NOT_LEADER, $"[PartyId:{Id}][ChangeLeader] is not authorized");
                }

                PlayerPartyMember newLeader = GetByCharacterId(leaderCharacterId) 
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_PARTY_JOIN, 
                    $"[PartyId:{Id}][ChangeLeader] new leader characterId:{leaderCharacterId} has no slot");

                if (_leader != null)
                {
                    _leader.IsLeader = false;
                }

                newLeader.IsLeader = true;
                _leader = newLeader;

                Logger.Info(changeRequester, $"[PartyId:{Id}][ChangeLeader] leader changed to {newLeader.Client.Identity}");
                return newLeader;
            }
        }

        public List<PartyMember> Breakup(GameClient client)
        {
            if (client == null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_MEMBER_NOT_FOUND, $"[PartyId:{Id}][Breakup] (client == null)");
            }

            lock (_lock)
            {
                PlayerPartyMember currentLeader = GetPlayerPartyMember(client);
                if (currentLeader == null)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_PARTY_JOIN);
                }

                if (!currentLeader.IsLeader)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_IS_NOT_LEADER);
                }

                if (!_partyManager.DisbandParty(Id))
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_INTERNAL_ERROR);
                }

                List<PartyMember> members = Members;
                for (int i = 0; i < MaxSlots; i++)
                {
                    FreeSlot(i);
                }

                _leader = null;
                _host = null;
                _isBreakup = true;

                return members;
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

        public void EnqueueToAll<TResStruct>(TResStruct res, PacketQueue queue)
            where TResStruct : class, IPacketStructure, new()
        {
            StructurePacket<TResStruct> packet = new StructurePacket<TResStruct>(res);
            foreach(GameClient client in Clients)
            {
                queue.Enqueue((client, packet));
            }
        }

        public void SendToAll(Packet packet)
        {
            foreach (GameClient client in Clients)
            {
                client.Send(packet);
            }
        }

        public void SendToAllExcept<TResStruct>(TResStruct res, params GameClient[] exceptions) where TResStruct : class, IPacketStructure, new()
        {
            StructurePacket<TResStruct> packet = new StructurePacket<TResStruct>(res);
            SendToAllExcept(packet, exceptions);
        }

        public void EnqueueToAllExcept<TResStruct>(TResStruct res, PacketQueue queue, params GameClient[] exceptions)
            where TResStruct : class, IPacketStructure, new()
        {
            StructurePacket<TResStruct> packet = new StructurePacket<TResStruct>(res);
            foreach (GameClient client in Clients)
            {
                if (exceptions.Contains(client))
                {
                    continue;
                }
                queue.Enqueue((client, packet));
            }
        }

        public void SendToAllExcept(Packet packet, params GameClient[] exceptions)
        {
            foreach (GameClient client in Clients)
            {
                bool send = true;
                foreach (GameClient exception in exceptions)
                {
                    if (client == exception)
                    {
                        send = false;
                    }
                }

                if (send)
                {
                    client.Send(packet);
                }
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

        public void ResetInstance()
        {
            InstanceEnemyManager.Clear();
            Contexts.Clear();
            QuestState.ResetInstance();
            foreach (GameClient client in Clients)
            {
                client.InstanceGatheringItemManager.Clear();
                client.InstanceDropItemManager.Clear();
                client.Character.ContextOwnership.Clear();
            }
            OmManager.ResetAllOmData(InstanceOmData);
        }

        public PartyMember GetPartyMemberByCharacter(CharacterCommon characterCommon)
        {
            lock (_lock)
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (_slots[i] is PartyMember member)
                    {
                        if (member == null)
                        {
                            continue;
                        }

                        if (member is PawnPartyMember pawnMember && characterCommon is Pawn)
                        {
                            Pawn pawn = (Pawn)characterCommon;
                            if (pawnMember.PawnId == pawn.PawnId)
                            {
                                return member;
                            }
                        }
                        else if (member is PlayerPartyMember playerMember && characterCommon is Character)
                        {
                            Character character = (Character)characterCommon;
                            if (playerMember.Client.Character.CharacterId == character.CharacterId)
                            {
                                return member;
                            }
                        }
                    }
                }
            }

            return null;
        }

        private PlayerPartyMember GetByCharacterId(uint characterId)
        {
            lock (_lock)
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (_slots[i] is PlayerPartyMember member)
                    {
                        Character character = member.Client.Character;
                        if (character == null)
                        {
                            continue;
                        }

                        if (character.CharacterId == characterId)
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
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_MEMBER_NOT_FOUND, $"[PartyId:{Id}][TakeSlot] (partyMember == null)");
            }

            int slotIndex = InvalidSlotIndex;
            lock (_lock)
            {
                for (int i = 0; i < MaxSlots; i++)
                {
                    if (_slots[i] != null && _slots[i].CommonId == partyMember.CommonId)
                    {
                        // This character is already in the party, so fail gracefully without letting them take a second slot.
                        Logger.Error($"[PartyId:{Id}][TakeSlot] Party member already present.");
                        slotIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < MaxSlots; i++)
                {
                    if (_slots[i] == null)
                    {
                        slotIndex = i;
                        break;
                    }
                }

                if (slotIndex == InvalidSlotIndex)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_JOIN_NUM_OVER);
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
            PlayerPartyMember partyMember = new PlayerPartyMember(client, _partyManager.Server);
            partyMember.IsPawn = false;
            partyMember.MemberType = 1;
            partyMember.CommonId = client.Character.CommonId;
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
            partyMember.IsPawn = true;
            partyMember.CommonId = pawn.CommonId;
            partyMember.MemberType = 2;
            partyMember.PawnId = pawn.PawnId;
            partyMember.IsPlayEntry = false;
            partyMember.AnyValueList = new byte[8];
            partyMember.IsLeader = false;
            partyMember.JoinState = JoinState.None;
            partyMember.SessionStatus = 0;
            partyMember.MemberIndex = InvalidSlotIndex;
            return partyMember;
        }

        public int ClientIndex(GameClient client)
        {
            if (!Members.Any() || !Clients.Any()) return 0;

            var ind = Members.FindIndex(member =>
                member is PlayerPartyMember playerMember
                && playerMember.Client == client
            );
            return ind >= 0 ? ind : ClientIndex(Clients.First());
        }

        public bool Contains(CharacterCommon character)
        {
            foreach (PartyMember member in Members)
            {
                if (member is PlayerPartyMember playerMember)
                {
                    if (playerMember.Client.Character == character) return true;
                }
                else if (member is PawnPartyMember pawnMember)
                {
                    if (pawnMember.Pawn == character) return true;
                }
            }
            return false;
        }
    }
}

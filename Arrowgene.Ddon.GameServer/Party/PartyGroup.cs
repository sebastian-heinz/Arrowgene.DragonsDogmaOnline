using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Server;
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

        public PartyGroup(uint id, PartyManager partyManager)
        {
            MaxSlots = MaxPartyMember;
            _lock = new object();
            _slots = new PartyMember[MaxSlots];
            _partyManager = partyManager;

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
        /// <param name="client"></param>
        /// <returns>PlayerPartyMember if a slot could be secured, or null on failure</returns>
        public PlayerPartyMember Invite(GameClient client)
        {
            if (client == null)
            {
                return null;
            }

            PlayerPartyMember partyMember = CreatePartyMember(client);
            lock (_lock)
            {
                if (!_partyManager.AddInvitedParty(client, this))
                {
                    Logger.Error(client, "could not register client for invitation");
                    return null;
                }

                int slotIndex = TakeSlot(partyMember);
                if (slotIndex <= InvalidSlotIndex)
                {
                    Logger.Error(client, "No free slot available for client");
                    return null;
                }

                partyMember.JoinState = JoinState.Prepare;
                return partyMember;
            }
        }

        /// <summary>
        /// Player has accepted the invitation and will progress to joining the party.
        /// </summary>
        /// <param name="client"></param>
        /// <returns>PlayerPartyMember on joining, or null on failure</returns>
        public PlayerPartyMember Accept(GameClient client)
        {
            if (client == null)
            {
                return null;
            }

            lock (_lock)
            {
                PartyGroup invitedPartyGroup = _partyManager.RemoveInvitedParty(client);
                if (invitedPartyGroup == null)
                {
                    Logger.Error(client, "client was not registered for party");
                    return null;
                }

                if (invitedPartyGroup != this)
                {
                    Logger.Error(client, "client was not invited to this party");
                    return null;
                }

                PlayerPartyMember partyMember = GetPlayerPartyMember(client);
                if (partyMember == null)
                {
                    Logger.Error(client, "client has no slot in this party");
                    return null;
                }

                return partyMember;
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
                    Logger.Error(client, "client has no slot in this party");
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
                    Logger.Error("No free slot available for pawn");
                    return null;
                }


                partyMember.JoinState = JoinState.On;

                return partyMember;
            }
        }

        public PlayerPartyMember Leave(GameClient client)
        {
            if (client == null)
            {
                return null;
            }

            Logger.Info(client, $"Leaving Party:{Id}");
            lock (_lock)
            {
                if (client.Party != this)
                {
                    Logger.Error(client, "client not part of this party");
                    return null;
                }

                PlayerPartyMember partyMember = GetPlayerPartyMember(client);
                if (partyMember == null)
                {
                    Logger.Error(client, "client has no slot in this party");
                    return null;
                }

                FreeSlot(partyMember.MemberIndex);

                if (Clients.Count <= 0)
                {
                    Logger.Info(client, $"last person of party:{Id} left, disband party");
                    _partyManager.DisbandParty(Id);
                    return partyMember;
                }

                if (partyMember.IsLeader)
                {
                    Logger.Info(client, $"was leader of party:{Id}, leader left");
                    // TODO designate new leader
                }

                return partyMember;
            }
        }

        public PartyMember Kick(byte memberIndex)
        {
            lock (_lock)
            {
                PartyMember member = GetSlot(memberIndex);
                if (member == null)
                {
                    Logger.Error($"memberIndex:{memberIndex} not occupied in partyId:{Id}");
                    return null;
                }

                FreeSlot(member.MemberIndex);

                return member;
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
                Logger.Error(
                    $"can not retrieve slot {index} is out of bounds for maximum party size of {MaxSlots}");
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

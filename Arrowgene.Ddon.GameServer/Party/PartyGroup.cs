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
        public const uint MaxPartyMembers = 4;
        public const int InvalidSlotIndex = -1;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyGroup));

        private readonly object _lock;
        private readonly object[] _slots;
        private readonly List<GameClient> _clients;
        private readonly List<Pawn> _pawns;

        private GameClient _leader;
        private GameClient _host;

        public PartyGroup(uint id, GameClient creator)
        {
            _lock = new object();
            _slots = new object[MaxPartyMembers];
            _clients = new List<GameClient>();
            _pawns = new List<Pawn>();

            Id = id;
            _leader = creator;
            _host = creator;
            _clients.Add(creator);
            creator.Party = this;

            // TODO 
            Contexts = new Dictionary<ulong, Tuple<CDataContextSetBase, CDataContextSetAdditional>>();
        }

        public Dictionary<ulong, Tuple<CDataContextSetBase, CDataContextSetAdditional>> Contexts { get; set; }

        public GameClient Leader
        {
            get
            {
                lock (_lock)
                {
                    return _leader;
                }
            }
        }

        public GameClient Host
        {
            get
            {
                lock (_lock)
                {
                    return _host;
                }
            }
        }

        public uint Id { get; }

        public List<GameClient> Clients
        {
            get
            {
                lock (_lock)
                {
                    return new List<GameClient>(_clients);
                }
            }
        }

        public List<Character> Characters
        {
            get
            {
                lock (_lock)
                {
                    List<Character> characters = new List<Character>();
                    foreach (GameClient client in _clients)
                    {
                        Character character = client.Character;
                        if (character == null)
                        {
                            continue;
                        }

                        characters.Add(character);
                    }

                    foreach (Pawn pawn in _pawns)
                    {
                        Character character = pawn.Character;
                        if (character == null)
                        {
                            continue;
                        }

                        characters.Add(character);
                    }

                    return characters;
                }
            }
        }

        public bool Join(GameClient client)
        {
            if (client == null)
            {
                return false;
            }

            lock (_lock)
            {
                if (client.Party != null)
                {
                    Logger.Error(client, "client already has a party assigned");
                    return false;
                }

                int slotIndex = TakeSlot(client);
                if (slotIndex <= InvalidSlotIndex)
                {
                    Logger.Error(client, "No free slot available for client");
                    return false;
                }

                _clients.Add(client);
                client.Party = this;
                return true;
            }
        }

        public bool Join(Pawn pawn)
        {
            if (pawn == null)
            {
                return false;
            }

            lock (_lock)
            {
                int slotIndex = TakeSlot(pawn);
                if (slotIndex <= InvalidSlotIndex)
                {
                    Logger.Error("No free slot available for pawn");
                    return false;
                }

                _pawns.Add(pawn);
                return true;
            }
        }

        public bool Leave(GameClient client)
        {
            if (client == null)
            {
                return false;
            }

            lock (_lock)
            {
                if (client.Party != this)
                {
                    Logger.Error(client, "client not assigned to this group");
                    return false;
                }

                client.Party = null;

                if (!_clients.Remove(client))
                {
                    Logger.Error(client, "client not part of this group");
                    return false;
                }

                int slotIndex = GetSlotIndex(client);
                if (slotIndex <= InvalidSlotIndex)
                {
                    Logger.Error(client, "client not occupied any slot");
                    return false;
                }

                FreeSlot(slotIndex);
            }

            return true;
        }

        public bool Leave(Pawn pawn)
        {
            if (pawn == null)
            {
                return false;
            }

            lock (_lock)
            {
                // TODO ? pawn.Party = null;
                if (!_pawns.Remove(pawn))
                {
                    Logger.Error("pawn not part of this group");
                    return false;
                }

                int slotIndex = GetSlotIndex(pawn);
                if (slotIndex <= InvalidSlotIndex)
                {
                    Logger.Error("pawn not occupied any slot");
                    return false;
                }

                FreeSlot(slotIndex);
            }

            return true;
        }

        public byte GetMemberType(Character character)
        {
            object obj = GetSlot(character);
            if (obj is GameClient)
            {
                return 1;
            }

            if (obj is Pawn)
            {
                return 2;
            }

            Logger.Error($"no member type for character {character.Id}");
            return 0;
        }

        public Pawn GetPawn(uint index)
        {
            return GetSlot(index) as Pawn;
        }

        public GameClient GetClient(uint index)
        {
            return GetSlot(index) as GameClient;
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
                for (int i = 0; i < MaxPartyMembers; i++)
                {
                    if (_slots[i] != null)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public int GetSlotIndex(Character character)
        {
            return GetSlotIndex(GetSlot(character));
        }

        public int GetSlotIndex(GameClient client)
        {
            return GetSlotIndex((object)client);
        }

        public int GetSlotIndex(Pawn pawn)
        {
            return GetSlotIndex((object)pawn);
        }

        /// <summary>
        /// Intended to hold a free slot, but makes no guarantee whatsoever at the moment.
        /// </summary>
        /// <returns></returns>
        public int RegisterSlot()
        {
            lock (_lock)
            {
                for (int i = 0; i < MaxPartyMembers; i++)
                {
                    if (_slots[i] == null)
                    {
                        return i;
                    }
                }
            }

            return InvalidSlotIndex;
        }

        private object GetSlot(Character character)
        {
            lock (_lock)
            {
                foreach (GameClient client in _clients)
                {
                    Character clientCharacter = client.Character;
                    if (clientCharacter == character)
                    {
                        return client;
                    }
                }

                foreach (Pawn pawn in _pawns)
                {
                    Character pawnCharacter = pawn.Character;
                    if (pawnCharacter == character)
                    {
                        return pawn;
                    }
                }

                return null;
            }
        }

        private int GetSlotIndex(object obj)
        {
            if (obj == null)
            {
                return InvalidSlotIndex;
            }

            lock (_lock)
            {
                int slotIndex = InvalidSlotIndex;
                for (int i = 0; i < MaxPartyMembers; i++)
                {
                    if (_slots[i] == obj)
                    {
                        slotIndex = i;
                        break;
                    }
                }

                return slotIndex;
            }
        }

        private int TakeSlot(object obj)
        {
            if (obj == null)
            {
                return InvalidSlotIndex;
            }

            int slotIndex = InvalidSlotIndex;
            lock (_lock)
            {
                for (int i = 0; i < MaxPartyMembers; i++)
                {
                    if (_slots[i] == null)
                    {
                        slotIndex = i;
                        break;
                    }
                }

                _slots[slotIndex] = obj;
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

        private object GetSlot(uint index)
        {
            if (index >= MaxPartyMembers)
            {
                Logger.Error(
                    $"can not retrieve slot {index} is out of bounds for maximum party size of {MaxPartyMembers}");
            }

            lock (_lock)
            {
                return _slots[index];
            }
        }
    }
}

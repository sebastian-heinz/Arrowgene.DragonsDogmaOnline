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

        private object _lock;
        private object[] _slots;
        private List<GameClient> _clients;

        public PartyGroup(uint id, GameClient creator)
        {
            _lock = new object();
            _slots = new object[MaxPartyMembers];
            _clients = new List<GameClient>();

            Id = id;
            
            Members = new List<GameClient>();
            Pawns = new List<Pawn>();
            Contexts = new Dictionary<ulong, Tuple<CDataContextSetBase, CDataContextSetAdditional>>();
 
            Leader = creator;
            Host = creator;
            Members.Add(creator);
            creator.Party = this;
        }

        public uint Id { get; }
        public List<GameClient> Members { get; set; }
        public List<Pawn> Pawns { get; set; }
        public GameClient Leader { get; set; }
        public GameClient Host { get; set; }

        public Dictionary<ulong, Tuple<CDataContextSetBase, CDataContextSetAdditional>> Contexts { get; set; }


        public bool Join(GameClient client)
        {
            int slotIndex = TakeSlot(client);
            if (slotIndex <= InvalidSlotIndex)
            {
                Logger.Error(client, "No free slot available");
                return false;
            }


            return true;
        }

        public void Join(Pawn pawn)
        {
        }

        public void Leave(GameClient client)
        {
        }

        public void Leave(Pawn pawn)
        {
        }

        public Pawn GetPawn(uint memberIndex)
        {
            return null;
        }

        public void SendToAll<TResStruct>(TResStruct res) where TResStruct : class, IPacketStructure, new()
        {
            StructurePacket<TResStruct> packet = new StructurePacket<TResStruct>(res);
            SendToAll(packet);
        }

        public void SendToAll(Packet packet)
        {
            foreach (GameClient member in Members)
            {
                member.Send(packet);
            }
        }

        private int TakeSlot(object obj)
        {
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
    }
}

using System;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Party
    {
        private static uint Instances = 0;

        public Party()
        {
            Id = ++Instances; // Incase 0 is a reserved ID
            Members = new List<GameClient>();
            Contexts = new Dictionary<ulong, Tuple<CDataContextSetBase, CDataContextSetAdditional>>();
        }

        public uint Id { get; set; }
        public List<GameClient> Members { get; set; }
        public GameClient Leader { get; set; }
        public GameClient Host { get; set; }
        public Dictionary<ulong, Tuple<CDataContextSetBase, CDataContextSetAdditional>> Contexts { get; set; }

        public void SendToAll<TResStruct>(TResStruct res) where TResStruct : class, IPacketStructure, new()
        {
            StructurePacket<TResStruct> packet = new StructurePacket<TResStruct>(res);
            foreach(GameClient member in Members) {
                member.Send(packet);
            }
        }
    }
}
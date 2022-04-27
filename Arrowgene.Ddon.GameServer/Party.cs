using System.Collections.Generic;
using Arrowgene.Ddon.GameServer;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Party
    {
        private static uint Instances = 0;

        public Party()
        {
            Id = ++Instances; // Incase 0 is a reserved ID
            Members = new List<GameClient>();
        }

        public uint Id { get; set; }
        public List<GameClient> Members { get; set; }
        public GameClient Leader { get; set; }
        public GameClient Host { get; set; }
    }
}
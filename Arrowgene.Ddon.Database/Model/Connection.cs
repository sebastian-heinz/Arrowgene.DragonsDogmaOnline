using System;

namespace Arrowgene.Ddon.Database.Model
{
    public class Connection
    {
        public int ServerId { get; set; }
        public int AccountId { get; set; }
        public ConnectionType Type { get; set; }
        public DateTime Created { get; set; }

        public Connection()
        {
        }
    }
}

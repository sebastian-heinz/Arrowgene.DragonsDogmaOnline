using System;

namespace Arrowgene.Ddon.Database.Model
{
    public class Connection
    {
        public int AccountId { get; set; }
        public ConnectionType ConnectionType { get; set; }
        public DateTime Created { get; set; }

        public Connection()
        {
        }
    }
}

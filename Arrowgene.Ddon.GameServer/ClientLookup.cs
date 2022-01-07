using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Network;

namespace Arrowgene.Ddon.GameServer
{
    public class ClientLookup
    {
        private readonly List<Client> _clients;

        private readonly object _lock = new object();

        public ClientLookup()
        {
            _clients = new List<Client>();
        }

        /// <summary>
        /// Returns all Clients.
        /// </summary>
        public List<Client> GetAll()
        {
            lock (_lock)
            {
                return new List<Client>(_clients);
            }
        }

        /// <summary>
        /// Adds a Client.
        /// </summary>
        public void Add(Client client)
        {
            if (client == null)
            {
                return;
            }

            lock (_lock)
            {
                _clients.Add(client);
            }
        }

        /// <summary>
        /// Removes the Client from all lists and lookup tables.
        /// </summary>
        public void Remove(Client client)
        {
            lock (_lock)
            {
                _clients.Remove(client);
            }
        }
    }
}

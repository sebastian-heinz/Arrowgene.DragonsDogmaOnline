using System.Collections.Generic;
using Arrowgene.Ddon.Server.Network;

namespace Arrowgene.Ddon.Server
{
    public class ClientLookup<TClient> where TClient : Client
    {
        private readonly List<TClient> _clients;

        private readonly object _lock = new object();

        public ClientLookup()
        {
            _clients = new List<TClient>();
        }

        /// <summary>
        /// Returns all Clients.
        /// </summary>
        public List<TClient> GetAll()
        {
            lock (_lock)
            {
                return new List<TClient>(_clients);
            }
        }

        /// <summary>
        /// Adds a Client.
        /// </summary>
        public void Add(TClient client)
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
        public void Remove(TClient client)
        {
            lock (_lock)
            {
                _clients.Remove(client);
            }
        }
    }
}

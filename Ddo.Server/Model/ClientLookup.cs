﻿using System.Collections.Generic;

 namespace Ddo.Server.Model
{
    public class ClientLookup
    {
        private readonly List<DdoClient> _clients;

        private readonly object _lock = new object();

        public ClientLookup()
        {
            _clients = new List<DdoClient>();
        }

        /// <summary>
        /// Returns all Clients.
        /// </summary>
        public List<DdoClient> GetAll()
        {
            lock (_lock)
            {
                return new List<DdoClient>(_clients);
            }
        }

        /// <summary>
        /// Adds a Client.
        /// </summary>
        public void Add(DdoClient client)
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
        public void Remove(DdoClient client)
        {
            lock (_lock)
            {
                _clients.Remove(client);
            }
        }

        /// <summary>
        /// Returns a Client by AccountId if it exists.
        /// </summary>
        public DdoClient GetByAccountId(int accountId)
        {
            List<DdoClient> clients = GetAll();
            foreach (DdoClient client in clients)
            {
                if (client.Account.Id == accountId)
                {
                    return client;
                }
            }

            return null;
        }
    }
}

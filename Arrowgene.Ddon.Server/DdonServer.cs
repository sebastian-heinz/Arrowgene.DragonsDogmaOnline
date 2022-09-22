/*
 * This file is part of Arrowgene.Ddon.LoginServer
 *
 * Arrowgene.Ddon.LoginServer is a server implementation for the game "Dragons Dogma Online".
 * Copyright (C) 2019-2022 DDON Team
 *
 * Github: https://github.com/sebastian-heinz/Ddo-server
 *
 * Arrowgene.Ddon.LoginServer is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Arrowgene.Ddon.LoginServer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Arrowgene.Ddon.LoginServer. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.Ddon.Server
{
    public abstract class DdonServer<TClient> : IClientFactory<TClient>
        where TClient : Client
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DdonServer<TClient>));

        private readonly Consumer<TClient> _consumer;
        private readonly AsyncEventServer _server;
        private readonly ServerSetting _setting;

        public DdonServer(ServerSetting setting, IDatabase database, AssetRepository assetRepository)
        {
            _setting = setting;
            AssetRepository = assetRepository;
            Database = database;

            LogProvider.Configure<ServerLogger>(_setting);

            _consumer = new Consumer<TClient>(
                _setting,
                _setting.ServerSocketSettings,
                this
            );
            _consumer.ClientConnected += ClientConnected;
            _consumer.ClientDisconnected += ClientDisconnected;

            _server = new AsyncEventServer(
                _setting.ListenIpAddress,
                _setting.ServerPort,
                _consumer,
                _setting.ServerSocketSettings
            );
        }

        public AssetRepository AssetRepository { get; }
        public IDatabase Database { get; }

        public virtual void Start()
        {
            Logger.Info($"[{_setting.ServerSocketSettings.Identity}] Listening: {_server.IpAddress}:{_server.Port}");
            _server.Start();
        }

        public void Stop()
        {
            _server.Stop();
        }

        protected void AddHandler(IPacketHandler<TClient> packetHandler)
        {
            _consumer.AddHandler(packetHandler);
        }

        protected abstract void ClientConnected(TClient client);
        protected abstract void ClientDisconnected(TClient client);
        public abstract TClient NewClient(ITcpSocket socket);

        [Obsolete("deprecated, use `ClientLookup.GetAll()` instead")]
        public List<TClient> Clients => ClientLookup.GetAll();

        public abstract ClientLookup<TClient> ClientLookup { get; }
    }
}

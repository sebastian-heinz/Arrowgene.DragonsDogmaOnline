/*
 * This file is part of Arrowgene.Ddon.GameServer
 *
 * Arrowgene.Ddon.GameServer is a server implementation for the game "Dragons Dogma Online".
 * Copyright (C) 2019-2022 DDON Team
 *
 * Github: https://github.com/sebastian-heinz/Ddo-server
 *
 * Arrowgene.Ddon.GameServer is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Arrowgene.Ddon.GameServer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Arrowgene.Ddon.GameServer. If not, see <https://www.gnu.org/licenses/>.
 */

using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.GameServer.Logging;
using Arrowgene.Ddon.GameServer.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.Ddon.GameServer
{
    public class DdonGameServer
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(DdonGameServer));
        public GameServerSetting Setting { get; }
        public ClientLookup Clients { get; }

        private readonly Consumer _authConsumer;
        private readonly AsyncEventServer _authServer;


        public DdonGameServer(GameServerSetting setting)
        {
            Setting = new GameServerSetting(setting);
            LogProvider.Configure<DdonLogger>(Setting);

            Clients = new ClientLookup();

            _authConsumer = new Consumer(Setting, Setting.AuthServerSocketSettings, "Auth");
            _authConsumer.ClientDisconnected += AuthClientDisconnected;
            _authConsumer.ClientConnected += AuthClientConnected;
            _authServer = new AsyncEventServer(
                Setting.ListenIpAddress,
                Setting.AuthServerPort,
                _authConsumer,
                Setting.AuthServerSocketSettings
            );
            LoadPacketHandler();
        }

        private void AuthClientConnected(Client client)
        {
            client.InitializeChallenge();
        }

        private void AuthClientDisconnected(Client client)
        {
        }

        public void Start()
        {
            _authServer.Start();
        }

        public void Stop()
        {
            _authServer.Stop();
        }

        private void LoadPacketHandler()
        {
            _authConsumer.AddHandler(new ClientChallengeHandler(this));
            _authConsumer.AddHandler(new ClientSessionKeyHandler(this));
            _authConsumer.AddHandler(new ClientX1Handler(this));
            _authConsumer.AddHandler(new GetErrorMessageListReqHandler(this));
            _authConsumer.AddHandler(new ClientX3Handler(this));
            _authConsumer.AddHandler(new ClientX4Handler(this));
            _authConsumer.AddHandler(new ClientX5Handler(this));
        }
    }
}

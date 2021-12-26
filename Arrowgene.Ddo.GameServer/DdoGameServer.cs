/*
 * This file is part of Arrowgene.Ddo.GameServer
 *
 * Arrowgene.Ddo.GameServer is a server implementation for the game "Dragons Dogma Online".
 * Copyright (C) 2019-2020 Ddo Team
 *
 * Github: https://github.com/sebastian-heinz/Ddo-server
 *
 * Arrowgene.Ddo.GameServer is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Arrowgene.Ddo.GameServer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Arrowgene.Ddo.GameServer. If not, see <https://www.gnu.org/licenses/>.
 */

using Arrowgene.Buffers;
using Arrowgene.Ddo.GameServer.Handler;
using Arrowgene.Ddo.GameServer.Logging;
using Arrowgene.Ddo.GameServer.Network;
using Arrowgene.Ddo.Shared;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.Ddo.GameServer
{
    public class DdoGameServer
    {
        private static readonly DdoLogger Logger = LogProvider.Logger<DdoLogger>(typeof(DdoGameServer));
        public GameServerSetting Setting { get; }
        public ClientLookup Clients { get; }

        private readonly Consumer _authConsumer;
        private readonly AsyncEventServer _authServer;


        public DdoGameServer(GameServerSetting setting)
        {
            Setting = new GameServerSetting(setting);
            LogProvider.Configure<DdoLogger>(Setting);

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

            byte[] handshake = client.Handshake.CreateClientCertChallenge();
            Packet packet = new Packet(new StreamBuffer(handshake));
            client.Send(packet);
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
            _authConsumer.AddHandler(new ClientNetworkKeyHandler(this));
        }
    }
}

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

using Arrowgene.Ddo.GameServer.Handler;
using Arrowgene.Ddo.GameServer.Logging;
using Arrowgene.Ddo.GameServer.Network;
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
            string dataHex =
                "0130" +
                "b71a9b05ff4c4f2cbfc63bbf8b955587" +
                "b1d42764a984c71c5b710fd4b351e98e" +
                "bc90d3be3dc9d49ebfb981b7c4f01b0b" +
                "3944f294f0a114b35b44bb24084ee164" +
                "71d1f4d9d13c784f434af0ef35f17505" +
                "557a4a0a5c1a25b8013cee0bb55d4645" +
                "effa115785c5e480e84ffc32f82c9c1f" +
                "2f3e658723ba2794c238cd5f51655c5d" +
                "64ba2f3ccf2fb7ea43546f9aa87122e9" +
                "c6e9e85598e0c8926d13f5ef1481a47a" +
                "5fbeb9f34978337b0c475d2a730f3370" +
                "306275b02d1c456633e3180a6c373433" +
                "8b1dbdc68c21a9039e3c8c8d26341476" +
                "41f6a7aacf88f3df1bd439517d82c9d5" +
                "3f6ac1fd69549357963e0f4762390c64" +
                "674009c10dee3f2fca1b415bef5f0bec" +
                "821f794e9a6a6917e61d600977e8945c" +
                "f899a803c975048b3faf20021a839e16" +
                "3169ad17d3270b7e2986bd237fb53209";
            byte[] data = Util.FromHexString(dataHex);
            client.Send(data);
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

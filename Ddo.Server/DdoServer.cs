/*
 * This file is part of Ddo.Server
 *
 * Ddo.Server is a server implementation for the game "Dragons Dogma Online".
 * Copyright (C) 2019-2020 Ddo Team
 *
 * Github: https://github.com/sebastian-heinz/Ddo-server
 *
 * Ddo.Server is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Ddo.Server is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Ddo.Server. If not, see <https://www.gnu.org/licenses/>.
 */

using Arrowgene.Services.Logging;
using Arrowgene.Services.Networking.Tcp.Server.AsyncEvent;
using Ddo.Server.Common;
using Ddo.Server.Common.Instance;
using Ddo.Server.Database;
using Ddo.Server.Logging;
using Ddo.Server.Model;
using Ddo.Server.Packet;
using Ddo.Server.PacketHandler;
using Ddo.Server.Setting;
using Ddo.Server.Web;
using Ddo.Server.Web.Server.Kestrel;
using Ddo.Server.WebMiddlewares;
using Ddo.Server.WebRoutes;
using Microsoft.Extensions.FileProviders;

namespace Ddo.Server
{
    public class DdoServer
    {
        public DdoSetting Setting { get; }
        public PacketRouter Router { get; }
        public ClientLookup Clients { get; }
        public IDatabase Database { get; }
        public InstanceGenerator Instances { get; }

        public IFileProvider WebFileProvider { get; }

        private readonly QueueConsumer _authConsumer;
        private readonly AsyncEventServer _authServer;
        private readonly WebServer _webServer;
        private readonly DdoLogger _logger;

        public DdoServer(DdoSetting setting)
        {
            Setting = new DdoSetting(setting);

            LogProvider.Configure<DdoLogger>(Setting);
            _logger = LogProvider.Logger<DdoLogger>(this);

            Instances = new InstanceGenerator();
            Clients = new ClientLookup();
            Router = new PacketRouter();
            Database = new DdoDatabaseBuilder().Build(Setting.DatabaseSetting);

            _authConsumer = new QueueConsumer(Setting, Setting.ServerSocketSettings);
            _authConsumer.ClientDisconnected += AuthClientDisconnected;
            _authConsumer.ClientConnected += AuthClientConnected;
            _authServer = new AsyncEventServer(
                Setting.ListenIpAddress,
                Setting.AuthServerPort,
                _authConsumer,
                Setting.ServerSocketSettings
            );
            _webServer = new WebServer(Setting, new KestrelWebServer(Setting));
            WebFileProvider = new PhysicalFileProvider(Setting.WebSetting.WebFolder);
            LoadPacketHandler();
            LoadWebRoutes();
        }

        private void AuthClientConnected(DdoConnection client)
        {
            string dataHex =
                "0130b71a9b05ff4c4f2cbfc63bbf8b955587b1d42764a984c71c5b710fd4b351e98ebc90d3be3dc9d49ebfb981b7c4f01b0b3944f294f0a114b35b44bb24084ee16471d1f4d9d13c784f434af0ef35f17505557a4a0a5c1a25b8013cee0bb55d4645effa115785c5e480e84ffc32f82c9c1f2f3e658723ba2794c238cd5f51655c5d64ba2f3ccf2fb7ea43546f9aa87122e9c6e9e85598e0c8926d13f5ef1481a47a5fbeb9f34978337b0c475d2a730f3370306275b02d1c456633e3180a6c3734338b1dbdc68c21a9039e3c8c8d2634147641f6a7aacf88f3df1bd439517d82c9d53f6ac1fd69549357963e0f4762390c64674009c10dee3f2fca1b415bef5f0bec821f794e9a6a6917e61d600977e8945cf899a803c975048b3faf20021a839e163169ad17d3270b7e2986bd237fb53209";
            byte[] data = Util.FromHexString(dataHex);
            client.Send(data);
        }

        private void AuthClientDisconnected(DdoConnection client)
        {
        }
        
        public void Start()
        {
            _webServer.Start();
            _authServer.Start();
        }

        public void Stop()
        {
            _webServer.Stop();
            _authServer.Stop();
        }

        private void LoadPacketHandler()
        {
            _authConsumer.AddHandler(new TestHandler(this));
        }

        private void LoadWebRoutes()
        {
            _webServer.AddRoute(new IndexRoute());

            // Middleware - Order Matters
            _webServer.AddMiddleware(new StaticFileMiddleware(WebFileProvider));
        }
    }
}

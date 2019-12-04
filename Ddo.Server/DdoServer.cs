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

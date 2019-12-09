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
                "0130C0FA76B46338A97AAD8469AF8926D93232CEA2FBB37C7A807BC9B91028CDFA74B553790754BB160A838C371630EA75347AE3BAEEE746F28D9809F669FF0F4D516DBE7FE00656B918C9AEA6CE794FE5EA6BB16E1D58223DCD87DA0B3B531DA3719DF7200624FC2EC5196F7676EBB7D4FC89587E84433526D9C9F11E7EAE3D44B5276516BAC887BB932D6E0442F1C46C0E2BDE1F07745E1D490AD0FEBFAED94B5ADEF93AADE151C09D3E999684B98230B4088B0407638E1A39BD14676D85AAD612496DC1852CB674C5647BC68DF1FC62FECEEAAFA55E728E723268BE988AB1EA33885CCD5FDAE7B9B3F867FEE4BD62464DECC56E2215AF88C6464B355080A348A471DF15E660CFA35C0A5A1106D0A47E057FE0857D3DC7A02EBE976AA9C1B7F45BBBC31C87106488A34B33832B7BC237A7";
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

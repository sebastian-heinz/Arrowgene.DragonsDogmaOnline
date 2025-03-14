using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.WebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Rpc.Web.Route
{
    public class ServerStatusCommand : RpcQueryCommand
    {
        public ServerStatusCommand(WebCollection<string, string> queryParams) : base(queryParams)
        {
        }

        public class ServerStatus
        {
            public ushort Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Brief { get; set; } = string.Empty;
            public string TrafficName { get; set; } = string.Empty;
            public uint TrafficLevel { get; set; }
            public uint MaxLoginNum { get; set; }
            public uint LoginNum { get; set; }
            public string Addr { get; set; } = string.Empty;
            public ushort Port { get; set; }
            public ushort RpcPort { get; set; }
            public bool IsHide { get; set; }
            public bool PreventLogin { get; set; }
        }

        public override string Name => "ServerStatusCommand";

        public override RpcCommandResult Execute(DdonGameServer gameServer)
        {
            List<ServerStatus> statusList = new();
            ReturnValue = statusList;
            uint serverId = uint.TryParse(_queryParams.Get("serverid"), out uint serverIdParse) ? serverIdParse : 0;

            var serverList = new List<ServerInfo>(gameServer.AssetRepository.ServerList);
            var serverListSelected = serverId == 0 ? serverList : serverList.Where(x => x.Id == serverId).ToList();

            if (serverListSelected.Any())
            {
                var serverInfos = gameServer.RpcManager.ServerListInfo();
                foreach (var server in serverListSelected)
                {
                    var serverInfo = serverInfos.Find(x => x.Id == server.Id);
                    statusList.Add(new ServerStatus()
                    {
                        Id = server.Id,
                        Name = server.Name,
                        Brief = server.Brief,
                        TrafficName = serverInfo.TrafficName,
                        TrafficLevel = serverInfo.TrafficLevel,
                        MaxLoginNum = server.MaxLoginNum,
                        LoginNum = serverInfo.LoginNum,
                        Addr = server.Addr,
                        Port = server.Port,
                        RpcPort = server.RpcPort,
                        IsHide = server.IsHide,
                        PreventLogin = server.PreventLogin
                    });
                }
            }
            else
            {
                return new RpcCommandResult(this, false);
            }

            return new RpcCommandResult(this, true);
        }
    }

    public class ServerStatusRoute : RpcRouteTemplate
    {
        public override string Route => "/rpc/status";

        public ServerStatusRoute(IRpcExecuter executer) : base(executer)
        {
        }

        public override async Task<WebResponse> Get(WebRequest request)
        {
            return await HandleQuery<ServerStatusCommand>(request);
        }
    }
}

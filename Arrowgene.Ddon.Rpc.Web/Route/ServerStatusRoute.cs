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
    public class ServerStatusCommand : IRpcCommand
    {
        public class ServerStatus
        {
            public ushort Id { get; set; }
            public string Name { get; set; }
            public string Brief { get; set; }
            public string TrafficName { get; set; }
            public uint TrafficLevel { get; set; }
            public uint MaxLoginNum { get; set; }
            public uint LoginNum { get; set; }
            public string Addr { get; set; }
            public ushort Port { get; set; }
            public bool IsHide { get; set; }

            public ServerStatus()
            {
                Id = 0;
                Name = "";
                Brief = "";
                TrafficName = "";
                TrafficLevel = 0;
                MaxLoginNum = 0;
                LoginNum = 0;
                Addr = "";
                Port = 0;
                IsHide = false;
            }
        }

        public string Name => "ServerStatusCommand";

        public ServerStatusCommand(uint serverId, bool returnAll)
        {
            ServerId = serverId;
            ReturnAll = returnAll;
            Status = new List<ServerStatus>();
        }
        public List<ServerStatus> Status { get; set; }
        public uint ServerId { get; set; }
        public bool ReturnAll { get; set; }

        public RpcCommandResult Execute(DdonGameServer gameServer)
        {
            Status.Clear();
            var serverList = new List<CDataGameServerListInfo>(gameServer.AssetRepository.ServerList);
            var serverListSelected = ReturnAll ? serverList : serverList.Where(x => x.Id == ServerId).ToList();

            if (serverListSelected.Any())
            {
                var connections = gameServer.Database.SelectConnections();
                foreach (var server in serverListSelected)
                {
                    Status.Add(new ServerStatus()
                    {
                        Id = server.Id,
                        Name = server.Name,
                        Brief = server.Brief,
                        TrafficName = server.TrafficName,
                        TrafficLevel = server.TrafficLevel,
                        MaxLoginNum = server.MaxLoginNum,
                        LoginNum = (uint)connections.Count(x => x.ServerId == server.Id && x.Type == ConnectionType.GameServer),
                        Addr = server.Addr,
                        Port = server.Port,
                        IsHide = server.IsHide
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

    public class ServerStatusRoute : RpcWebRoute
    {
        public override string Route => "/rpc/status";

        public ServerStatusRoute(IRpcExecuter executer) : base(executer)
        {
        }

        public override async Task<WebResponse> Get(WebRequest request)
        {
            WebCollection<string, string> queryParams = request.QueryParameter;
            bool returnAll = queryParams.ContainsKey("all") ? bool.Parse(queryParams["all"]) : false;
            uint serverId = queryParams.ContainsKey("serverid") ? uint.Parse(queryParams["serverid"]) : 0;

            WebResponse response = new WebResponse();
            ServerStatusCommand status = new ServerStatusCommand(serverId, returnAll);
            RpcCommandResult result = Executer.Execute(status);
            if (!result.Success)
            {
                response.StatusCode = 500;
                await response.WriteAsync("Error");
                return response;
            }
            response.StatusCode = 200;
            await response.WriteJsonAsync(status.Status);
            return response;
        }
    }
}

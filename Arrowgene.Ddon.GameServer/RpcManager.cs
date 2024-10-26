using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer
{
    public class RpcManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanManager));

        private readonly DdonGameServer Server;
        private readonly Dictionary<ushort, ServerInfo> ChannelInfo; 

        public RpcManager(DdonGameServer server)
        {
            Server = server;
            ChannelInfo = Server.AssetRepository.ServerList.ToDictionary(x => x.Id,
                x => new ServerInfo()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Brief = x.Brief,
                    TrafficName = x.TrafficName,
                    TrafficLevel = x.TrafficLevel,
                    MaxLoginNum = x.MaxLoginNum,
                    LoginNum = x.LoginNum,
                    Addr = x.Addr,
                    Port = x.Port,
                    IsHide = x.IsHide,
                    RpcPort = x.RpcPort,
                    RpcAuthToken = x.RpcAuthToken,
                    IsHead = x.IsHead,
                });
        }

        public bool Auth(ushort id, string token)
        {
            return ChannelInfo.Values.Where(x => x.Id == id && x.RpcAuthToken == token).Any();
        }

        public List<CDataGameServerListInfo> ServerListInfo()
        {
            return ChannelInfo.Values.Select(x => x.ToCDataGameServerListInfo()).ToList();
        }

        public CDataGameServerListInfo ServerListInfo(ushort id)
        {
            return ChannelInfo[id].ToCDataGameServerListInfo();
        }

        public string Route(ushort id, string route)
        {
            var channel = ChannelInfo[id];
            return $"http://{channel.Addr}:{channel.RpcPort}/rpc/{route}";
        }
    }
}

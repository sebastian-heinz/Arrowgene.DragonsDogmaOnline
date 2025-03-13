using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class ServerInfo
    {
        public ServerInfo()
        {
            Name = string.Empty;
            Brief = string.Empty;
            RpcAuthToken = string.Empty;
        }

        public ushort Id { get; set; }
        public string Name { get; set; }
        public string Brief { get; set; }
        public uint MaxLoginNum { get; set; }
        public uint LoginNum { get; set; }
        public string Addr { get; set; }
        public ushort Port { get; set; }
        public bool IsHide { get; set; }
        public ushort RpcPort { get; set; }
        public string RpcAuthToken { get; set; }
        public ushort LoginId { get; set; }
        public bool PreventLogin { get; set; }

        public CDataGameServerListInfo ToCDataGameServerListInfo()
        {
            return new CDataGameServerListInfo()
            {
                Id = Id,
                Name = Name,
                Brief = Brief,
                TrafficName = string.Empty,
                TrafficLevel = 0,
                MaxLoginNum = MaxLoginNum,
                LoginNum = LoginNum,
                Addr = Addr,
                Port = Port,
                IsHide = IsHide,
            };
        }

        public string ToRpcRoute(string route = "")
        {
            return $"http://{Addr}:{RpcPort}/rpc/{route}";
        }
    }
}

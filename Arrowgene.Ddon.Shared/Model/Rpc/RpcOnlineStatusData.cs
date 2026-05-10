using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Model.Rpc
{
    public class RpcOnlineStatusData
    {
        public uint CharacterId { get; set; }
        public ushort ServerId { get; set; }
        public OnlineStatus OnlineStatus { get; set; }
    }
}
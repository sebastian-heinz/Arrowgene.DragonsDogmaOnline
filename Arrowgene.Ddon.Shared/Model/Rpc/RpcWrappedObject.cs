using System;

namespace Arrowgene.Ddon.Shared.Model.Rpc
{
    public class RpcWrappedObject
    {
        public RpcInternalCommand Command { get; set; }
        public ushort Origin { get; set; }
        public object Data { get; set; }
        public DateTime Timestamp { get; set; }
        public RpcWrappedObject()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}

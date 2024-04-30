using System;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum RpcMessageId : UInt32
    {
        Login = 0x41000001,
        HeartBeat0 = 0x40010002,
        HeartBeat1 = 0x40010015,
        HeartBeat2 = 0x4001001A,
        Combat1    = 0x40010037,
    }
}

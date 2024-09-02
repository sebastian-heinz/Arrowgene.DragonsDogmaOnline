using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum RpcMsgType : byte
    {
        Peer = 0,
        Other = 1,
        All = 2,
        AllAndParty = 3,
        Server = 4,
    }
}

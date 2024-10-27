using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum RpcInternalCommand
    {
        NotifyPlayerJoin,
        NotifyPlayerLeave,
        SendWhisperMessage,
        SendClanMessage
    }
}

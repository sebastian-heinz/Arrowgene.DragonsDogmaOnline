using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum RpcNetMsgDti : UInt16
    {
        cNetMsgCtrlAction = 0x4001, // MtDTI_MtDTI_0(&stru_2544948, "cNetMsgCtrlAction", &stru_2544910, 0x78uLL, 0x40010000u, 0, 0);
        cNetMsgToolNormal = 0x4F00,
        cNetMsgToolEasy = 0x4F01,
        cNetMsgSetNormal = 0x4300,
        cNetMsgGameNormal = 0x4100,
        cNetMsgGameEasy = 0x4101
    }
}

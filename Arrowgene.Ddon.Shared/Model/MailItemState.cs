using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    [Flags]
    public enum MailItemState : byte
    {
        None   = 0,
        Exist  = 1 << 0, // 1
        Item   = 1 << 1, // 2
        GP     = 1 << 2, // 4
        Course = 1 << 3, // 8
        Pawn   = 1 << 4, // 16
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum MailItemState : byte
    {
        None   = 0,
        Exist  = 1 << 0,
        Item   = 1 << 1,
        GP     = 1 << 2,
        Course = 1 << 3,
        Pawn   = 1 << 4,
    }
}

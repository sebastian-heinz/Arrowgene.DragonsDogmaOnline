using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public class ClientErrorCode
    {
        public ClientErrorCode()
        {
            Message = new();
        }

        public ErrorCode Code { get; set; }
        public Dictionary<string, string> Message { get; set;}
    }
}

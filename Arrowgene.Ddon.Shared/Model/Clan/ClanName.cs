using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Clan
{
    public class ClanName
    {
        public ClanName()
        {
            Name = string.Empty;
            ShortName = string.Empty;
        }

        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}

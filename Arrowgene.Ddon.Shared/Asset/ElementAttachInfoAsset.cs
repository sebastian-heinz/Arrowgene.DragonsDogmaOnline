using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class ElementAttachInfoAsset
    {
        public ElementAttachInfoAsset()
        {
            ElementAttachInfo = new Dictionary<uint, ElementAttachInfo>();
        }

        public Dictionary<uint, ElementAttachInfo> ElementAttachInfo {  get; set; }
    }
}

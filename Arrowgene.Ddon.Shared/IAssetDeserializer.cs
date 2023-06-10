using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared
{
    public interface IAssetDeserializer<T>
    {
        List<T> ReadPath(string path);
    }
}
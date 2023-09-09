using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared
{
    public interface IAssetDeserializer<T>
    {
        List<T> ReadPath(string path);
    }
}
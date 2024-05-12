namespace Arrowgene.Ddon.Shared
{
    public interface IAssetDeserializer<T>
    {
        T ReadPath(string path);
    }
}
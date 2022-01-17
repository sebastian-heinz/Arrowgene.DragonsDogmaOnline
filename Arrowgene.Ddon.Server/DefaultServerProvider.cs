namespace Arrowgene.Ddon.Server
{
    public class DefaultServerProvider : IServerProvider
    {
        public DefaultServerProvider(string assetPath)
        {
            AssetRepository = new AssetRepository(assetPath);
            AssetRepository.Initialize();
        }

        public AssetRepository AssetRepository { get; }
    }
}

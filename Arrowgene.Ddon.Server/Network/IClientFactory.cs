using Arrowgene.Networking.SAEAServer;

namespace Arrowgene.Ddon.Server.Network
{
    public interface IClientFactory<TClient> where TClient : Client
    {
        TClient NewClient(ClientHandle clientHandle);
    }
}

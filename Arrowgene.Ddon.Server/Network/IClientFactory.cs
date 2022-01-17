using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.Shared
{
    public interface IClientFactory<TClient> where TClient : Client
    {
        TClient NewClient(ITcpSocket socket);
    }
}

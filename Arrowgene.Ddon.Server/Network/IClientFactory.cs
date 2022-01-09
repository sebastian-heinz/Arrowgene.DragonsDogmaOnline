using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.Server.Network
{
    public interface IClientFactory<TClient> where TClient : Client
    {
        TClient NewClient(ITcpSocket socket);
    }
}

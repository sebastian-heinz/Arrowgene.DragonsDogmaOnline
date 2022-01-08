namespace Arrowgene.Ddon.Server.Network
{
    public interface IPacketHandler<TClient> where TClient : Client
    {
        void Handle(TClient client, Packet packet);
        PacketId Id { get; }
    }
}

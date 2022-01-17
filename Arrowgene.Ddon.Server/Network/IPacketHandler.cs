namespace Arrowgene.Ddon.Shared
{
    public interface IPacketHandler<TClient> where TClient : Client
    {
        void Handle(TClient client, Packet packet);
        PacketId Id { get; }
    }
}

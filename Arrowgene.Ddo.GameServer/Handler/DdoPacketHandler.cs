using Arrowgene.Ddo.GameServer.Network;

namespace Arrowgene.Ddo.GameServer.Handler
{
    public abstract class DdoPacketHandler<Req, Res>
        where Req : Packet
        where Res : Packet
    {
        protected abstract Res Handle(Client client, Req packet);
        PacketId Id { get; }
        
        void Handle(Client client, Packet packet)
        {
            Req req = packet as Req;
            if (req == null)
            {
                return;
            }
            Res res = Handle(client, req);
            client.Send(res);
        }
    }
}

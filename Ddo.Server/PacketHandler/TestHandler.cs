using Ddo.Server.Model;
using Ddo.Server.Packet;
using Ddo.Server.PacketResponses;

namespace Ddo.Server.PacketHandler
{
    public class TestHandler : ConnectionHandler
    {
        public TestHandler(DdoServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) PacketId.NewConnectionResponse;

        public override void Handle(DdoConnection connection, DdoPacket packet)
        {
            NewConnectionResponse response = new NewConnectionResponse();
            Router.Send(response, connection);
        }
    };
}

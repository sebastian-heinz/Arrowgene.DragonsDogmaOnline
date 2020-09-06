using System;
using Ddo.Server.Common;
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
            Console.WriteLine(packet.Data.ToAsciiString(true));
            Console.WriteLine(packet.Data.ToHexString(' '));
            
            string hexResponse1 =
                "00603b440b4e0e65f4d73322e9f37c0d73adb4b72750bc9e7a45d14bf59e1031576fdb9dce65b0ce1743c69ce4a1dafd8eb5175f0ec9372ed50d7b59f68ce1b87a13d4472c8478240b3c37dd2229d254337bede3f8f1f60a0d263634a994b6abbd97";

            byte[] response1 = Util.FromHexString(hexResponse1);
            
          connection.Send(response1);
        }
    };
}

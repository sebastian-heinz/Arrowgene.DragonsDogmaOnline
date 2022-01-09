using Arrowgene.Buffers;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientX11Handler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientX11Handler));


        public ClientX11Handler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.X11_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
       //     IBuffer req = packet.AsBuffer();
       //     uint error = req.ReadUInt32(Endianness.Big);
       //     uint result = req.ReadUInt32(Endianness.Big);
       //     CDataCharacterListInfo character = EntitySerializer.Get<CDataCharacterListInfo>().Read(req);
//
       //     
       //     IBuffer buffer = new StreamBuffer();
       //     buffer.WriteInt32(0, Endianness.Big); // error
       //     buffer.WriteInt32(0, Endianness.Big); // result
       //     client.Send(new Packet(PacketId.X11_RES, buffer.GetAllBytes(), PacketSource.Server));
        }
    }
}

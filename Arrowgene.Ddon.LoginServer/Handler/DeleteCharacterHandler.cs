using Arrowgene.Buffers;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;
using System.Text;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class DeleteCharacterHandler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(DeleteCharacterHandler));

        public DeleteCharacterHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_DELETE_CHARACTER_INFO_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
            IBuffer recv = packet.AsBuffer();
            uint characterID = recv.ReadUInt32(Endianness.Big);
            Logger.Debug(client, $"Tried to delete character with ID: {characterID}");


            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0); //us_error
            buffer.WriteUInt32(0, Endianness.Big);
            client.Send(new Packet(PacketId.L2C_DELETE_CHARACTER_INFO_RES, buffer.GetAllBytes()));
        }
    }
}

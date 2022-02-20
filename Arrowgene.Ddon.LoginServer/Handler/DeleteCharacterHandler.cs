using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Text;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class DeleteCharacterHandler : PacketHandler<LoginClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DeleteCharacterHandler));

        public DeleteCharacterHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_DELETE_CHARACTER_INFO_REQ;

        public override void Handle(LoginClient client, IPacket packet)
        {
            IBuffer recv = packet.AsBuffer();
            uint characterID = recv.ReadUInt32(Endianness.Big);
            Logger.Debug(client, $"Tried to delete character with ID: {characterID}");


            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0); //us_error
            buffer.WriteUInt32(0, Endianness.Big);
            client.Send(new Packet(PacketId.L2C_DELETE_CHARACTER_INFO_RES, buffer.GetAllBytes()));

            // Test for L2C_EJECTION_NTC: Kick user if they try to delete char.
            //
            //IBuffer buffer1 = new StreamBuffer();
            //buffer.WriteMtString("ArrowGene.Ddon: Kicked from server due to attempted character deletion!");
            //client.Send(new Packet(PacketId.L2C_EJECTION_NTC, buffer1.GetAllBytes()));
        }
    }
}

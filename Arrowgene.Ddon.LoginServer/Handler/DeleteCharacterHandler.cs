using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

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
            IBuffer req = packet.AsBuffer();
            uint characterId = req.ReadUInt32(Endianness.Big);

            IBuffer res = new StreamBuffer();
            if (!Database.DeleteCharacter(characterId))
            {
                Logger.Error(client, $"Failed to delete character with ID: {characterId}");
                res.WriteUInt32(1, Endianness.Big);
                res.WriteUInt32(0, Endianness.Big);
            }
            else
            {
                Logger.Info(client, $"Deleted character with ID: {characterId}");
                res.WriteUInt32(0, Endianness.Big);
                res.WriteUInt32(0, Endianness.Big);
            }

            client.Send(new Packet(PacketId.L2C_DELETE_CHARACTER_INFO_RES, res.GetAllBytes()));
        }
    }
}

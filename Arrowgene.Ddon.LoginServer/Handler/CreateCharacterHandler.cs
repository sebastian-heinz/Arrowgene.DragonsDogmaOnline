using Arrowgene.Buffers;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class CreateCharacterHandler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(CreateCharacterHandler));


        public CreateCharacterHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_CREATE_CHARACTER_DATA_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
            IBuffer req = packet.AsBuffer();
            var character = EntitySerializer.Get<C2LCreateCharacterDataReq>().Read(req);

            Logger.Debug(client, $"Create character '{character.CharacterInfo.FirstName} {character.CharacterInfo.LastName}'");
            Logger.Debug(client, $"CharacterID '{character.CharacterInfo.CharacterID}, UserID: {character.CharacterInfo.UserID}'");


            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0, Endianness.Big); // error
            buffer.WriteInt32(0, Endianness.Big); // result
            buffer.WriteInt32(0, Endianness.Big); // WaitNum
            client.Send(new Packet(PacketId.L2C_CREATE_CHARACTER_DATA_RES, buffer.GetAllBytes(), PacketSource.Server));
        }
    }
}

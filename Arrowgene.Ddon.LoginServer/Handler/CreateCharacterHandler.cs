using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class CreateCharacterHandler : StructurePacketHandler<LoginClient, C2LCreateCharacterDataReq>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(CreateCharacterHandler));


        public CreateCharacterHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_CREATE_CHARACTER_DATA_REQ;

        public override void Handle(LoginClient client, StructurePacket<C2LCreateCharacterDataReq> packet)
        {
            Logger.Debug(client, $"Create character '{packet.Structure.CharacterInfo.FirstName} {packet.Structure.CharacterInfo.LastName}'");
            Logger.Debug(client, $"CharacterID '{packet.Structure.CharacterInfo.CharacterID}, UserID: {packet.Structure.CharacterInfo.UserID}'");


            Send(client, new L2CCreateCharacterDataRes());

            // Sent to client once the player queue "WaitNum" above is 0,
            // send immediately in our case.
            Send(client, new L2CCreateCharacterDataNtc());
        }
    }
}

using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class CreateCharacterHandler : StructurePacketHandler<LoginClient, C2LCreateCharacterDataReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CreateCharacterHandler));


        public CreateCharacterHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_CREATE_CHARACTER_DATA_REQ;

        public override void Handle(LoginClient client, StructurePacket<C2LCreateCharacterDataReq> packet)
        {
            Logger.Debug(client, $"Create character '{packet.Structure.CharacterInfo.FirstName} {packet.Structure.CharacterInfo.LastName}'");
            Logger.Debug(client, $"CharacterID '{packet.Structure.CharacterInfo.CharacterID}, UserID: {packet.Structure.CharacterInfo.UserID}'");

            CDataCharacterInfo characterInfo = packet.Structure.CharacterInfo;
            
            Character character = new Character();
            character.AccountId = client.Account.Id;
            character.FirstName = characterInfo.FirstName;
            character.LastName = characterInfo.LastName;
            character.Visual = characterInfo.EditInfo;
            character.Status = characterInfo.StatusInfo;
            if (!Database.CreateCharacter(character))
            {
                Logger.Error(client, "Failed to create character");
            }

            client.Send(new L2CCreateCharacterDataRes());

            // Sent to client once the player queue "WaitNum" above is 0,
            // send immediately in our case.
            client.Send(new L2CCreateCharacterDataNtc());
        }
    }
}

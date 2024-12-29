using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class DeleteCharacterHandler : LoginRequestPacketHandler<C2LDeleteCharacterInfoReq, L2CDeleteCharacterInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DeleteCharacterHandler));

        public DeleteCharacterHandler(DdonLoginServer server) : base(server)
        {
        }

        public override L2CDeleteCharacterInfoRes Handle(LoginClient client, C2LDeleteCharacterInfoReq request)
        {
            if (!Database.DeleteCharacter(request.CharacterId))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_INTERNAL, $"Failed to delete character with ID: {request.CharacterId}");
            }
            else
            {
                Logger.Info(client, $"Deleted character with ID: {request.CharacterId}");
            }

            return new();
        }
    }
}

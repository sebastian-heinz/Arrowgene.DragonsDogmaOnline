using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionMoveInServerHandler : GameRequestPacketHandler<C2SConnectionMoveInServerReq, S2CConnectionMoveInServerRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ConnectionMoveInServerHandler));

        public ConnectionMoveInServerHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CConnectionMoveInServerRes Handle(GameClient client, C2SConnectionMoveInServerReq request)
        {
            Logger.Debug(client, $"Received SessionKey:{request.SessionKey}");
            GameToken token = Database.SelectToken(request.SessionKey);
            if (token == null)
            {
                Logger.Error(client, $"SessionKey:{request.SessionKey} not found");
                // TODO reply error
                // return;
            }

            Database.DeleteTokenByAccountId(token.AccountId);

            Account account = Database.SelectAccountById(token.AccountId);
            if (account == null)
            {
                Logger.Error(client, $"AccountId:{token.AccountId} not found");
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_FAIL);
            }
            client.Account = account;

            Character character = Server.CharacterManager.SelectCharacter(client, token.CharacterId);
            if (character == null)
            {
                Logger.Error(client, $"CharacterId:{token.CharacterId} not found");
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_DATA_INVALID_CHARACTER_ID);
            }

            Logger.Info(client, "Moved Into GameServer");


            // NTC
            client.Send(new S2CItemExtendItemSlotNtc());
            // client.Send(GameFull.Dump_5);
            //  client.Send(GameFull.Dump_6);

            foreach (var ValidCourse in Server.AssetRepository.GPCourseInfoAsset.ValidCourses)
            {
                S2CGPCourseExtendNtc courseExtendNtc = new S2CGPCourseExtendNtc()
                {
                    CourseID = ValidCourse.Value.Id,
                    ExpiryTimestamp = ValidCourse.Value.EndTime
                };
                client.Send(courseExtendNtc);
            }

            return new S2CConnectionMoveInServerRes();
        }
    }
}

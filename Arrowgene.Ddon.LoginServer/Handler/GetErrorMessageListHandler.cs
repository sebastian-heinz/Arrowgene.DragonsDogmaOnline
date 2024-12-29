using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GetErrorMessageListHandler : LoginRequestPacketHandler<C2LGetErrorMessageListReq, L2CGetErrorMessageListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GetErrorMessageListHandler));

        public GetErrorMessageListHandler(DdonLoginServer server) : base(server)
        {
        }

        public override L2CGetErrorMessageListRes Handle(LoginClient client, C2LGetErrorMessageListReq packet)
        {
            // TODO: Investigate if we can break up the error list to get around the packet length limit.
            L2CGetErrorMessageListNtc ntc = new L2CGetErrorMessageListNtc();

            ntc.ErrorMessages = Server.AssetRepository.ClientErrorCodes;

            client.Send(ntc);
            L2CGetErrorMessageListRes res = new L2CGetErrorMessageListRes();
            return res;
        }
    }
}

using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GetErrorMessageListHandler : StructurePacketHandler<LoginClient, C2LGetErrorMessageListReq>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(GetErrorMessageListHandler));


        public GetErrorMessageListHandler(DdonLoginServer server) : base(server)
        {
        }

        public override void Handle(LoginClient client, StructurePacket<C2LGetErrorMessageListReq> packet)
        {
            L2CGetErrorMessageListNtc ntc = new L2CGetErrorMessageListNtc();
            ntc.ErrorMessages = Server.AssetRepository.ClientErrorCodes;
            client.Send(ntc);

            L2CGetErrorMessageListRes res = new L2CGetErrorMessageListRes();
            client.Send(res);
        }
    }
}

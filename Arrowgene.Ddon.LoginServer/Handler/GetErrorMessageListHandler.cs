using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GetErrorMessageListHandler : StructurePacketHandler<LoginClient, C2LGetErrorMessageListReq>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(GetErrorMessageListHandler));


        public GetErrorMessageListHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GET_ERROR_MESSAGE_LIST_REQ;

        public override void Handle(LoginClient client, StructurePacket<C2LGetErrorMessageListReq> packet)
        {
            L2CGetErrorMessageListNtc ntc = new L2CGetErrorMessageListNtc(Server.AssetRepository.ClientErrorCodes);
            client.Send(ntc);
            L2CGetErrorMessageListRes res = new L2CGetErrorMessageListRes();
            client.Send(res);
        }
    }
}

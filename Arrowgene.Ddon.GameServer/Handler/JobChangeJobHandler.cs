using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobChangeJobHandler : StructurePacketHandler<GameClient, C2SJobChangeJobReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobChangeJobHandler));


        public JobChangeJobHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SJobChangeJobReq> packet)
        {
            S2CJobChangeJobRes response = new S2CJobChangeJobRes();
            client.Send(response);

            S2CJobChangeJobNtc notice = new S2CJobChangeJobNtc();
            notice.CharacterId = client.Character.Id;
            client.Send(notice);

            S2CItemUpdateCharacterItemNtc updateCharacterItemNotice = new S2CItemUpdateCharacterItemNtc();
            client.Send(updateCharacterItemNotice);
        }
    }
}
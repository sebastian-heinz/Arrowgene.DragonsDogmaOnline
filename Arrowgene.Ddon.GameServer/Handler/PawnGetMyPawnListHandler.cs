using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetMyPawnListHandler : StructurePacketHandler<GameClient, C2SPawnGetMypawnListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetMyPawnListHandler));


        public PawnGetMyPawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnGetMypawnListReq> packet)
        {
            client.Send(new S2CPawnGetMypawnListRes() {
                PawnList = Server.AssetRepository.MyPawnAsset.Select((asset, index) => new CDataPawnList() {
                    PawnId = (int) asset.PawnId,
                    SlotNo = (uint) (index+1),
                    Name = asset.Name,
                    Sex = asset.Sex,
                    PawnListData = new CDataPawnListData() {
                        Job = asset.Job,
                        Level = asset.JobLv
                    }
                }).ToList(),
                PartnerInfo = new CDataPartnerPawnInfo() {
                    PawnId = Server.AssetRepository.MyPawnAsset[0].PawnId,
                    Likability = 1,
                    Personality = 1
                },
            });
        }
    }
}

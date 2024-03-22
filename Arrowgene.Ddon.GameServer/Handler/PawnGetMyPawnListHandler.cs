using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
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
            S2CPawnGetMypawnListRes pcap = EntitySerializer.Get<S2CPawnGetMypawnListRes>().Read(SelectedDump.data_Dump_32_A);

            client.Send(new S2CPawnGetMypawnListRes() {
                PawnList = client.Character.Pawns.Select((pawn, index) => new CDataPawnList() {
                    PawnId = (int) pawn.PawnId,
                    SlotNo = (uint) (index+1),
                    Name = pawn.Name,
                    Sex = pawn.EditInfo.Sex,
                    PawnListData = new CDataPawnListData() 
                    {
                        Job = pawn.Job,
                        Level = pawn.ActiveCharacterJobData.Lv,
                        // TODO: Fetch from DB
                        CraftRank = pcap.PawnList[0].PawnListData.CraftRank,
                        PawnCraftSkillList = pcap.PawnList[0].PawnListData.PawnCraftSkillList
                        // TODO: CraftRank, PawnCraftSkillList, CommentSize, LatestReturnDate
                    }
                    // TODO: PawnState, ShareRange, Unk0, Unk1, Unk2
                }).ToList(),
                // TODO: PartnerInfo
                PartnerInfo = new CDataPartnerPawnInfo() {
                    PawnId = client.Character.Pawns.FirstOrDefault()?.PawnId ?? 0,
                    Likability = 1,
                    Personality = 1
                },
            });
        }
    }
}

using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetRentedPawnListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetRentedPawnListHandler));


        public PawnGetRentedPawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_PAWN_GET_RENTED_PAWN_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            // client.Send(InGameDump.Dump_35);
            // client.Send(new Packet(PacketId.S2C_PAWN_GET_RENTED_PAWN_LIST_RES, new byte[] {0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x1, 0x0, 0xB6, 0x58, 0xA9, 0x0, 0x0, 0x0, 0x1, 0x0, 0x4, 0x49, 0x72, 0x69, 0x73, 0x2, 0x0, 0x9, 0x5, 0x6, 0x0, 0x0, 0x0, 0x64, 0x0, 0x0, 0x0, 0x46, 0x0, 0x0, 0x0, 0xA, 0x1, 0x0, 0x0, 0x0, 0x0, 0x2, 0x0, 0x0, 0x0, 0x45, 0x3, 0x0, 0x0, 0x0, 0x0, 0x4, 0x0, 0x0, 0x0, 0x0, 0x5, 0x0, 0x0, 0x0, 0x0, 0x6, 0x0, 0x0, 0x0, 0x0, 0x7, 0x0, 0x0, 0x0, 0x0, 0x8, 0x0, 0x0, 0x0, 0x0, 0x9, 0x0, 0x0, 0x0, 0x0, 0xA, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x6, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xC0, 0xE6, 0x1, 0x7A, 0x80, 0x0, 0x0, 0x0, 0x45, 0xAE, 0xFC}));
            // client.Send(new Packet(PacketId.S2C_PAWN_GET_RENTED_PAWN_LIST_RES, new byte[12]));
            // var pcap = new S2CPawnGetRentedPawnListRes.Serializer().Read(InGameDump.Dump_35.AsBuffer());

            var response = new S2CPawnGetRentedPawnListRes();
            for (int i = 0; i < client.Character.RentedPawns.Count; i++)
            {
                var pawn = client.Character.RentedPawns[i];
                response.RentedPawnList.Add(new CDataRentedPawnList()
                {
                    Name = pawn.Name,
                    PawnId = pawn.PawnId,
                    AdventureCount = 10,
                    CraftCount = 10,
                    PawnType = pawn.PawnType,
                    Sex = pawn.EditInfo.Sex,
                    PawnState = 0,
                    SlotNo = (uint) (i + 1),
                    PawnListData = new CDataPawnListData()
                    {
                        Job = pawn.Job,
                        CraftRank = pawn.CraftData.CraftRank,
                        Level = pawn.ActiveCharacterJobData.Lv,
                        PawnCraftSkillList = pawn.CraftData.PawnCraftSkillList,
                    }
                });
            }

            client.Send(response);
        }
    }
}

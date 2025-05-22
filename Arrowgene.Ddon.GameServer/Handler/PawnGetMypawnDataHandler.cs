using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetMyPawnDataHandler : GameRequestPacketHandler<C2SPawnGetMyPawnDataReq, S2CPawnGetMyPawnDataRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetMyPawnDataHandler));

        private readonly OrbUnlockManager _OrbUnlockManager;
        private readonly CharacterManager _CharacterManager;

        public PawnGetMyPawnDataHandler(DdonGameServer server) : base(server)
        {
            _OrbUnlockManager = server.OrbUnlockManager;
            _CharacterManager = server.CharacterManager;
        }

        public override S2CPawnGetMyPawnDataRes Handle(GameClient client, C2SPawnGetMyPawnDataReq request)
        {
            Pawn pawn = client.Character.PawnBySlotNo(request.SlotNo);

            S2CPawnGetPawnProfileNtc pcap33 = EntitySerializer.Get<S2CPawnGetPawnProfileNtc>().Read(new byte[] {0x0, 0x20, 0xB8, 0xF8, 0x0, 0xDB, 0x3B, 0xCF, 0x0, 0x20, 0xB8, 0xF8, 0x0, 0x5, 0x44, 0x69, 0x61, 0x6E, 0x61, 0x0, 0x6, 0x53, 0x65, 0x65, 0x6C, 0x69, 0x78, 0x0, 0x3, 0x53, 0x3B, 0x52, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x1, 0xB8, 0xC0, 0xC1});
            pcap33.CharacterId = client.Character.CharacterId;
            pcap33.PawnId = pawn.PawnId;
            pcap33.OwnerCharacterId = client.Character.CharacterId;
            pcap33.OwnerFirstName = client.Character.FirstName;
            pcap33.OwnerLastName = client.Character.LastName;
            // TODO: Etc
            client.Send(pcap33);

            S2CPawnGetPawnHistoryInfoNtc pcap34 = EntitySerializer.Get<S2CPawnGetPawnHistoryInfoNtc>().Read(new byte[] {0x0, 0x20, 0xB8, 0xF8, 0x0, 0xDB, 0x3B, 0xCF, 0x0, 0x0, 0x0, 0x0, 0x0, 0x5, 0x44, 0x69, 0x61, 0x6E, 0x61, 0x0, 0x6, 0x53, 0x65});
            pcap34.CharacterId = client.Character.CharacterId;
            pcap34.PawnId = pawn.PawnId;
            // TODO: Etc
            client.Send(pcap34);

            S2CPawnGetPawnTotalScoreInfoNtc pcap35 = EntitySerializer.Get<S2CPawnGetPawnTotalScoreInfoNtc>().Read(new byte[] {0x0, 0x20, 0xB8, 0xF8, 0x0, 0xDB, 0x3B, 0xCF, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x24, 0x0, 0x0, 0x0, 0x3, 0x0, 0x0, 0x0, 0x0, 0x6C, 0x69, 0x78, 0x0, 0x3, 0x53, 0x3B, 0x52, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0});
            pcap35.CharacterId = client.Character.CharacterId;
            pcap35.PawnId = pawn.PawnId;
            // TODO: Etc
            client.Send(pcap35);

            S2CPawnGetPawnOrbDevoteInfoNtc orbNtc = new S2CPawnGetPawnOrbDevoteInfoNtc()
            {
                CharacterId = client.Character.CharacterId,
                PawnId = pawn.PawnId,
                OrbPageStatusList = _OrbUnlockManager.GetOrbPageStatus(pawn),
                JobOrbTreeStatusList = Server.JobOrbUnlockManager.GetJobOrbTreeStatus(client.Character)
            };
            client.Send(orbNtc);

            var res = new S2CPawnGetMyPawnDataRes();
            res.PawnId = pawn.PawnId;
            GameStructure.CDataPawnInfo(res.PawnInfo, pawn);
            res.PawnInfo.AbilityCostMax = _CharacterManager.GetMaxAugmentAllocation(pawn);

            return res;
        }
    }
}

using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestPlayEndHandler : GameRequestPacketHandler<C2SQuestPlayEndReq, S2CQuestPlayEndRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestPlayEndHandler));

        public QuestPlayEndHandler(DdonGameServer server) : base(server)
        {
        }

        // C2S_QUEST_PLAY_END_REQ (C2SQuestPlayEndReq)
        // S2C_QUEST_11_117_16_NTC
        // S2C_QUEST_PLAY_END_RES (S2CQuestPlayEndRes)
        // S2C_QUEST_11_45_16_NTC (S2CQuestPlayEndNtc)
        // <item update>
        // ..
        // C2S_STAGE_AREA_CHANGE
        // C2S_QUEST_11_60_16_NTC
        // <party leave>
        // <party create>
        // Party "ContentNumber" has changed: 17180184836 -> 4294967297

        public override S2CQuestPlayEndRes Handle(GameClient client, C2SQuestPlayEndReq request)
        {
            var groupId = Server.ExmManager.GetContentIdForCharacter(client.Character);
            var quest = Server.ExmManager.GetQuestForContent(groupId);

            var ntc = new S2CQuestPlayEndNtc();
            ntc.ContentsPlayEnd.RewardItemDetailList = quest.ToCDataTimeGainQuestList(0).RewardItemDetailList;
            client.Send(ntc);

            // PacketId.S2C_QUEST_11_117_16_NTC
            // ushort
            // ushort
            // byte

            // PacketId.C2S_QUEST_11_60_16_NTC

            // TODO: Clean up state from ExmManager?

            return new S2CQuestPlayEndRes();

            // Send S2CQuestPlayEndNtc (S2C_QUEST_11_45_16_NTC)
            // Send 0x7f4e10
        }
    }
}

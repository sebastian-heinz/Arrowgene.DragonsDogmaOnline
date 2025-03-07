using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Instance;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceSetOmInstantKeyValueHandler : GameRequestPacketHandler<C2SInstanceSetOmInstantKeyValueReq, S2CInstanceSetOmInstantKeyValueRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceSetOmInstantKeyValueHandler));

        public InstanceSetOmInstantKeyValueHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInstanceSetOmInstantKeyValueRes Handle(GameClient client, C2SInstanceSetOmInstantKeyValueReq request)
        {
            Logger.Debug($"OM: Key=0x{request.Key:x16}, Value=0x{request.Value:x8}");

            OmManager.SetOmData(client.Party.InstanceOmData, client.Character.Stage.Id, request.Key, request.Value);

            S2CInstanceSetOmInstantKeyValueNtc ntc = new S2CInstanceSetOmInstantKeyValueNtc();
            ntc.StageId = client.Character.Stage.Id;
            ntc.Key = request.Key;
            ntc.Value = request.Value;
            client.Party.SendToAll(ntc);

            S2CInstanceSetOmInstantKeyValueRes res = new S2CInstanceSetOmInstantKeyValueRes();
            res.StageId = client.Character.Stage.Id;
            res.Key = request.Key;
            res.Value = request.Value;

            // Check for OM callbacks in the quest
            foreach (var questScheduleId in client.Party.QuestState.GetActiveQuestScheduleIds())
            {
                var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                if (quest != null)
                {
                    quest.HandleOmInstantValue(client, request.Key, request.Value);
                }
            }

            return res;
        }
    }
}

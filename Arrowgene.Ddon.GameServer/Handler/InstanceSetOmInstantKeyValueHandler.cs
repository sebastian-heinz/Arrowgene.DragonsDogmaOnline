using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Instance;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceSetOmInstantKeyValueHandler : StructurePacketHandler<GameClient, C2SInstanceSetOmInstantKeyValueReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceSetOmInstantKeyValueHandler));

        public InstanceSetOmInstantKeyValueHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceSetOmInstantKeyValueReq> req)
        {
            OmManager.SetOmData(client.Party.InstanceOmData, client.Character.Stage.Id, req.Structure.Key, req.Structure.Value);

            S2CInstanceSetOmInstantKeyValueNtc ntc = new S2CInstanceSetOmInstantKeyValueNtc();
            ntc.StageId = client.Character.Stage.Id;
            ntc.Key = req.Structure.Key;
            ntc.Value = req.Structure.Value;
            client.Party.SendToAll(ntc);

            S2CInstanceSetOmInstantKeyValueRes res = new S2CInstanceSetOmInstantKeyValueRes();
            res.StageId = client.Character.Stage.Id;
            res.Key = req.Structure.Key;
            res.Value = req.Structure.Value;
            client.Send(res);

            // Check for OM callbacks in the quest
            foreach (var questScheduleId in client.Party.QuestState.GetActiveQuestScheduleIds())
            {
                var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                if (quest != null)
                {
                    quest.HandleOmInstantValue(client, req.Structure.Key, req.Structure.Value);
                }
            }
        }
    }
}

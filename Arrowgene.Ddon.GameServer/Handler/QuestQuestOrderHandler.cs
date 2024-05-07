using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestQuestOrderHandler : GameStructurePacketHandler<C2SQuestQuestOrderReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestQuestOrderHandler));

        public QuestQuestOrderHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestQuestOrderReq> packet)
        {
            var res = new S2CQuestQuestOrderRes();

            // TODO: Make this configurable
            switch(packet.Structure.QuestScheduleId)
            {
                case 40000035:
                    // A Personal Request
                    res.QuestProcessStateList.Add(new CDataQuestProcessState()
                    {
                        CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                        {
                            QuestManager.CheckCommand.DeliverItem(13805, 1)
                        }),
                        ResultCommandList = new List<CDataQuestCommand>()
                        {
                            QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Accept)
                        }
                    });
                    break;

                case 50300010:
                    // Spirit Dragon EM as a Light Quest, yes
                    res.QuestProcessStateList.Add(new CDataQuestProcessState()
                    {
                        CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                        { 
                            // EmHpLess
                            QuestManager.CheckCommand.EmHpLess(StageNo.SpiritDragonsRoost2, 2, 0, 60)
                        }),
                        ResultCommandList = new List<CDataQuestCommand>()
                        {
                            QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Accept),
                            QuestManager.ResultCommand.StageJump(StageNo.SpiritDragonsRoost2, 0),
                            QuestManager.ResultCommand.ExeEventAfterStageJump(StageNo.SpiritDragonsRoost2, 0, 0)
                        }
                    });
                    break;
            }

            client.Send(res);
        }
    }
}

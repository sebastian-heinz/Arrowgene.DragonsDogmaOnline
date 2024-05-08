using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Quests.PersonalQuests
{
    internal class APersonalRequest
    {
    }
}

#if false
            S2CQuestGetLightQuestListRes pcap = EntitySerializer.Get<S2CQuestGetLightQuestListRes>().Read(pcap_data);
            S2CQuestGetLightQuestListRes res = new S2CQuestGetLightQuestListRes();

            // TODO: Make these configurable

            // Spirit Dragon EM
            res.LightQuestList.Add(new CDataLightQuestList()
            {
                Param = new CDataQuestList() {
                    QuestScheduleId = 50300010,
                    QuestId = 50300010
                }
            });
            
            // A Personal Request
            res.LightQuestList.Add(new CDataLightQuestList()
            {
                Param = new CDataQuestList()
                {
                    QuestScheduleId = 40000035,
                    QuestId = 40000035
                }
            });
---------------------------------
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
#endif

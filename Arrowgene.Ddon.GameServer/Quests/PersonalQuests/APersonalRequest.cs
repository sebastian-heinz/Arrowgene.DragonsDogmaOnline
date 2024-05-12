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
----------------------------------
               case 40000035:
                {
                    // A Personal Request
                    S2CQuestQuestProgressRes res = new S2CQuestQuestProgressRes();
                    res.QuestScheduleId = packet.Structure.QuestScheduleId;
                    switch (packet.Structure.ProcessNo)
                    {
                        case 0:
                            res.QuestProcessStateList = new List<CDataQuestProcessState>()
                            {
                                new CDataQuestProcessState()
                                {
                                    ProcessNo = 1,
                                    ResultCommandList = new List<CDataQuestCommand>()
                                    {
                                        CDataQuestCommand.ResultSetAnnounce(CDataQuestCommand.AnnounceType.QUEST_ANNOUNCE_TYPE_CLEAR)
                                    }
                                }
                            };
                            break;
                    }

                    client.Send(res);

                    // Sent for the rest of the party members
                    S2CQuestQuestProgressNtc ntc = new S2CQuestQuestProgressNtc();
                    ntc.ProgressCharacterId = packet.Structure.ProgressCharacterId;
                    ntc.QuestScheduleId = res.QuestScheduleId;
                    ntc.QuestProcessStateList = res.QuestProcessStateList;
                    client.Party.SendToAllExcept(ntc, client);
                    break;
                }
#endif

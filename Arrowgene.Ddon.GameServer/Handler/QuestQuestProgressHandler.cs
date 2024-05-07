using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Quests.Missions;
using Arrowgene.Ddon.GameServer.Quests.WorldQuests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestQuestProgressHandler : StructurePacketHandler<GameClient, C2SQuestQuestProgressReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestQuestProgressHandler));

        private readonly ExpManager _ExpManager;

        public QuestQuestProgressHandler(DdonGameServer server) : base(server)
        {
            _ExpManager = server.ExpManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestQuestProgressReq> packet)
        {

            S2CQuestQuestProgressRes res = new S2CQuestQuestProgressRes();
            res.QuestScheduleId = packet.Structure.QuestScheduleId;
            res.QuestProgressResult = 0;

            Logger.Debug($"KeyId={packet.Structure.KeyId} ProgressCharacterId={packet.Structure.ProgressCharacterId}, QuestScheduleId={packet.Structure.QuestScheduleId}, ProcessNo={packet.Structure.ProcessNo}\n");

            // TODO: These can be saved in some dictionary
            // TODO: Extra params for which character it is for
            // TODO: Make some interface they adhere to
            // TODO: Other quests happen here as well, this is the main quest handler
            // -- Missions
            // -- World Quests
            // -- Personal Requests
            // -- Spirit Dragon
            QuestProcess process = QuestProcess.ExecuteCommand;
            if (packet.Structure.QuestScheduleId == 287350)
            {
                Logger.Debug("============ Main Story Quest activated ============");
                Logger.Debug($"ProcessNo = {packet.Structure.ProcessNo}");
                res.QuestProcessState = Mq000002_TheSlumberingGod.StateMachineExecute(packet.Structure.KeyId, packet.Structure.QuestScheduleId, packet.Structure.ProcessNo);
            }
            else if (packet.Structure.QuestScheduleId == 20005010)
            {
                Logger.Debug("============ World Quest activated ============");
                Logger.Debug($"ProcessNo = {packet.Structure.ProcessNo}");
                res.QuestProcessState = WorldQuests.LestaniaCyclops.StateMachineExecute(client, packet.Structure.KeyId, packet.Structure.QuestScheduleId, packet.Structure.ProcessNo, out process, _ExpManager);
            }
            else
            {
                List<CDataQuestCommand> ResultCommandList = new List<CDataQuestCommand>();
                ResultCommandList.Add(new CDataQuestCommand()
                {
                    Command = (ushort)QuestCommandCheckType.IsEndTimer,
                    Param01 = 0x173
                });

                res.QuestScheduleId = 0x32f00;
                res.QuestProcessState.Add(new CDataQuestProcessState()
                {
                    ProcessNo = 0x1b,
                    SequenceNo = 0x1,
                    BlockNo = 0x2,
                    ResultCommandList = ResultCommandList
                });
            }

            if (process == QuestProcess.ProcessEnd)
            {
                S2CItemUpdateCharacterItemNtc rewardNtc = new S2CItemUpdateCharacterItemNtc();

                rewardNtc.UpdateType = (ushort) ItemNoticeType.Quest;
                rewardNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint() { Type = WalletType.Gold, AddPoint = 390 });
                rewardNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint() { Type = WalletType.RiftPoints, AddPoint = 70 });
                client.Send(rewardNtc);
            }

            S2CQuestQuestProgressNtc ntc = new S2CQuestQuestProgressNtc()
            {
                ProgressCharacterId = packet.Structure.ProgressCharacterId,
                QuestScheduleId = res.QuestScheduleId,
                QuestProcessStateList = res.QuestProcessState,
            };
            client.Party.SendToAllExcept(ntc, client);

            client.Send(res);


#if false
            if (process == QuestProcess.ProcessEnd)
            {
                var ntccomplete = new S2CQuestCompleteNtc() { QuestScheduleId = packet.Structure.QuestScheduleId };
                client.Party.SendToAll(ntccomplete);
            }
#endif
#if false
            // Logger.Debug($"CharacterId={req.Structure.CharacterId}, QuestScheduleId={req.Structure.QuestScheduleId}, ProcessNo={req.Structure.ProcessNo}\n");

            IBuffer inBuffer = new StreamBuffer(packet.Data);
            inBuffer.SetPositionStart();
            uint data0 = inBuffer.ReadUInt32(Endianness.Big);
            uint data1 = inBuffer.ReadUInt32(Endianness.Big);
            uint data2 = inBuffer.ReadUInt32(Endianness.Big);
            Logger.Debug("data0: "+data0+" data1: "+data1+" data2: "+data2+"\n");
            if(data2 == 287350){
                client.Send(GameFull.Dump_652);
            }
            else
            {
                IBuffer outBuffer = new StreamBuffer();
                outBuffer.WriteInt32(0, Endianness.Big);
                outBuffer.WriteInt32(0, Endianness.Big);
                outBuffer.WriteByte(0); // QuestProgressResult
                outBuffer.WriteUInt32(data2, Endianness.Big); // QuestScheduleId
                outBuffer.WriteUInt32(0, Endianness.Big); // QuestProgressStateList
                                                          //client.Send(new Packet(PacketId.S2C_QUEST_QUEST_PROGRESS_RES, outBuffer.GetAllBytes()));
                // client.Send(GameFull.Dump_166);
                // client.Send(GameFull.Dump_168);
                // client.Send(GameFull.Dump_170);
                // client.Send(GameFull.Dump_172);
                // client.Send(GameFull.Dump_175);
                // client.Send(GameFull.Dump_177);
                // client.Send(GameFull.Dump_179);
                // client.Send(GameFull.Dump_181);
                // client.Send(GameFull.Dump_185);
                // client.Send(GameFull.Dump_188);
                // client.Send(GameFull.Dump_190);
                // client.Send(GameFull.Dump_294);
                // client.Send(GameFull.Dump_297);
                // client.Send(GameFull.Dump_299);
                client.Send(GameFull.Dump_524);
            }
#endif
        }
    }
}

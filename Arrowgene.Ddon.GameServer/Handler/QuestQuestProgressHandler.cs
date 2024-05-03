using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestQuestProgressHandler : StructurePacketHandler<GameClient, C2SQuestQuestProgressReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestQuestProgressHandler));


        public QuestQuestProgressHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestQuestProgressReq> packet)
        {

            S2CQuestQuestProgressRes res = new S2CQuestQuestProgressRes();
            res.QuestScheduleId = packet.Structure.QuestScheduleId;
            res.QuestProgressResult = 0;

            Logger.Debug($"KeyId={packet.Structure.KeyId} ProgressCharacterId={packet.Structure.ProgressCharacterId}, QuestScheduleId={packet.Structure.QuestScheduleId}, ProcessNo={packet.Structure.ProcessNo}\n");

            if (packet.Structure.KeyId == 1337)
            {
                switch (packet.Structure.ProcessNo)
                {
                    case 0:
                        res.QuestProcessState = new List<CDataQuestProcessState>()
                        {
                            new CDataQuestProcessState() {
                                ProcessNo = 1, SequenceNo = 0x0, BlockNo = 0x0,
                                CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                                {
                                    QuestManager.CheckCommand.DummyNotProgress(),
                                    QuestManager.CheckCommand.IsStageNo(StageNo.AudienceChamber),
                                }),
                                ResultCommandList = new List<CDataQuestCommand>()
                                {
                                    // QuestManager.ResultCommand.EventExec(StageNo.WhiteDragonTemple, 1, StageNo.WhiteDragonTemple, 0x20),
                                    // QuestManager.ResultCommand.SetCheckPoint(),
                                    // Event numbers are files based on st<stageNo>ev<eventNo> in nativepc/rom/event and nativepc/movie
                                    // QuestManager.ResultCommand.EventExec(StageNo.WhiteDragonTemple, 0, StageNo.WhiteDragonTemple, 0x1),
                                    // QuestManager.ResultCommand.SetAnnounceType(QuestAnnounceType.Accept),
                                    // QuestManager.ResultCommand.MyQstFlagOn(0x1234)
                                    QuestManager.ResultCommand.EventExec(StageNo.WhiteDragonTemple, 0, StageNo.WhiteDragonTemple, 0x1),
                                    QuestManager.ResultCommand.SetAnnounceType(QuestAnnounceType.Accept),
                                    QuestManager.ResultCommand.MyQstFlagOn(0x1234),
                                }
                            }
                        };
                        break;
                    case 1:
                        res.QuestProcessState = new List<CDataQuestProcessState>()
                        {
                            new CDataQuestProcessState() {
                                ProcessNo = 2, SequenceNo = 0x0, BlockNo = 0x0,
                                ResultCommandList = new List<CDataQuestCommand>()
                                {
                                    QuestManager.ResultCommand.SetAnnounceType(QuestAnnounceType.Clear),
                                    // QuestManager.ResultCommand.EventExec(StageNo.WhiteDragonTemple, 1, StageNo.WhiteDragonTemple, 0x20),
                                    // QuestManager.ResultCommand.SetCheckPoint(),
                                    // QuestManager.ResultCommand.SetAnnounceType(QuestAnnounceType.Accept)
                                }
                            }
                        };
                        break;
                }

                S2CQuestQuestProgressNtc ntc = new S2CQuestQuestProgressNtc()
                {
                    ProgressCharacterId = client.Character.CharacterId, // packet.Structure.ProgressCharacterId,
                    QuestScheduleId = res.QuestScheduleId,
                    QuestProcessStateList = res.QuestProcessState
                };
                client.Party.SendToAllExcept(ntc, client);
            }
            else
            {
                List<CDataQuestCommand> ResultCommandList = new List<CDataQuestCommand>();
                ResultCommandList.Add(new CDataQuestCommand()
                {
                    Command = (ushort)QuestCommandCheckType.IsEndTimer,
                    Param01 = 0x173
                });

                res.QuestProgressResult = 0;
                res.QuestScheduleId = 0x32f00;
                res.QuestProcessState.Add(new CDataQuestProcessState()
                {
                    ProcessNo = 0x1b,
                    SequenceNo = 0x1,
                    BlockNo = 0x2,
                    ResultCommandList = ResultCommandList
                });
            }

            client.Send(res);

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

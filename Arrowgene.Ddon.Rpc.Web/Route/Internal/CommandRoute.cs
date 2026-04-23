using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Logging;
using Arrowgene.WebServer;
using Microsoft.AspNetCore.Hosting.Server;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Rpc.Web.Route.Internal
{
    public class CommandRoute : RpcRouteTemplate
    {
        public class InternalCommand : RpcBodyCommand<RpcUnwrappedObject>
        {
            private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InternalCommand));

            public InternalCommand(RpcUnwrappedObject entry) : base(entry)
            {
            }

            public override string Name => "InternalCommand";

            public override RpcCommandResult Execute(DdonGameServer gameServer)
            {
                return _entry.Command switch
                {
                    RpcInternalCommand.Ping => HandlePing(),
                    RpcInternalCommand.NotifyPlayerList => HandleNotifyPlayerList(gameServer),
                    RpcInternalCommand.NotifyClanQuestCompletion => HandleNotifyClanQuestCompletion(gameServer),
                    RpcInternalCommand.EpitaphRoadWeeklyReset => HandleEpitaphRoadWeeklyReset(gameServer),
                    RpcInternalCommand.KickInternal => HandleKickInternal(gameServer),
                    RpcInternalCommand.AreaRankResetStart => HandleAreaRankResetStart(gameServer),
                    RpcInternalCommand.AreaRankResetEnd => HandleAreaRankResetEnd(gameServer),
                    RpcInternalCommand.BoardQuestDailyRotation => HandleBoardQuestDailyRotation(gameServer),
                    RpcInternalCommand.StampReset => HandleStampReset(gameServer),
                    RpcInternalCommand.UpdateCrafting => HandleUpdateCrafting(gameServer),
                    RpcInternalCommand.WorldQuestReset => HandleWorldQuestReset(gameServer),
                    _ => new RpcCommandResult(this, false),
                };
            }

            private RpcCommandResult HandlePing()
            {
                return new RpcCommandResult(this, true)
                {
                    Message = $"Ping {_entry.Origin}"
                };
            }

            private RpcCommandResult HandleNotifyPlayerList(DdonGameServer gameServer)
            {
                gameServer.RpcManager.UpdatePlayerList();

                return new RpcCommandResult(this, true)
                {
                    Message = $"NotifyPlayerList Channel {_entry.Origin}"
                };
            }

            private RpcCommandResult HandleNotifyClanQuestCompletion(DdonGameServer gameServer)
            {
                RpcQuestCompletionData data = _entry.GetData<RpcQuestCompletionData>();
                gameServer.ClanManager.UpdateClanQuestCompletion(data.CharacterId, data.QuestStatus);
                return new RpcCommandResult(this, true)
                {
                    Message = $"NotifyClanQuestCompletion for CharacterId {data.CharacterId}"
                };
            }

            private RpcCommandResult HandleEpitaphRoadWeeklyReset(DdonGameServer gameServer)
            {
                gameServer.EpitaphRoadManager.PerformWeeklyReset();
                return new RpcCommandResult(this, true)
                {
                    Message = _entry.Command.ToString()
                };
            }

            private RpcCommandResult HandleKickInternal(DdonGameServer gameServer)
            {
                int target = _entry.GetData<int>();
                var clientList = gameServer.ClientLookup.GetAll();
                foreach (var client in clientList)
                {
                    if (client.Account?.Id == target)
                    {
                        Logger.Error(client, $"[AUTOKICK] Handling auto kick for account {target}");
                        client.Close();
                    }
                }
                gameServer.Database.DeleteConnection(gameServer.Id, target);
                return new RpcCommandResult(this, true)
                {
                    Message = $"KickInternal for AccountId {target}"
                };
            }

            private RpcCommandResult HandleAreaRankResetStart(DdonGameServer gameServer)
            {
                foreach (var character in gameServer.ClientLookup.GetAllCharacter())
                {
                    foreach ((var area, var rank) in character.AreaRanks)
                    {
                        lock (rank)
                        {
                            rank.LastWeekPoint = rank.WeekPoint;
                            rank.WeekPoint = 0;
                        }
                    }
                    character.AreaSupply.Clear();
                }

                return new RpcCommandResult(this, true)
                {
                    Message = _entry.Command.ToString()
                };
            }

            private RpcCommandResult HandleAreaRankResetEnd(DdonGameServer gameServer)
            {
                gameServer.Database.ExecuteInTransaction(connection =>
                {
                    foreach (var character in gameServer.ClientLookup.GetAllCharacter())
                    {
                        character.AreaSupply = gameServer.Database.SelectAreaRankSupply(character.CharacterId, connection);
                    }
                });
                return new RpcCommandResult(this, true)
                {
                    Message = _entry.Command.ToString()
                };
            }

            private RpcCommandResult HandleBoardQuestDailyRotation(DdonGameServer gameServer)
            {
                var questRecords = gameServer.Database.SelectLightQuestRecords();
                var extantQuests = QuestManager.GetQuestsByType(QuestType.Light);

                var quests = questRecords
                    .Where(x => !extantQuests.Contains(x.QuestScheduleId))
                    .Select(x => gameServer.LightQuestManager.GenerateQuestFromRecord(x));

                QuestManager.AddQuests(gameServer, quests);

                foreach (var character in gameServer.ClientLookup.GetAllCharacter())
                {
                    foreach (var key in character.CompletedQuests.Keys
                        .Where(QuestManager.IsBoardQuest)
                        .ToList())
                    {
                        character.CompletedQuests.Remove(key);
                    }
                }

                return new RpcCommandResult(this, true)
                {
                    Message = _entry.Command.ToString()
                };
            }

            private RpcCommandResult HandleStampReset(DdonGameServer gameServer)
            {
                foreach (var character in gameServer.ClientLookup.GetAllCharacter())
                {
                    gameServer.StampManager.RefreshStamp(character);
                }

                return new RpcCommandResult(this, true)
                {
                    Message = _entry.Command.ToString()
                };
            }

            private RpcCommandResult HandleUpdateCrafting(DdonGameServer gameServer)
            {
                gameServer.CraftManager.UpdateOnlineCraftingProgress();
                return new RpcCommandResult(this, true);


            }
            private RpcCommandResult HandleWorldQuestReset(DdonGameServer gameServer)
            {
                long seed = _entry.GetData<long>();
                gameServer.WorldQuestManager.PerformReset(seed);
                return new RpcCommandResult(this, true)
                {
                    Message = $"WorldQuestReset with seed {seed}"
                };
            }
        }

        public CommandRoute(IRpcExecuter executer) : base(executer)
        {
        }

        public override string Route => "/rpc/internal/command";

        public async override Task<WebResponse> Post(WebRequest request)
        {
            return await HandleBody<RpcUnwrappedObject, InternalCommand>(request);
        }
    }
}

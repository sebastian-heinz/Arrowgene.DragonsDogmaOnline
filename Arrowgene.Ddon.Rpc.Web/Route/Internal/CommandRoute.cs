using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Logging;
using Arrowgene.WebServer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Rpc.Web.Route.Internal
{
    public class CommandRoute : RpcRouteTemplate
    {
        public class InternalCommand : RpcBodyCommand<RpcUnwrappedObject>
        {
            private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(InternalCommand));

            public InternalCommand(RpcUnwrappedObject entry) : base(entry)
            {
            }

            public override string Name => "InternalCommand";

            public override RpcCommandResult Execute(DdonGameServer gameServer)
            {
                switch (_entry.Command)
                {
                    case RpcInternalCommand.NotifyPlayerList:
                        {
                            List<RpcCharacterData> data = _entry.GetData<List<RpcCharacterData>>();
                            gameServer.RpcManager.ReceivePlayerList(_entry.Origin, _entry.Timestamp, data);
                            return new RpcCommandResult(this, true)
                            {
                                Message = $"NotifyPlayerList Channel {_entry.Origin}"
                            };
                        }
                    case RpcInternalCommand.NotifyClanQuestCompletion:
                        {
                            RpcQuestCompletionData data = _entry.GetData<RpcQuestCompletionData>();
                            gameServer.ClanManager.UpdateClanQuestCompletion(data.CharacterId, data.QuestStatus);
                            return new RpcCommandResult(this, true)
                            {
                                Message = $"NotifyClanQuestCompletion for CharacterId {data.CharacterId}"
                            };
                        }
                    case RpcInternalCommand.EpitaphRoadWeeklyReset:
                        {
                            gameServer.EpitaphRoadManager.PerformWeeklyReset();
                            return new RpcCommandResult(this, true);
                        }
                    case RpcInternalCommand.KickInternal:
                        {
                            uint target = _entry.GetData<uint>();
                            foreach (var client in gameServer.ClientLookup.GetAll())
                            {
                                if (client.Account.Id == target)
                                {
                                    client.Close();
                                }
                            }
                            return new RpcCommandResult(this, true);
                        }
                    default:
                        return new RpcCommandResult(this, false);
                }
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

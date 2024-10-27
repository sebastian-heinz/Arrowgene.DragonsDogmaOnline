using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.WebServer;
using System.Threading.Tasks;
using static Arrowgene.Ddon.GameServer.RpcManager;

namespace Arrowgene.Ddon.Rpc.Web.Route.Internal
{
    public class TrackingRoute : RpcRouteTemplate
    {
        public class InternalTrackingCommand : RpcBodyCommand<RpcUnwrappedObject>
        {
            private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(InternalTrackingCommand));

            public InternalTrackingCommand(RpcUnwrappedObject entry) : base(entry)
            {
            }

            public override string Name => "InternalTrackingCommand";

            public override RpcCommandResult Execute(DdonGameServer gameServer)
            {
                switch (_entry.Command)
                {
                    case RpcInternalCommand.NotifyPlayerLeave:
                        {
                            CharacterSummary data = _entry.GetData<CharacterSummary>();
                            gameServer.RpcManager.RemovePlayerSummary(_entry.Origin, data.CharacterId);
                            return new RpcCommandResult(this, true);
                        }
                    case RpcInternalCommand.NotifyPlayerJoin:
                        {
                            CharacterSummary data = _entry.GetData<CharacterSummary>();
                            gameServer.RpcManager.AddPlayerSummary(_entry.Origin, data);
                            return new RpcCommandResult(this, true);
                        }
                    default:
                        return new RpcCommandResult(this, false);
                }
            }
        }

        public TrackingRoute(IRpcExecuter executer) : base(executer)
        {
        }

        public override string Route => "/rpc/internal/tracking";

        public async override Task<WebResponse> Post(WebRequest request)
        {
            return await HandleBody<RpcUnwrappedObject, InternalTrackingCommand>(request);
        }
    }
}

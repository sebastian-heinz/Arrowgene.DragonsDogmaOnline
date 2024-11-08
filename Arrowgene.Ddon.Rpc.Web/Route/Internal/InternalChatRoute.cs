using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.GameServer.Chat;
using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Logging;
using Arrowgene.WebServer;
using System.Linq;
using System.Threading.Tasks;
using static Arrowgene.Ddon.GameServer.RpcManager;

namespace Arrowgene.Ddon.Rpc.Web.Route.Internal
{
    public class InternalChatRoute : RpcRouteTemplate
    {
        public class InternalChatCommand : RpcBodyCommand<RpcUnwrappedObject>
        {
            private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(InternalChatCommand));

            public InternalChatCommand(RpcUnwrappedObject entry) : base(entry)
            {
            }

            public override string Name => "InternalChatCommand";

            public override RpcCommandResult Execute(DdonGameServer gameServer)
            {
                switch (_entry.Command)
                {
                    case RpcInternalCommand.SendClanMessage:
                        {
                            RpcChatData data = _entry.GetData<RpcChatData>();

                            if (data.SourceData.ClanId == 0)
                            {
                                return new RpcCommandResult(this, false)
                                {
                                    Message = $"SendClanMessage ID {data.SourceData.CharacterId} ClanID {data.SourceData.ClanId}"
                                };
                            }

                            ChatResponse response = new ChatResponse()
                            {
                                HandleId = 0,
                                Deliver = false,
                                Type = data.Type,
                                MessageFlavor = data.MessageFlavor,
                                PhrasesCategory = data.PhrasesCategory,
                                PhrasesIndex = data.PhrasesIndex,
                                CharacterId = data.SourceData.CharacterId,
                                Message = data.Message,
                                FirstName = data.SourceData.FirstName,
                                LastName = data.SourceData.LastName,
                                ClanName = data.SourceData.ClanName,
                            };

                            response.Recipients.AddRange(gameServer.ClientLookup.GetAll().Where(
                                x => x.Character != null
                                && x.Character.ClanId == data.SourceData.ClanId)
                            );

                            gameServer.ChatManager.Send(response);

                            return new RpcCommandResult(this, true)
                            {
                                Message = $"SendClanMessage ID {data.SourceData.CharacterId} ClanID {data.SourceData.ClanId}"
                            };
                        }
                    case RpcInternalCommand.SendTellMessage:
                        {
                            RpcChatData data = _entry.GetData<RpcChatData>();

                            GameClient recipient = gameServer.ClientLookup.GetClientByCharacterId(data.TargetData.CharacterId);

                            if (recipient == null)
                            {
                                return new RpcCommandResult(this, false)
                                {
                                    Message = $"SendClanMessage ID {data.SourceData.CharacterId} -> {data.TargetData.CharacterId}"
                                };
                            }

                            ChatResponse response = new ChatResponse
                            {
                                HandleId = 0,
                                Deliver = false,
                                FirstName = data.SourceData.FirstName,
                                LastName = data.SourceData.LastName,
                                ClanName = data.SourceData.ClanName,
                                CharacterId = data.SourceData.CharacterId,
                                Type = LobbyChatMsgType.Tell,
                                Message = data.Message,
                                MessageFlavor = data.MessageFlavor,
                                PhrasesCategory = data.PhrasesCategory,
                                PhrasesIndex = data.PhrasesIndex
                            };

                            response.Recipients.Add(recipient);
                            gameServer.ChatManager.Send(response);

                            return new RpcCommandResult(this, true)
                            {
                                Message = $"SendTellMessage ID {data.SourceData.CharacterId} -> {data.TargetData.CharacterId}"
                            };
                        }
                    default:
                        return new RpcCommandResult(this, false);
                }
            }
        }

        public InternalChatRoute(IRpcExecuter executer) : base(executer)
        {
        }

        public override string Route => "/rpc/internal/chat";

        public async override Task<WebResponse> Post(WebRequest request)
        {
            return await HandleBody<RpcUnwrappedObject, InternalChatCommand>(request);
        }
    }
}

using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.GameServer.Chat;
using Arrowgene.Ddon.Rpc.Command;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Logging;
using Arrowgene.WebServer;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Arrowgene.Ddon.Rpc.Web.Route.Internal
{
    public class InternalChatRoute(IRpcExecuter executer) : RpcRouteTemplate(executer)
    {
        public class InternalChatCommand(RpcUnwrappedObject entry) : RpcBodyCommand<RpcUnwrappedObject>(entry)
        {
            private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(InternalChatCommand));

            public override string Name => "InternalChatCommand";

            public override RpcCommandResult Execute(DdonGameServer gameServer)
            {
                return _entry.Command switch
                {
                    RpcInternalCommand.SendClanMessage => HandleClanMessage(gameServer),
                    RpcInternalCommand.SendTellMessage => HandleTellMessage(gameServer),
                    RpcInternalCommand.SendShoutMessage => HandleShoutMessage(gameServer),
                    RpcInternalCommand.SendGroupMessage => HandleGroupMessage(gameServer),
                    RpcInternalCommand.SendMail => HandleSendMail(gameServer),
                    RpcInternalCommand.JoinGroupChat => HandleJoinGroupChat(gameServer),
                    RpcInternalCommand.LeaveGroupChat => HandleLeaveGroupChat(gameServer),
                    _ => new RpcCommandResult(this, false),
                };
            }

            private RpcCommandResult HandleClanMessage(DdonGameServer gameServer)
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

            private RpcCommandResult HandleTellMessage(DdonGameServer gameServer)
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

            private RpcCommandResult HandleSendMail(DdonGameServer gameServer)
            {
                RpcChatData data = _entry.GetData<RpcChatData>();

                GameClient receiverClient = gameServer.ClientLookup.GetClientByCharacterId(data.TargetData.CharacterId);

                receiverClient?.Send(new S2CMailMailSendNtc()
                {
                    MailInfo = new()
                    {
                        Id = data.HandleId,
                        State = MailState.Unopened,
                        BaseInfo = data.SourceData.CommunityCharacterBaseInfo,
                        SenderName = data.SourceData.CommunityCharacterBaseInfo.CharacterName.ToString(),
                        MailText = data.Message,
                        SenderDate = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                    }
                });

                return new RpcCommandResult(this, true)
                {
                    Message = $"SendMail {data.SourceData.FirstName} {data.SourceData.LastName} > {data.TargetData.CharacterId}"
                };
            }
        
            private RpcCommandResult HandleShoutMessage(DdonGameServer gameServer)
            {
                RpcChatData data = _entry.GetData<RpcChatData>();

                ChatResponse response = new()
                {
                    HandleId = 0,
                    Deliver = false,
                    FirstName = data.SourceData.FirstName,
                    LastName = data.SourceData.LastName,
                    ClanName = data.SourceData.ClanName,
                    CharacterId = data.SourceData.CharacterId,
                    Type = LobbyChatMsgType.Shout,
                    Message = data.Message,
                    MessageFlavor = data.MessageFlavor,
                    PhrasesCategory = data.PhrasesCategory,
                    PhrasesIndex = data.PhrasesIndex
                };

                response.Recipients.AddRange(gameServer.ClientLookup
                    .GetAll()
                    .Where(x => x.Character != null)
                );
                gameServer.ChatManager.Send(response);

                return new RpcCommandResult(this, true)
                {
                    Message = $"SendShoutMessage {data.SourceData.FirstName} {data.SourceData.LastName}: {data.Message}"
                };
            }
        
            private RpcCommandResult HandleGroupMessage(DdonGameServer gameServer)
            {
                RpcChatData data = _entry.GetData<RpcChatData>();

                ChatResponse response = new()
                {
                    HandleId = 0,
                    Deliver = false,
                    FirstName = data.SourceData.FirstName,
                    LastName = data.SourceData.LastName,
                    ClanName = data.SourceData.ClanName,
                    CharacterId = data.SourceData.CharacterId,
                    Type = LobbyChatMsgType.Group,
                    Message = data.Message,
                    MessageFlavor = data.MessageFlavor,
                    PhrasesCategory = data.PhrasesCategory,
                    PhrasesIndex = data.PhrasesIndex
                };

                // The target group is encoded in the HandleId field.
                response.Recipients.AddRange(gameServer.ClientLookup
                    .GetAll()
                    .Where(x => x.Character != null && x.Character.GroupChatId == data.HandleId)
                );
                gameServer.ChatManager.Send(response);

                return new RpcCommandResult(this, true)
                {
                    Message = $"SendGroupMessage {data.SourceData.FirstName} {data.SourceData.LastName} ({data.HandleId}): {data.Message}"
                };
            }
        
            private RpcCommandResult HandleJoinGroupChat(DdonGameServer gameServer)
            {
                RpcPacketData data = _entry.GetData<RpcPacketData>();
                S2CGroupChatInviteCharacterNtc inviteNtc = EntitySerializer.Get<S2CGroupChatInviteCharacterNtc>().Read(data.Data);

                foreach (var client in gameServer.ClientLookup.GetAll())
                {
                    if (client?.Character.GroupChatId == inviteNtc.GroupId)
                    {
                        client.Send(inviteNtc);
                    }
                }

                return new RpcCommandResult(this, true)
                {
                    Message = $"JoinGroupChat {inviteNtc.VisitorInfo.CommunityCharacterBaseInfo.CharacterName} -> {data.GroupId}"
                };
            }

            private RpcCommandResult HandleLeaveGroupChat(DdonGameServer gameServer)
            {
                RpcPacketData data = _entry.GetData<RpcPacketData>();
                S2CGroupChatLeaveCharacterNtc leaveNtc = EntitySerializer.Get<S2CGroupChatLeaveCharacterNtc>().Read(data.Data);

                foreach (var client in gameServer.ClientLookup.GetAll())
                {
                    if (client?.Character.GroupChatId == leaveNtc.GroupId)
                    {
                        client.Send(leaveNtc);
                    }
                }

                return new RpcCommandResult(this, true)
                {
                    Message = $"LeaveGroupChat {leaveNtc.LeaveCharacterId} -x-> {data.GroupId}"
                };
            }
        }

        public override string Route => "/rpc/internal/chat";

        public async override Task<WebResponse> Post(WebRequest request)
        {
            return await HandleBody<RpcUnwrappedObject, InternalChatCommand>(request);
        }
    }
}

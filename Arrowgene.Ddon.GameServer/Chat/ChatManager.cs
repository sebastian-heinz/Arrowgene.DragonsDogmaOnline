using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Chat
{
    public class ChatManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ChatManager));

        private readonly List<IChatHandler> _handler;
        private readonly DdonGameServer _Server;

        public ChatManager(DdonGameServer server)
        {
            _Server = server;
            _handler = new List<IChatHandler>();
        }

        public void AddHandler(IChatHandler handler)
        {
            _handler.Add(handler);
        }

        public void SendMessage(string message, string firstName, string lastName, LobbyChatMsgType type,
            params uint[] characterIds)
        {
            ChatResponse response = new ChatResponse();
            response.Deliver = true;
            response.Message = message;
            response.FirstName = firstName;
            response.LastName = lastName;
            response.CharacterId = 0;
            response.Type = type;
            response.MessageFlavor = 0;
            response.PhrasesCategory = 0;
            response.PhrasesIndex = 0;
            foreach (uint characterId in characterIds)
            {
                GameClient client = _Server.ClientLookup.GetClientByCharacterId(characterId);
                if (client == null)
                {
                    continue;
                }

                response.Recipients.Add(client);
            }

            Send(response);
        }

        public void SendMessage(string message, string firstName, string lastName, LobbyChatMsgType type,
            List<GameClient> recipients)
        {
            ChatResponse response = new ChatResponse
            {
                HandleId = 0,
                Deliver = true,
                Message = message,
                FirstName = firstName,
                LastName = lastName,
                CharacterId = 0,
                Type = type,
                MessageFlavor = 0,
                PhrasesCategory = 0,
                PhrasesIndex = 0
            };
            response.Recipients.AddRange(recipients);
            Send(response);
        }

        public void BroadcastMessageToParty(PartyGroup party, LobbyChatMsgType type, string message)
        {
            SendMessage(message, string.Empty, string.Empty, type, party.Clients);
        }

        public void BroadcastMessage(LobbyChatMsgType type, string message)
        {
            SendMessage(message, string.Empty, string.Empty, type, _Server.ClientLookup.GetAll());
        }
        
        public void SendTellMessage(GameClient sender, GameClient receiver, C2SChatSendTellMsgReq request)
        {
            var senderCharacterInfo = sender.Character.CDataCommunityCharacterBaseInfo;
            var receiverCharacterInfo = receiver.Character.CDataCommunityCharacterBaseInfo;
            ChatResponse senderChatResponse = GetTellChatResponse(senderCharacterInfo.CharacterId, receiverCharacterInfo, request);
            senderChatResponse.Recipients.Add(sender);
            ChatResponse receiverChatResponse = GetTellChatResponse(senderCharacterInfo.CharacterId, senderCharacterInfo, request);
            receiverChatResponse.Recipients.Add(receiver);

            Send(senderChatResponse);

            if (sender.Account.State != AccountStateType.Muted)
            {
                Send(receiverChatResponse);
            }
        }

        public void SendTellMessageForeign(GameClient client, C2SChatSendTellMsgReq request)
        {
            if (client.Account.State != AccountStateType.Muted)
            {
                _Server.RpcManager.AnnounceTellChat(client, request);
            }

            ChatResponse senderChatResponse = new ChatResponse
            {
                HandleId = request.CharacterInfo.CharacterId,
                Deliver = false,
                FirstName = request.CharacterInfo.CharacterName.FirstName,
                LastName = request.CharacterInfo.CharacterName.LastName,
                ClanName = request.CharacterInfo.ClanName,
                CharacterId = request.CharacterInfo.CharacterId,
                Type = LobbyChatMsgType.Tell,
                Message = request.Message,
                MessageFlavor = request.MessageFlavor,
                PhrasesCategory = request.PhrasesCategory,
                PhrasesIndex = request.PhrasesIndex
            };

            senderChatResponse.Recipients.Add(client);
            Send(senderChatResponse);
        }

        public void Handle(GameClient client, ChatMessage message)
        {
            if (client == null)
            {
                Logger.Debug("Client is Null");
                return;
            }

            if (message == null)
            {
                Logger.Debug(client, "Chat Message is Null");
                return;
            }

            List<ChatResponse> responses = new List<ChatResponse>();
            foreach (IChatHandler handler in _handler)
            {
                handler.Handle(client, message, responses);
            }

            if (message.Deliver)
            {
                // deliver original chat message
                ChatResponse response = ChatResponse.FromMessage(client, message);
                Deliver(client, response);
            }

            foreach (ChatResponse response in responses)
            {
                // deliver additional messages generated form handler
                if (!response.Deliver)
                {
                    continue;
                }

                Deliver(client, response);
            }
        }

        private void Deliver(GameClient client, ChatResponse response)
        {
            if (client.Account.State == AccountStateType.Muted)
            {
                response.Recipients.Add(client);
                Send(response);
                return;
            }

            switch (response.Type)
            {
                case LobbyChatMsgType.Say:
                    // Quick-chats are local/party-shared.
                    if (response.MessageFlavor > 0)
                    {
                        HashSet<GameClient> recipients;
                        if (StageManager.IsHubArea(client.Character.Stage))
                        {
                            recipients = [.. (client.Party?.Clients ?? [])
                                .Union(
                                    _Server.ClientLookup.GetAll()
                                    .Where(x => x.Character?.StageNo == client.Character?.StageNo)
                                )];
                        }
                        else
                        {
                            recipients = [.. client.Party?.Clients ?? []];
                        }
                            
                        response.Recipients.AddRange(recipients);
                        break;
                    }
                    else
                    {
                        response.Recipients.AddRange(_Server.ClientLookup
                            .GetAll()
                        );
                        break;
                    }
                case LobbyChatMsgType.Shout:
                    response.Recipients.AddRange(_Server.ClientLookup
                        .GetAll()
                    );

                    if (_Server.GameSettings.ChatCommandsSettings.CrossChannelShout)
                    {
                        _Server.RpcManager.AnnounceShoutChat(client, response);
                    }

                    break;
                case LobbyChatMsgType.Party:
                    PartyGroup party = client.Party;
                    if (party != null)
                    {
                        response.Recipients.AddRange(party.Clients);
                    }
                    break;
                case LobbyChatMsgType.Clan:
                    if (client.Character.ClanId == 0)
                    {
                        response.Recipients.Add(client);
                        break;
                    }

                    response.Recipients.AddRange(_Server.ClientLookup.GetAll().Where(
                        x => x.Character != null
                        && client.Character != null
                        && x.Character.ClanId == client.Character.ClanId)
                    );

                    _Server.RpcManager.AnnounceClanChat(client, response);
                    break;
                case LobbyChatMsgType.Group:
                    if (client.Character.GroupChatId == 0)
                    {
                        response.Recipients.Add(client);
                        break;
                    }

                    response.Recipients.AddRange(_Server.ClientLookup
                        .GetAll()
                        .Where(x => x?.Character.GroupChatId == client?.Character.GroupChatId)
                    );

                    _Server.RpcManager.AnnounceGroupChat(client, response);

                    break;
                default:
                    response.Recipients.Add(client);
                    break;
            }

            Send(response);
        }
        
        public static S2CLobbyChatMsgNotice GetTellMsgNtc(uint handleId, CDataCommunityCharacterBaseInfo characterInfo, C2SChatSendTellMsgReq request)
        {
            return new S2CLobbyChatMsgNotice
            {
                Type = LobbyChatMsgType.Tell,
                HandleId = handleId,
                CharacterBaseInfo = characterInfo,
                MessageFlavor = request.MessageFlavor,
                PhrasesCategory = request.PhrasesCategory,
                PhrasesIndex = request.PhrasesIndex,
                Message = request.Message
            };
        }
        
        public static ChatResponse GetTellChatResponse(uint handleId, CDataCommunityCharacterBaseInfo characterInfo, C2SChatSendTellMsgReq request)
        {
            return new ChatResponse
            {
                HandleId = handleId,
                Deliver = false,
                FirstName = characterInfo.CharacterName.FirstName,
                LastName = characterInfo.CharacterName.LastName,
                ClanName = characterInfo.ClanName,
                CharacterId = characterInfo.CharacterId,
                Type = LobbyChatMsgType.Tell,
                Message = request.Message,
                MessageFlavor = request.MessageFlavor,
                PhrasesCategory = request.PhrasesCategory,
                PhrasesIndex = request.PhrasesIndex
            };
        }

        public void Send(ChatResponse response)
        {
            S2CLobbyChatMsgNotice notice = new S2CLobbyChatMsgNotice
            {
                HandleId = response.HandleId,
                Type = response.Type,
                MessageFlavor = response.MessageFlavor,
                PhrasesCategory = response.PhrasesCategory,
                PhrasesIndex = response.PhrasesIndex,
                Message = response.Message,
                CharacterBaseInfo =
                {
                    CharacterId = response.CharacterId,
                    CharacterName =
                    {
                        FirstName = response.FirstName,
                        LastName = response.LastName
                    },
                    ClanName = response.ClanName
                }
            };
            foreach (GameClient client in response.Recipients.Distinct())
            {
                client.Send(notice);
            }
        }

        public void SendSystemMail(uint recieverId, string senderName, string title, string body, IEnumerable<(ItemId ItemId, uint Count)> itemIds, DbConnection? connectionIn = null)
        {
            MailMessage mail = new()
            {
                SenderName = senderName,
                CharacterId = recieverId,
                Title = title,
                Body = body,
                MessageState = MailState.Unopened,
                SendDate = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            foreach (var (itemId, count) in itemIds)
            {
                mail.Attachments.Add(new SystemMailAttachment()
                {
                    AttachmentType = SystemMailAttachmentType.Item,
                    AttachmentId = (ulong)(mail.Attachments.Count + 1),
                    Param1 = (uint)itemId,
                    Param2 = count,
                    IsReceived = false,
                });
            }

            _Server.Database.ExecuteQuerySafe(connectionIn, connection =>
            {
                uint messageId = (uint)_Server.Database.InsertSystemMailMessage(connection, mail);

                foreach (var attachment in mail.Attachments)
                {
                    attachment.MessageId = messageId;
                    _Server.Database.InsertSystemMailAttachment(connection, attachment);
                }
            });

            byte itemState = 0;
            if (mail.Attachments.Count > 0)
            {
                // TODO: Should item state be based on attachment type?
                itemState = (byte)(mail.Attachments.All(x => x.IsReceived) ? MailItemState.Exist : MailItemState.Exist | MailItemState.Item);
            }

            var channel = _Server.RpcManager.FindPlayerById(recieverId);
            var notice = new S2CMailSystemMailSendNtc()
            {
                MailInfo = mail.ToCDataMailInfo(itemState)
            };

            if (channel == _Server.Id)
            {
                // Player is online on this channel.
                var client = _Server.ClientLookup.GetClientByCharacterId(recieverId);
                client.Send(notice);
            }
            else if (channel != 0)
            {
                // Player is online on another server.
                _Server.RpcManager.AnnounceCharacterPacket(notice, recieverId);
            }
            else
            {
                // Player is offline.
                // Do nothing.
            }
        }
    }
}

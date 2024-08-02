using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class GiveItemCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;

        private static readonly uint DefaultAmount = 1;
        private static readonly bool DefaultToStorage = false;

        public override string Key => "giveitem";
        public override string HelpText => "usage: `/giveitem [itemid] [amount?]` - Obtain items.";

        private DdonGameServer _server;

        public GiveItemCommand(DdonGameServer server)
        {
            _server = server;
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            uint itemId = 0;
            if (command.Length == 0)
            {
                responses.Add(ChatResponse.CommandError(client, "No arguments provided."));
                return;
            }

            if (command.Length >= 1)
            {
                if (UInt32.TryParse(command[0], out uint parsedId))
                {
                    itemId = parsedId;
                }
                else
                {
                    responses.Add(ChatResponse.CommandError(client, $"Invalid itemId \"{command[0]}\". It must be a number"));
                    return;
                }
            }

            uint amount = DefaultAmount;
            if (command.Length >= 2)
            {
                if (UInt32.TryParse(command[1], out uint parsedAmount))
                {
                    amount = parsedAmount;
                }
                else
                {
                    responses.Add(ChatResponse.CommandError(client, $"Invalid amount \"{command[1]}\". It must be a number"));
                    return;
                }
            }

            if (!_server.AssetRepository.ClientItemInfos.ContainsKey(itemId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid itemId \"{command[0]}\". This item does not exist."));
                return;
            }

            ClientItemInfo itemInfo = ClientItemInfo.GetInfoForItemId(_server.AssetRepository.ClientItemInfos, itemId);

            SystemMailMessage mail = new SystemMailMessage()
            {
                Title = $"GiveItem: {itemInfo.Name} x{amount}",
                Body = $"",
                CharacterId = client.Character.CharacterId,
                SenderName = "/giveitem",
                MessageState = MailState.Unopened
            };
            mail.Attachments.Add(new SystemMailAttachment()
            {
                AttachmentType = SystemMailAttachmentType.Item,
                Param1 = itemId,
                Param2 = amount,
                MessageId = (ulong)(mail.Attachments.Count + 1),
                IsReceived = false,
            });
            SystemMailService.DeliverSystemMailMessage(_server.Database, mail);

            S2CMailSystemMailSendNtc notice = new S2CMailSystemMailSendNtc()
            {
                MailInfo = mail.ToCDataMailInfo((byte)(MailItemState.Exist | MailItemState.Item))
            };
            client.Send(notice);
        }
    }
}

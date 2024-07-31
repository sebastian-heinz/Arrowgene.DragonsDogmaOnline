using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class GiveItemCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;

        private static readonly uint DefaultAmount = 1;
        private static readonly bool DefaultToBag = true;

        public override string Key => "giveitem";
        public override string HelpText => "usage: `/giveitem [itemid] [amount?] [toStorage?]` - Obtain items.";

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

            bool toBag = DefaultToBag;
            if (command.Length >= 3)
            {
                if (bool.TryParse(command[2], out bool parsedToBag))
                {
                    toBag = parsedToBag;
                }
                else
                {
                    responses.Add(ChatResponse.CommandError(client, $"Invalid to storage flag \"{command[2]}\". It must be a boolean."));
                    return;
                }
            }

            S2CItemUpdateCharacterItemNtc itemUpdateNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.Default
            };

            itemUpdateNtc.UpdateItemList = _server.ItemManager.AddItem(_server, client.Character, toBag, itemId, amount);

            client.Send(itemUpdateNtc);

            ClientItemInfo itemInfo = ClientItemInfo.GetInfoForItemId(_server.AssetRepository.ClientItemInfos, itemId);

            responses.Add(ChatResponse.ServerMessage(client, $"Granting {itemInfo.Name} <{itemId}> x{amount}."));

        }
    }
}

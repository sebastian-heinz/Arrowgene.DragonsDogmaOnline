using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
                if (uint.TryParse(command[0], out uint parsedId))
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
                if (uint.TryParse(command[1], out uint parsedAmount))
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

            client.Send(new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.StampBonus,
                UpdateItemList = _server.ItemManager.AddItem(_server, client.Character, StorageType.ItemPost, itemId, amount),
            });
        }
    }
}

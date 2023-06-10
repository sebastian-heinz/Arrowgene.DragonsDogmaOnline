using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class MotherlodeCommand : ChatCommand
    {
        private static readonly Dictionary<string, WalletType> WalletTypeNames = new Dictionary<string, WalletType>()
        {
            {"G", WalletType.Gold},
            {"RP", WalletType.RiftPoints},
            {"BO", WalletType.BloodOrbs},
            {"HO", WalletType.HighOrbs}
        };

        private static readonly uint DefaultAmount = 10000;
        private static readonly WalletType[] DefaultWalletTypes = new WalletType[] {
            WalletType.Gold,
            WalletType.RiftPoints
        };

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "motherlode";
        public override string HelpText => "usage: `/motherlode [amount?] [walletType?...]` - Obtain wallet points";

        private DdonGameServer _server;

        public MotherlodeCommand(DdonGameServer server)
        {
            _server = server;
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            uint amount = DefaultAmount;
            if (command.Length >= 1)
            { 
                if(UInt32.TryParse(command[0], out uint parsedAmount))
                {
                    amount = parsedAmount;
                }
                else
                {
                    responses.Add(ChatResponse.CommandError(client, $"Invalid amount \"{command[0]}\". It must be a number"));
                    return;
                }
            }

            WalletType[] walletTypes = DefaultWalletTypes;
            if (command.Length >= 2)
            {
                walletTypes = new WalletType[command.Length-1];
                for (int i = 1; i < command.Length; i++)
                {
                    if (WalletTypeNames.TryGetValue(command[i], out WalletType parsedWalletType))
                    {
                        walletTypes[i-1] = parsedWalletType;
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client, $"Invalid wallet type \"{command[i]}\". It must be one of the following: {string.Join(", ", WalletTypeNames.Keys)}"));
                        return;
                    }
                }
            }

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            foreach (var walletType in walletTypes)
            {
                CDataWalletPoint walletPoint = client.Character.WalletPointList.Where(wp => wp.Type == walletType).Single();
                walletPoint.Value += amount;
                _server.Database.UpdateWalletPoint(client.Character.CharacterId, walletPoint);
                
                CDataUpdateWalletPoint updateWalletPoint = new CDataUpdateWalletPoint();
                updateWalletPoint.Type = walletPoint.Type;
                updateWalletPoint.AddPoint = (int) amount;
                updateWalletPoint.Value = walletPoint.Value;
                updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);
            }
            client.Send(updateCharacterItemNtc);
        }
    }
}
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
            {"HO", WalletType.HighOrbs},
            {"GG", WalletType.GoldenGemstones},
            {"RC", WalletType.ResetCraftSkills}
        };

        private static readonly uint DefaultAmount = 10000;
        private static readonly HashSet<WalletType> DefaultWalletTypes = new HashSet<WalletType> {
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

            HashSet<WalletType> walletTypes = DefaultWalletTypes;
            if (command.Length >= 2)
            {
                walletTypes = new HashSet<WalletType>();
                foreach (string arg in command.Skip(1))
                {
                    if (WalletTypeNames.TryGetValue(arg, out WalletType parsedWalletType))
                    {
                        walletTypes.Add(parsedWalletType);
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client, $"Invalid wallet type \"{arg}\". It must be one of the following: {string.Join(", ", WalletTypeNames.Keys)}"));
                        return;
                    }
                }
            }

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            foreach (var walletType in walletTypes)
            {
                CDataWalletPoint walletPoint = client.Character.WalletPointList.Single(wp => wp.Type == walletType);
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

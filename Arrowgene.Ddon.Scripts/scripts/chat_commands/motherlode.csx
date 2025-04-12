public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "motherlode";
    public override string HelpText               => "usage: `/motherlode [amount?] [walletType?...]` - Obtain wallet points";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
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
            CDataUpdateWalletPoint updateWalletPoint = server.WalletManager.AddToWallet(client.Character, walletType, amount);
            updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);
        }
        client.Send(updateCharacterItemNtc);
    }

    private static readonly Dictionary<string, WalletType> WalletTypeNames = new Dictionary<string, WalletType>()
    {
        {"G", WalletType.Gold},
        {"RP", WalletType.RiftPoints},
        {"BO", WalletType.BloodOrbs},
        {"HO", WalletType.HighOrbs},
        {"GG", WalletType.GoldenGemstones},
        {"RC", WalletType.ResetCraftSkills},
        {"ST", WalletType.SilverTickets},
    };

    private static readonly uint DefaultAmount = 10000;
    private static readonly HashSet<WalletType> DefaultWalletTypes = new HashSet<WalletType> {
        WalletType.Gold,
        WalletType.RiftPoints,
        WalletType.BloodOrbs
    };
}

return new ChatCommand();

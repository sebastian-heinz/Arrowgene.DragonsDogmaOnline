using Arrowgene.Ddon.GameServer.Handler;
using System.Collections.Generic;
using System.Globalization;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.User;
    public override string CommandName => "wallet";
    public override string HelpText => "usage: `/wallet` - Check hidden wallet amounts.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        S2CConnectionInformationNtc ntc = new(client.Character.WalletPointList.Select(x => $"{x.Type} : {x.Value.ToString("N0", CultureInfo.InvariantCulture)} / {server.GameSettings.GameServerSettings.WalletLimits.GetValueOrDefault(x.Type).ToString("N0", CultureInfo.InvariantCulture)}"));
        client.Send(ntc);
    }
}

return new ChatCommand();

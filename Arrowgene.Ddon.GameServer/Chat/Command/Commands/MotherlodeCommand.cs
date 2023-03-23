using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class MotherlodeCommand : ChatCommand
    {
        private static readonly uint DefaultAmount = 10000;

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "motherlode";
        public override string HelpText => "usage: `/motherlode [amount]` - Obtain Gold and Rift Points";

        private DdonGameServer _server;

        public MotherlodeCommand(DdonGameServer server)
        {
            _server = server;
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            uint amount = DefaultAmount;
            if(command.Length > 0 && UInt32.TryParse(command[0], out uint parsedAmount))
            {
                amount = parsedAmount;
            }

            CDataWalletPoint goldWalletPoint = client.Character.WalletPointList.Where(wp => wp.Type == WalletType.Gold).Single();
            goldWalletPoint.Value += amount;
            _server.Database.UpdateWalletPoint(client.Character.Id, goldWalletPoint);
            
            CDataWalletPoint rpWalletPoint = client.Character.WalletPointList.Where(wp => wp.Type == WalletType.RiftPoints).Single();
            rpWalletPoint.Value += amount;
            _server.Database.UpdateWalletPoint(client.Character.Id, rpWalletPoint);

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            CDataUpdateWalletPoint updateWalletGold = new CDataUpdateWalletPoint();
            updateWalletGold.Type = goldWalletPoint.Type;
            updateWalletGold.AddPoint = (int) amount;
            updateWalletGold.Value = goldWalletPoint.Value;
            updateCharacterItemNtc.UpdateWalletList.Add(updateWalletGold);
            CDataUpdateWalletPoint updateWalletRP = new CDataUpdateWalletPoint();
            updateWalletRP.Type = rpWalletPoint.Type;
            updateWalletRP.AddPoint = (int) amount;
            updateWalletRP.Value = rpWalletPoint.Value;
            updateCharacterItemNtc.UpdateWalletList.Add(updateWalletRP);
            client.Send(updateCharacterItemNtc);
        }
    }
}
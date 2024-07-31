using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class GivePawnCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;

        public override string Key => "givepawn";
        public override string HelpText => "usage: `/givepawn` - Give yourself a pawn.";

        private DdonGameServer _server;
        private PawnCreatePawnHandler _handler;
        private GiveItemCommand _giveItem;

        public GivePawnCommand(DdonGameServer server)
        {
            _server = server;
            _handler = new PawnCreatePawnHandler(server);
            _giveItem = new GiveItemCommand(server);
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            int pawnCount = client.Character.Pawns.Count + 1;

            if (pawnCount > 1)
            {
                //Give riftstone ore in preparation for the PawnCreatePawnHandler.
                _giveItem.Execute(new[] { $"{10133}", $"{10}" }, client, message, responses);
            }

            string pawnName = "Pawn" + (char)((int)'A' + pawnCount - 1);

            _handler.Handle(client, new C2SPawnCreatePawnReq()
            {
                SlotNo = (byte)pawnCount,
                PawnInfo = new CDataPawnInfo()
                {
                    Name = pawnName,
                    EditInfo = client.Character.EditInfo,
                    HideEquipHead = client.Character.HideEquipHead,
                    HideEquipLantern = client.Character.HideEquipLantern,
                    JobId = client.Character.Job
                }
            });

            responses.Add(ChatResponse.ServerMessage(client, $"Giving a basic pawn."));
        }
    }
}

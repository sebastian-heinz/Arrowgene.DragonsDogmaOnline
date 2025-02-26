using Arrowgene.Ddon.GameServer.Handler;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "givepawn";
    public override string HelpText               => "usage: `/givepawn` - Give yourself a pawn.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        var handler = new PawnCreatePawnHandler(server);

        int pawnCount = client.Character.Pawns.Count + 1;
        if (pawnCount > 1)
        {
            //Give riftstone ore in preparation for the PawnCreatePawnHandler.
            S2CItemUpdateCharacterItemNtc itemUpdateNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.Default
            };
            itemUpdateNtc.UpdateItemList = server.ItemManager.AddItem(server, client.Character, false, 10133, 10);

            client.Send(itemUpdateNtc);
        }

        string pawnName = "Pawn" + (char)((int)'A' + pawnCount - 1);

        handler.Handle(client, new C2SPawnCreatePawnReq()
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

return new ChatCommand();
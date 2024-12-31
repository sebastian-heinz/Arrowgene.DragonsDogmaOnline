public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "givepowerfulitems";
    public override string HelpText               => "usage: `/givepowerfulitems` - Get a set of free stuff.";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        SystemMailMessage mail = new SystemMailMessage()
        {
            Title = $"GivePowerfulItems",
            Body = $"",
            CharacterId = client.Character.CharacterId,
            SenderName = "/givepowerfulitems",
            MessageState = MailState.Unopened
        };

        foreach (var item in Items)
        {
            ClientItemInfo itemInfo = ClientItemInfo.GetInfoForItemId(server.AssetRepository.ClientItemInfos, item);
            mail.Attachments.Add(new SystemMailAttachment()
            {
                AttachmentType = SystemMailAttachmentType.Item,
                Param1 = item,
                Param2 = 1,
                MessageId = (ulong)(mail.Attachments.Count + 1),
                IsReceived = false,
            });
        }

        SystemMailService.DeliverSystemMailMessage(server.Database, mail);

        S2CMailSystemMailSendNtc notice = new S2CMailSystemMailSendNtc()
        {
            MailInfo = mail.ToCDataMailInfo((byte)(MailItemState.Exist | MailItemState.Item))
        };
        client.Send(notice);
    }

    private static List<uint> Items = new List<uint>()
    {
        25604,
        25606,
        25607,
        25609,
        25610,
        25611,
        25612,
        25613,
        25614,
        25615,
        25616,
        25605,
        25608,
        25621,
        25622
    };
}

return new ChatCommand();
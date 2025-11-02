using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model
{
    public class MailMessage
    {
        public ulong MessageId { get; set; }
        public uint CharacterId { get; set; }
        public MailState MessageState { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public ulong SendDate { get; set; }
        public List<SystemMailAttachment> Attachments { get; set; } = [];
        public CDataCommunityCharacterBaseInfo BaseInfo { get; set; } = new();

        public CDataMailInfo ToCDataMailInfo(byte itemState)
        {
            return new CDataMailInfo()
            {
                Id = MessageId,
                State = MessageState,
                SenderName = SenderName,
                MailText = Title,
                SenderDate = SendDate,
                ItemState = itemState,
                BaseInfo = BaseInfo
            };
        }
    }
}

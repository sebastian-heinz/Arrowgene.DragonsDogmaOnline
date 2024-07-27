using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model
{
    public class SystemMailMessage
    {
        public SystemMailMessage()
        {
            SenderName = String.Empty;
            Body = String.Empty;
            Attachments = new List<SystemMailAttachment>();
        }

        public ulong MessageId { get; set; }
        public uint CharacterId { get; set; }
        public MailState MessageState;
        public string SenderName { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public ulong SendDate { get; set; }
        public List<SystemMailAttachment> Attachments { get; set; }

        public CDataMailInfo ToCDataMailInfo(byte itemState)
        {
            return new CDataMailInfo()
            {
                Id = MessageId,
                State = MessageState,
                SenderName = SenderName,
                MailText = Title,
                SenderDate = SendDate,
                ItemState = itemState
            };
        }
    }
}

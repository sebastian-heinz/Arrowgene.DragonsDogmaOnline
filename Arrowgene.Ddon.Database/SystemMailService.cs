using Arrowgene.Ddon.Shared.Model;
using System;
using System.Data.Common;

namespace Arrowgene.Ddon.Database
{
    public class SystemMailService
    {
        public static bool DeliverSystemMailMessage(IDatabase db, DbConnection conn, SystemMailMessage message)
        {
            message.SendDate = (ulong) DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            uint messageId = (uint) db.InsertSystemMailMessage(conn, message);

            foreach (var attachment in message.Attachments)
            {
                attachment.MessageId = messageId;
                db.InsertSystemMailAttachment(conn, attachment);
            }

            return true;
        }

        public static bool DeliverSystemMailMessage(IDatabase db, SystemMailMessage message)
        {
            message.SendDate = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            uint messageId = (uint)db.InsertSystemMailMessage(message);

            foreach (var attachment in message.Attachments)
            {
                attachment.MessageId = messageId;
                db.InsertSystemMailAttachment(attachment);
            }

            return true;
        }
    }
}

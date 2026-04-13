using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MailSystemMailGetTextHandler : GameRequestPacketHandler<C2SMailSystemMailGetTextReq, S2CMailSystemMailGetTextRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MailSystemMailGetTextHandler));

        public MailSystemMailGetTextHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMailSystemMailGetTextRes Handle(GameClient client, C2SMailSystemMailGetTextReq request)
        {
            var message = Server.Database.SelectSystemMailMessage(request.MailId);
            var attachments = Server.Database.SelectAttachmentsForSystemMail(request.MailId);

            Server.Database.UpdateSystemMailMessageState(request.MailId, MailState.Opened);

            var result = new S2CMailSystemMailGetTextRes()
            {
                MailId = request.MailId,
                MailTextInfo = new CDataMailTextInfo()
                {
                    Text = message.Body
                }
            };

            foreach (var attachment in attachments)
            {
                switch (attachment.AttachmentType)
                {
                    case SystemMailAttachmentType.Item:
                        result.MailTextInfo.MailAttachmentList.ItemList.Add(attachment.ToCDataMailItemInfo());
                        break;
                    case SystemMailAttachmentType.GP:
                        result.MailTextInfo.MailAttachmentList.GPList.Add(attachment.ToCDataMailGPInfo());
                        break;
                    case SystemMailAttachmentType.Course:
                        result.MailTextInfo.MailAttachmentList.OptionCourseList.Add(attachment.ToCDataMailOptionCourseInfo());
                        break;
                    case SystemMailAttachmentType.PawnLegend:
                        result.MailTextInfo.MailAttachmentList.LegendPawnList.Add(attachment.ToCDataMailLegendPawnInfo());
                        break;
                }
            }

            return result;
        }
    }
}


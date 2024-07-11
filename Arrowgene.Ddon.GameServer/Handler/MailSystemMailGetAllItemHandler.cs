using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Linq;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MailSystemMailGetAllItemHandler : GameRequestPacketHandler<C2SMailSystemMailGetAllItemReq, S2CMailSystemGetAllItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MailSystemMailGetAllItemHandler));

        public MailSystemMailGetAllItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMailSystemGetAllItemRes Handle(GameClient client, C2SMailSystemMailGetAllItemReq request)
        {
            var pcap = new S2CMailSystemGetAllItemRes.Serializer().Read(pcap_data);

            var result = new S2CMailSystemGetAllItemRes()
            {
                MessageId = request.MessageId,
            };
            
            bool toItemBag = false;
            switch (request.StorageType)
            {
                case 13: // ItemPost   StorageType = 13
                    toItemBag = false;
                    break;
                case 19: // ItemBag    StorageType = 19
                    toItemBag = true;
                    break;
                case 20: // StorageBox StorageType = 20
                    toItemBag = false;
                    break;
            }

            S2CItemUpdateCharacterItemNtc itemUpdateNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = 18 // value from pcap
            };

            var attachments = Server.Database.SelectAttachmentsForSystemMail(request.MessageId);
            foreach (var attachment in attachments)
            {
                attachment.IsReceived = true;
                switch (attachment.AttachmentType)
                {
                    case SystemMailAttachmentType.Item:
                        result.AttachmentList.ItemList.Add(attachment.ToCDataMailItemInfo());
                        itemUpdateNtc.UpdateItemList.AddRange(Server.ItemManager.AddItem(Server, client.Character, toItemBag, attachment.Param1, attachment.Param2));
                        break;
                    case SystemMailAttachmentType.GP:
                        result.AttachmentList.GPList.Add(attachment.ToCDataMailGPInfo());
                        break;
                    case SystemMailAttachmentType.Course:
                        result.AttachmentList.OptionCourseList.Add(attachment.ToCDataMailOptionCourseInfo());
                        break;
                    case SystemMailAttachmentType.PawnLegend:
                        result.AttachmentList.LegendPawnList.Add(attachment.ToCDataMailLegendPawnInfo());
                        break;
                }
                Server.Database.UpdateSystemMailAttachmentReceivedStatus(attachment.MessageId, attachment.AttachmentId, true);
            }

            if (itemUpdateNtc.UpdateItemList.Count > 0)
            {
                client.Send(itemUpdateNtc);
            }

            return result;
        }

        private byte[] pcap_data = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1A, 0xA8, 0x12, 0x8F, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0B, 0xC0, 0x01, 0x00, 0x00, 0x1F, 0x74, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
    }
}


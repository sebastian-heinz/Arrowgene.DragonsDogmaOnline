using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Linq;

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

            // ItemPost   StorageType = 13
            // ItemBag    StorageType = 19
            // StorageBox StorageType = 20

            // Send S2C_ITEM_UPDATE_CHARACTER_ITEM_NTC to deliver items to player
            // {"Structure": {"UpdateType": 18, "UpdateItemList": [{"ItemList": {"ItemUId": "62CFAB88", "ItemId": 8052, "ItemNum": 4, "Unk3": 0, "StorageType": "Unk13", "SlotNo": 190, "Color": 0, "PlusValue": 0, "Bind": false, "EquipPoint": 0, "EquipCharacterID": 0, "EquipPawnID": 0, "WeaponCrestDataList": [], "ArmorCrestDataList": [], "EquipElementParamList": []}, "UpdateItemNum": 4}], "UpdateWalletList": [{"Type": "GoldenGemstones", "Value": 0, "AddPoint": 0, "ExtraBonusPoint": 0}]}}

            var attachments = Server.Database.SelectAttachmentsForSystemMail(request.MessageId);
            foreach (var attachment in attachments)
            {
                attachment.IsReceived = true;
                switch (attachment.AttachmentType)
                {
                    case SystemMailAttachmentType.Item:
                        result.AttachmentList.ItemList.Add(attachment.ToCDataMailItemInfo());
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

            return result;
        }

        private byte[] pcap_data = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1A, 0xA8, 0x12, 0x8F, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0B, 0xC0, 0x01, 0x00, 0x00, 0x1F, 0x74, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
    }
}


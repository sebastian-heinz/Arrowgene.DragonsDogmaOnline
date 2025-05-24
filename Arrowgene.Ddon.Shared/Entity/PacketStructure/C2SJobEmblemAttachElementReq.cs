using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobEmblemAttachElementReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_JOB_EMBLEM_ATTACH_ELEMENT_REQ;

        public C2SJobEmblemAttachElementReq()
        {
            EmblemUIDs = new();
            JewelryUIDs = new();
            AttachChanceItems = new();
            PremiumCurrencyCost = new();
        }

        public JobId JobId { get; set; }
        public byte InheritanceSlot { get; set; }
        public List<string> EmblemUIDs { get; set; }
        public List<string> JewelryUIDs { get; set; }
        public List<CDataItemAmount> AttachChanceItems { get; set; }
        public List<CDataJobEmblemActionCostParam> PremiumCurrencyCost { get; set; }

        public class Serializer : PacketEntitySerializer<C2SJobEmblemAttachElementReq>
        {
            public override void Write(IBuffer buffer, C2SJobEmblemAttachElementReq obj)
            {
                WriteByte(buffer, (byte) obj.JobId);
                WriteByte(buffer, obj.InheritanceSlot);
                WriteMtStringList(buffer, obj.EmblemUIDs);
                WriteMtStringList(buffer, obj.JewelryUIDs);
                WriteEntityList(buffer, obj.AttachChanceItems);
                WriteEntityList(buffer, obj.PremiumCurrencyCost);
            }

            public override C2SJobEmblemAttachElementReq Read(IBuffer buffer)
            {
                C2SJobEmblemAttachElementReq obj = new C2SJobEmblemAttachElementReq();
                obj.JobId = (JobId) ReadByte(buffer);
                obj.InheritanceSlot = ReadByte(buffer);
                obj.EmblemUIDs = ReadMtStringList(buffer);
                obj.JewelryUIDs = ReadMtStringList(buffer);
                obj.AttachChanceItems = ReadEntityList<CDataItemAmount>(buffer);
                obj.PremiumCurrencyCost = ReadEntityList<CDataJobEmblemActionCostParam>(buffer);
                return obj;
            }
        }
    }
}

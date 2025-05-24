using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobEmblemAttachElementRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_JOB_EMBLEM_ATTACH_ELEMENT_RES;

        public S2CJobEmblemAttachElementRes()
        {
            InheritanceResult = new();
            UpdateWalletPointList = new();
            ItemUpdateResultList = new();
        }

        public CDataJobEmblemInheritanceResult InheritanceResult { get; set; }
        public List<CDataUpdateWalletPoint> UpdateWalletPointList { get; set; }
        public List<CDataItemUpdateResult> ItemUpdateResultList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobEmblemAttachElementRes>
        {
            public override void Write(IBuffer buffer, S2CJobEmblemAttachElementRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.InheritanceResult);
                WriteEntityList(buffer, obj.UpdateWalletPointList);
                WriteEntityList(buffer, obj.ItemUpdateResultList);
            }

            public override S2CJobEmblemAttachElementRes Read(IBuffer buffer)
            {
                S2CJobEmblemAttachElementRes obj = new S2CJobEmblemAttachElementRes();
                ReadServerResponse(buffer, obj);
                obj.InheritanceResult = ReadEntity<CDataJobEmblemInheritanceResult>(buffer);
                obj.UpdateWalletPointList = ReadEntityList<CDataUpdateWalletPoint>(buffer);
                obj.ItemUpdateResultList = ReadEntityList<CDataItemUpdateResult>(buffer);
                return obj;
            }
        }
    }
}

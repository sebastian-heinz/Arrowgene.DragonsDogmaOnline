using System.Collections.Generic;
using Arrowgene.Buffers;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGatheringItemRestriction
    {
        public CDataGatheringItemRestriction()
        {
            Unk1 = new();
            RequiredItems = new();
        }

        public uint RestrictionId { get; set; }
        public List<CDataWalletPoint> Unk1 { get; set; }
        public List<CDataItemAmount> RequiredItems { get; set; }

        public class Serializer : EntitySerializer<CDataGatheringItemRestriction>
        {
            public override void Write(IBuffer buffer, CDataGatheringItemRestriction obj)
            {
                WriteUInt32(buffer, obj.RestrictionId);
                WriteEntityList(buffer, obj.Unk1);
                WriteEntityList(buffer, obj.RequiredItems);
            }
        
            public override CDataGatheringItemRestriction Read(IBuffer buffer)
            {
                CDataGatheringItemRestriction obj = new CDataGatheringItemRestriction();
                obj.RestrictionId = ReadUInt32(buffer);
                obj.Unk1 = ReadEntityList<CDataWalletPoint>(buffer);
                obj.RequiredItems = ReadEntityList<CDataItemAmount>(buffer);
                return obj;
            }
        }
    }
}

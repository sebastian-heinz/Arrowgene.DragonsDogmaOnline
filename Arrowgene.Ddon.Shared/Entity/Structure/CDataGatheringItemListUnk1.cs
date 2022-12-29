using System.Collections.Generic;
using Arrowgene.Buffers;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGatheringItemListUnk1
    {    
        public uint Unk0 { get; set; }
        public List<CDataWalletPoint> Unk1 { get; set; }
        public List<CDataGatheringItemListUnk1Unk2> Unk2 { get; set; }

        public class Serializer : EntitySerializer<CDataGatheringItemListUnk1>
        {
            public override void Write(IBuffer buffer, CDataGatheringItemListUnk1 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteEntityList<CDataWalletPoint>(buffer, obj.Unk1);
                WriteEntityList<CDataGatheringItemListUnk1Unk2>(buffer, obj.Unk2);
            }
        
            public override CDataGatheringItemListUnk1 Read(IBuffer buffer)
            {
                CDataGatheringItemListUnk1 obj = new CDataGatheringItemListUnk1();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadEntityList<CDataWalletPoint>(buffer);
                obj.Unk2 = ReadEntityList<CDataGatheringItemListUnk1Unk2>(buffer);
                return obj;
            }
        }
    }
}
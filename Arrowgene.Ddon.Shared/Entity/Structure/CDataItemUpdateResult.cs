using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataItemUpdateResult
    {
        public CDataItemUpdateResult()
        {
            ItemList = new CDataItemList();
            UpdateItemNum=0;
        }

        public CDataItemList ItemList { get; set; }
        public int UpdateItemNum { get; set; }

        public class Serializer : EntitySerializer<CDataItemUpdateResult>
        {
            public override void Write(IBuffer buffer, CDataItemUpdateResult obj)
            {
                WriteEntity(buffer, obj.ItemList);
                WriteInt32(buffer, obj.UpdateItemNum);
            }

            public override CDataItemUpdateResult Read(IBuffer buffer)
            {
                CDataItemUpdateResult obj = new CDataItemUpdateResult();
                obj.ItemList = ReadEntity<CDataItemList>(buffer);
                obj.UpdateItemNum = ReadInt32(buffer);
                return obj;
            }
        }
    }
}

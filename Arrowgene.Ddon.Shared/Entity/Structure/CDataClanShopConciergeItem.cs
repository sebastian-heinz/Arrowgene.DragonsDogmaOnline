using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanShopConciergeItem
    {
        public uint NpcId { get; set; }
        public uint RequireClanPoint { get; set; }

        public class Serializer : EntitySerializer<CDataClanShopConciergeItem>
        {
            public override void Write(IBuffer buffer, CDataClanShopConciergeItem obj)
            {
                WriteUInt32(buffer, obj.NpcId);
                WriteUInt32(buffer, obj.RequireClanPoint);
            }

            public override CDataClanShopConciergeItem Read(IBuffer buffer)
            {
                CDataClanShopConciergeItem obj = new CDataClanShopConciergeItem();
                obj.NpcId = ReadUInt32(buffer);
                obj.RequireClanPoint = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

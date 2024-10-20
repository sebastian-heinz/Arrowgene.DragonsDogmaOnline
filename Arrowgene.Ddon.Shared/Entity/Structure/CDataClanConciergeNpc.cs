using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanConciergeNpc
    {
        public uint NpcId { get; set; }
        public uint Price { get; set; }
        public bool IsInit {  get; set; }
        public uint SortId { get; set; }

        public class Serializer : EntitySerializer<CDataClanConciergeNpc>
        {
            public override void Write(IBuffer buffer, CDataClanConciergeNpc obj)
            {
                WriteUInt32(buffer, obj.NpcId);
                WriteUInt32(buffer, obj.Price);
                WriteBool(buffer, obj.IsInit);
                WriteUInt32(buffer, obj.SortId);
            }

            public override CDataClanConciergeNpc Read(IBuffer buffer)
            {
                CDataClanConciergeNpc obj = new CDataClanConciergeNpc();
                obj.NpcId = ReadUInt32(buffer);
                obj.Price = ReadUInt32(buffer);
                obj.IsInit = ReadBool(buffer);
                obj.SortId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

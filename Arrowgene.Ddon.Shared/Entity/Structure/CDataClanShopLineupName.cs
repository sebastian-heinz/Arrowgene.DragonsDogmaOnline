using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanShopLineupName
    {
        public CDataClanShopLineupName()
        {
            Name = string.Empty;
        }

        public uint LineupID { get; set; }
        public string Name { get; set; }

        public class Serializer : EntitySerializer<CDataClanShopLineupName>
        {
            public override void Write(IBuffer buffer, CDataClanShopLineupName obj)
            {
                WriteUInt32(buffer, obj.LineupID);
                WriteMtString(buffer, obj.Name);
            }

            public override CDataClanShopLineupName Read(IBuffer buffer)
            {
                CDataClanShopLineupName obj = new CDataClanShopLineupName();
                obj.LineupID = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                return obj;
            }
        }
    }
}

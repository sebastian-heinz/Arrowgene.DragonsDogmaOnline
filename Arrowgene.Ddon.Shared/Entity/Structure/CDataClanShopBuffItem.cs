using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanShopBuffItem
    {
        public CDataClanShopBuffItem()
        {
            BuffInfo = new();
            RequireLineupId = new();
        }

        public uint LineupId { get; set; }
        public uint RequireClanPoint {  get; set; }
        public byte RequireLevel { get; set; }
        public uint IconID { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CDataClanShopBuffInfo> BuffInfo { get; set; }
        public List<CDataCommonU32> RequireLineupId { get; set; }

        public class Serializer : EntitySerializer<CDataClanShopBuffItem>
        {
            public override void Write(IBuffer buffer, CDataClanShopBuffItem obj)
            {
                WriteUInt32(buffer, obj.LineupId);
                WriteUInt32(buffer, obj.RequireClanPoint);
                WriteByte(buffer, obj.RequireLevel);
                WriteUInt32(buffer, obj.IconID);
                WriteMtString(buffer, obj.Name);
                WriteEntityList<CDataClanShopBuffInfo>(buffer, obj.BuffInfo);
                WriteEntityList<CDataCommonU32>(buffer, obj.RequireLineupId);
            }

            public override CDataClanShopBuffItem Read(IBuffer buffer)
            {
                CDataClanShopBuffItem obj = new CDataClanShopBuffItem();

                obj.LineupId = ReadUInt32(buffer);
                obj.RequireClanPoint = ReadUInt32(buffer);
                obj.RequireLevel = ReadByte(buffer);
                obj.IconID = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.BuffInfo = ReadEntityList<CDataClanShopBuffInfo>(buffer);
                obj.RequireLineupId = ReadEntityList<CDataCommonU32>(buffer);

                return obj;
            }
        }
    }
}

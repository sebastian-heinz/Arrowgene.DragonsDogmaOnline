using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanShopFunctionItem
    {
        public CDataClanShopFunctionItem()
        {
            FunctionInfo = new();
            RequireLineupId = new();
        }

        public uint LineupId { get; set; }
        public uint RequireClanPoint { get; set; }
        public byte RequireLevel { get; set; }
        public uint IconID { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CDataClanShopFunctionInfo> FunctionInfo { get; set; }
        public List<CDataCommonU32> RequireLineupId { get; set; }

        public class Serializer : EntitySerializer<CDataClanShopFunctionItem>
        {
            public override void Write(IBuffer buffer, CDataClanShopFunctionItem obj)
            {
                WriteUInt32(buffer, obj.LineupId);
                WriteUInt32(buffer, obj.RequireClanPoint);
                WriteByte(buffer, obj.RequireLevel);
                WriteUInt32(buffer, obj.IconID);
                WriteMtString(buffer, obj.Name);
                WriteEntityList<CDataClanShopFunctionInfo>(buffer, obj.FunctionInfo);
                WriteEntityList<CDataCommonU32>(buffer, obj.RequireLineupId);
            }

            public override CDataClanShopFunctionItem Read(IBuffer buffer)
            {
                CDataClanShopFunctionItem obj = new CDataClanShopFunctionItem();

                obj.LineupId = ReadUInt32(buffer);
                obj.RequireClanPoint = ReadUInt32(buffer);
                obj.RequireLevel = ReadByte(buffer);
                obj.IconID = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.FunctionInfo = ReadEntityList<CDataClanShopFunctionInfo>(buffer);
                obj.RequireLineupId = ReadEntityList<CDataCommonU32>(buffer);

                return obj;
            }
        }
    }
}

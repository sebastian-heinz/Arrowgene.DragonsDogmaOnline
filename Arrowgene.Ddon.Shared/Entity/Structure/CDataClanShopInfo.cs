using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanShopInfo
    {
        public CDataClanShopInfo()
        {
            CurrentFunctionList = new();
            CurrentBuffList = new();
        }

        public uint ClanPoint { get; set; }
        public List<CDataCommonU32> CurrentFunctionList { get; set; }
        public List<CDataCommonU32> CurrentBuffList {  get; set; }

        public class Serializer : EntitySerializer<CDataClanShopInfo>
        {
            public override void Write(IBuffer buffer, CDataClanShopInfo obj)
            {
                WriteUInt32(buffer, obj.ClanPoint);
                WriteEntityList<CDataCommonU32>(buffer, obj.CurrentFunctionList);
                WriteEntityList<CDataCommonU32>(buffer, obj.CurrentBuffList);
            }

            public override CDataClanShopInfo Read(IBuffer buffer)
            {
                CDataClanShopInfo obj = new CDataClanShopInfo();
                obj.ClanPoint = ReadUInt32(buffer);
                obj.CurrentFunctionList = ReadEntityList<CDataCommonU32>(buffer);
                obj.CurrentBuffList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}

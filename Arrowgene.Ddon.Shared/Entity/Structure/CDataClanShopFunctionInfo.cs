using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanShopFunctionInfo
    {
        public uint FunctionId { get; set; }
        public byte FunctionType { get; set; }

        public class Serializer : EntitySerializer<CDataClanShopFunctionInfo>
        {
            public override void Write(IBuffer buffer, CDataClanShopFunctionInfo obj)
            {
                WriteUInt32(buffer, obj.FunctionId);
                WriteByte(buffer, obj.FunctionType);
            }

            public override CDataClanShopFunctionInfo Read(IBuffer buffer)
            {
                CDataClanShopFunctionInfo obj = new CDataClanShopFunctionInfo();
                obj.FunctionId = ReadUInt32(buffer);
                obj.FunctionType = ReadByte(buffer);
                return obj;
            }
        }
    }
}

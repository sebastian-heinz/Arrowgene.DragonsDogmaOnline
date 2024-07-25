using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGPDetail
    {
        public uint GP { get; set; }
        public uint Max { get; set; }
        public bool isFree { get; set; }

        public uint Type {  get; set; }

        public ulong Expire { get; set; }

        public ulong Created { get; set; }
        public CDataGPDetail()
        {
            GP = 0;
            Max = 0;
            isFree = false;
            Type = 0; //TODO: Figure this out.
            Expire = ulong.MaxValue;
            Created = 0;
        }

        public class Serializer : EntitySerializer<CDataGPDetail>
        {
            public override void Write(IBuffer buffer, CDataGPDetail obj)
            {
                WriteUInt32(buffer, obj.GP);
                WriteUInt32(buffer, obj.Max);
                WriteBool(buffer, obj.isFree);
                WriteUInt32(buffer, obj.Type);
                WriteUInt64(buffer, obj.Expire);
                WriteUInt64(buffer, obj.Created);
            }

            public override CDataGPDetail Read(IBuffer buffer)
            {
                CDataGPDetail obj = new CDataGPDetail();
                obj.GP = ReadUInt32(buffer);
                obj.Max = ReadUInt32(buffer);
                obj.isFree = ReadBool(buffer);
                obj.Type = ReadUInt32(buffer);
                obj.Expire = ReadUInt64(buffer);
                obj.Created = ReadUInt64(buffer);
                
                return obj;
            }
        }
    }
}

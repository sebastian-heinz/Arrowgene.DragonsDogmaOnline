using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
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
        public GPDetailType Type {  get; set; }
        public DateTimeOffset Expire { get; set; }
        public DateTimeOffset Created { get; set; }

        public CDataGPDetail()
        {
            GP = 0;
            Max = 0;
            isFree = false;
            Type = GPDetailType.None;
            Expire = DateTimeOffset.MaxValue;
            Created = DateTimeOffset.UnixEpoch;
        }

        public class Serializer : EntitySerializer<CDataGPDetail>
        {
            public override void Write(IBuffer buffer, CDataGPDetail obj)
            {
                WriteUInt32(buffer, obj.GP);
                WriteUInt32(buffer, obj.Max);
                WriteBool(buffer, obj.isFree);
                WriteUInt32(buffer, (uint)obj.Type);
                WriteInt64(buffer, obj.Expire.ToUnixTimeSeconds());
                WriteInt64(buffer, obj.Created.ToUnixTimeSeconds());
            }

            public override CDataGPDetail Read(IBuffer buffer)
            {
                CDataGPDetail obj = new CDataGPDetail();
                obj.GP = ReadUInt32(buffer);
                obj.Max = ReadUInt32(buffer);
                obj.isFree = ReadBool(buffer);
                obj.Type = (GPDetailType)ReadUInt32(buffer);
                obj.Expire = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.Created = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                
                return obj;
            }
        }
    }
}

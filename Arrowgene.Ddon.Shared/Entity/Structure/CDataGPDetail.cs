using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGPDetail
    {
        public uint GP { get; set; }
        public uint Max { get; set; }
        public bool IsFree { get; set; }
        public GPDetailType GetType { get; set; }
        public DateTimeOffset Expire { get; set; }
        public DateTimeOffset Created { get; set; }

        public CDataGPDetail()
        {
            GP = 0;
            Max = 0;
            IsFree = false;
            GetType = GPDetailType.None;
            Expire = DateTimeOffset.MaxValue;
            Created = DateTimeOffset.UnixEpoch;
        }

        public class Serializer : EntitySerializer<CDataGPDetail>
        {
            public override void Write(IBuffer buffer, CDataGPDetail obj)
            {
                WriteUInt32(buffer, obj.GP);
                WriteUInt32(buffer, obj.Max);
                WriteBool(buffer, obj.IsFree);
                WriteUInt32(buffer, (uint)obj.GetType);
                WriteInt64(buffer, obj.Expire.ToUnixTimeSeconds());
                WriteInt64(buffer, obj.Created.ToUnixTimeSeconds());
            }

            public override CDataGPDetail Read(IBuffer buffer)
            {
                CDataGPDetail obj = new CDataGPDetail();
                obj.GP = ReadUInt32(buffer);
                obj.Max = ReadUInt32(buffer);
                obj.IsFree = ReadBool(buffer);
                obj.GetType = (GPDetailType)ReadUInt32(buffer);
                obj.Expire = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.Created = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));

                return obj;
            }
        }
    }
}

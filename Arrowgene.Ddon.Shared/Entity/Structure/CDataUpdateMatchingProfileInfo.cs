using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataUpdateMatchingProfileInfo
    {
        public CDataUpdateMatchingProfileInfo()
        {
            CharacterId = 0;
            Comment = "";
        }

        public uint CharacterId { get; set; }
        public String Comment { get; set; }

        public class Serializer : EntitySerializer<CDataUpdateMatchingProfileInfo>
        {
            public override void Write(IBuffer buffer, CDataUpdateMatchingProfileInfo obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteMtString(buffer, obj.Comment);
            }

            public override CDataUpdateMatchingProfileInfo Read(IBuffer buffer)
            {
                CDataUpdateMatchingProfileInfo obj = new CDataUpdateMatchingProfileInfo();
                obj.CharacterId = ReadUInt32(buffer);
                obj.Comment = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
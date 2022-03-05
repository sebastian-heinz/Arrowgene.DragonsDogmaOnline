using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CCommunityCharacterBaseInfo
    {
        public CCommunityCharacterBaseInfo()
        {
            CharacterId = 0;
            StrFirstName = string.Empty;
            StrLastName = string.Empty;
            StrClanName = string.Empty;
        }

        public uint CharacterId { get; set; }
        public string StrFirstName { get; set; }
        public string StrLastName { get; set; }
        public string StrClanName { get; set; }

        public class Serializer : EntitySerializer<CCommunityCharacterBaseInfo>
        {
            public override void Write(IBuffer buffer, CCommunityCharacterBaseInfo obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteMtString(buffer, obj.StrFirstName);
                WriteMtString(buffer, obj.StrLastName);
                WriteMtString(buffer, obj.StrClanName);
            }

            public override CCommunityCharacterBaseInfo Read(IBuffer buffer)
            {
                CCommunityCharacterBaseInfo obj = new CCommunityCharacterBaseInfo();
                obj.CharacterId = ReadByte(buffer);
                obj.StrFirstName = ReadMtString(buffer);
                obj.StrLastName = ReadMtString(buffer);
                obj.StrClanName = ReadMtString(buffer);
                return obj;
            }
        }
    }
}

using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterListElement
    {
        public CDataCommunityCharacterBaseInfo CommunityCharacterBaseInfo { get; set; } = new();
        public ushort ServerId { get; set; }
        public OnlineStatus OnlineStatus { get; set; }
        public CDataJobBaseInfo CurrentJobBaseInfo { get; set; } = new();
        public CDataJobBaseInfo EntryJobBaseInfo { get; set; } = new();
        public string MatchingProfile { get; set; } = string.Empty;
        public byte unk2 { get; set; } // Party type?

        public class Serializer : EntitySerializer<CDataCharacterListElement>
        {
            public override void Write(IBuffer buffer, CDataCharacterListElement obj)
            {
                WriteEntity(buffer, obj.CommunityCharacterBaseInfo);
                WriteUInt16(buffer, obj.ServerId);
                WriteByte(buffer, (byte) obj.OnlineStatus);
                WriteEntity(buffer, obj.CurrentJobBaseInfo);
                WriteEntity(buffer, obj.EntryJobBaseInfo);
                WriteMtString(buffer, obj.MatchingProfile);
                WriteByte(buffer, obj.unk2);
            }

            public override CDataCharacterListElement Read(IBuffer buffer)
            {
                CDataCharacterListElement obj = new CDataCharacterListElement();
                obj.CommunityCharacterBaseInfo = ReadEntity<CDataCommunityCharacterBaseInfo>(buffer);
                obj.ServerId = ReadUInt16(buffer);
                obj.OnlineStatus = (OnlineStatus) ReadByte(buffer);
                obj.CurrentJobBaseInfo = ReadEntity<CDataJobBaseInfo>(buffer);
                obj.EntryJobBaseInfo = ReadEntity<CDataJobBaseInfo>(buffer);
                obj.MatchingProfile = ReadMtString(buffer);
                obj.unk2 = ReadByte(buffer);
                return obj;
            }
        }
    }
}

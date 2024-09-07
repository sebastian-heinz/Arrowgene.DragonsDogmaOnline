using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterListElement
    {
        public CDataCharacterListElement()
        {
            CommunityCharacterBaseInfo = new CDataCommunityCharacterBaseInfo();
            ServerId = 0;
            OnlineStatus = 0;
            CurrentJobBaseInfo = new CDataJobBaseInfo();
            EntryJobBaseInfo = new CDataJobBaseInfo();
            MatchingProfile = "";
            unk2 = 0;
        }

        public CDataCommunityCharacterBaseInfo CommunityCharacterBaseInfo { get; set; }
        public ushort ServerId;
        public OnlineStatus OnlineStatus;
        public CDataJobBaseInfo CurrentJobBaseInfo;
        public CDataJobBaseInfo EntryJobBaseInfo;
        public string MatchingProfile;
        public byte unk2; // Party type?
        
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

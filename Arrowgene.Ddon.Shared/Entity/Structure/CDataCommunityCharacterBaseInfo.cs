using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataCommunityCharacterBaseInfo
{
    public CDataCommunityCharacterBaseInfo()
    {
        CharacterId = 0;
        CharacterName = new CDataCharacterName();
        ClanName = string.Empty;
    }
    
    public uint CharacterId { get; set; }
    public CDataCharacterName CharacterName { get; set; }
    public string ClanName { get; set; }

    public class Serializer : EntitySerializer<CDataCommunityCharacterBaseInfo>
    {
        public override void Write(IBuffer buffer, CDataCommunityCharacterBaseInfo obj)
        {
            WriteUInt32(buffer, obj.CharacterId);
            WriteEntity(buffer, obj.CharacterName);
            WriteMtString(buffer, obj.ClanName);
        }

        public override CDataCommunityCharacterBaseInfo Read(IBuffer buffer)
        {
            CDataCommunityCharacterBaseInfo obj = new CDataCommunityCharacterBaseInfo();
            obj.CharacterId = ReadUInt32(buffer);
            obj.CharacterName = ReadEntity<CDataCharacterName>(buffer);
            obj.ClanName = ReadMtString(buffer);
            return obj;
        }
    }
}

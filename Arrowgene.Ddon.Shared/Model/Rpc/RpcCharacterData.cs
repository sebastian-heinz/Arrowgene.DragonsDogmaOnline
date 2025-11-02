using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model.Rpc
{
    public class RpcCharacterData
    {
        public RpcCharacterData()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            ClanName = string.Empty;
            ClanShortName = string.Empty;
        }

        public RpcCharacterData(Character character)
        {
            CharacterId = character.CharacterId;
            FirstName = character.FirstName;
            LastName = character.LastName;
            ClanName = character.ClanName.Name;
            ClanShortName = character.ClanName.ShortName;
            ClanId = character.ClanId;
        }

        public uint CharacterId { get; set; }
        public uint ClanId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClanName { get; set; }
        public string ClanShortName { get; set; }

        public CDataCommunityCharacterBaseInfo CommunityCharacterBaseInfo
        {
            get
            {
                return new()
                {
                    CharacterId = CharacterId,
                    ClanName = ClanName,
                    CharacterName = new()
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                    }
                };
            }
        }
    }
}

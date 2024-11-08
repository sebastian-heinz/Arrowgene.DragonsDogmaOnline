namespace Arrowgene.Ddon.Shared.Model
{
    public class RpcCharacterData
    {
        public RpcCharacterData()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            ClanName = string.Empty;
        }

        public RpcCharacterData(Character character)
        {
            CharacterId = character.CharacterId;
            FirstName = character.FirstName;
            LastName = character.LastName;
            ClanName = character.ClanName.ShortName;
            ClanId = character.ClanId;
        }

        public uint CharacterId { get; set; }
        public uint ClanId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClanName { get; set; }
    }
}

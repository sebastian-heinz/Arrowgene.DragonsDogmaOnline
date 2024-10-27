using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public class CharacterSummary
    {
        public CharacterSummary()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        public CharacterSummary(Character character)
        {
            CharacterId = character.CharacterId;
            FirstName = character.FirstName;
            LastName = character.LastName;
        }

        public uint CharacterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}

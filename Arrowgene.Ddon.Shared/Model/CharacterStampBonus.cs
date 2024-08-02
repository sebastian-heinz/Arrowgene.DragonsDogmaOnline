using System;

namespace Arrowgene.Ddon.Shared.Model
{
    public class CharacterStampBonus
    {
        public CharacterStampBonus() {
            LastStamp = DateTime.MinValue;
            ConsecutiveStamp = 0;
            TotalStamp = 0;
        }
        public DateTime LastStamp { get; set; }
        public ushort ConsecutiveStamp {  get; set; }
        public ushort TotalStamp { get; set; }
    }
}

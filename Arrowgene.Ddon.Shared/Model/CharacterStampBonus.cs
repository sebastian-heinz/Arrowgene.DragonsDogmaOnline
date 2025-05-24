using System;

namespace Arrowgene.Ddon.Shared.Model;

public class CharacterStampBonus
{
    private static readonly DateTime DateTimeSafeMinValue = new(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public CharacterStampBonus()
    {
        LastStamp = DateTimeSafeMinValue;
        ConsecutiveStamp = 0;
        TotalStamp = 0;
    }

    public DateTime LastStamp { get; set; }
    public ushort ConsecutiveStamp { get; set; }
    public ushort TotalStamp { get; set; }
}

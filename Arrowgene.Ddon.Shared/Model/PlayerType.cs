using System;

namespace Arrowgene.Ddon.Shared.Model
{
    [Flags]
    public enum PlayerType
    {
        None       = 0,
        Player     = 1,
        MyPawn     = 2,
        RentedPawn = 4
    }
}

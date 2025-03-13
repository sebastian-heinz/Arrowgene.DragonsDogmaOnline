using Arrowgene.Ddon.Shared.Entity.Structure;
using System;

namespace Arrowgene.Ddon.Shared.Model
{
    public class PartnerPawnData
    {
        public uint PawnId { get; set; }
        public uint NumGifts { get; set; }
        public uint NumCrafts { get; set; }
        public uint NumAdventures { get; set; }

        public uint CalculateLikability()
        {
            uint totalXP = (NumGifts * 20) + (NumCrafts * 20) + (NumAdventures * 10);
            uint level = (totalXP < 65) ? 0 : (uint)Math.Sqrt((totalXP - 65) / 14.4);
            return Math.Min(level, 25);  // Clamp 0 to 25
        }

        public CDataPartnerPawnData ToCDataPartnerPawnData(Pawn pawn)
        {
            return new CDataPartnerPawnData()
            {
                PawnId = PawnId,
                Personality = pawn.EditInfo.Personality,
                Likability = CalculateLikability()
            };
        }
    }
}

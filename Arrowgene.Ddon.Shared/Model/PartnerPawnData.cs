using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.Shared.Model
{
    public class PartnerPawnData
    {
        public uint PawnId { get; set; }
        public uint NumGifts { get; set; }
        public uint NumCrafts { get; set; }
        public uint NumAdventures { get; set; }

        private static readonly double OffsetFactor = 65;
        private static readonly double ScaleFactor = 14.4;
        public static readonly uint MaxLevel = 25;

        public uint CalculateLikabilityXP()
        {
            return (NumGifts * 20) + (NumCrafts * 20) + (NumAdventures * 10);
        }

        public uint CalculateLikability()
        {
            uint totalXP = CalculateLikabilityXP();
            uint level = (totalXP < OffsetFactor) ? 0 : (uint)Math.Sqrt((totalXP - OffsetFactor) / ScaleFactor);
            return Math.Min(level, MaxLevel);  // Clamp 0 to 25
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

        public static readonly List<uint> LikabilityCurve = [.. Enumerable.Range(0, (int)(MaxLevel + 1)).Select(x => CalculateLikabilityToLevel((uint)x))];

        public static uint CalculateLikabilityToLevel(uint level)
        {
            if (level > MaxLevel)
            {
                return CalculateLikabilityToLevel(MaxLevel);
            }

            return level > 0 ? (uint)(OffsetFactor + ScaleFactor * Math.Pow((level - 1),2)) : 0;
        }
    }
}

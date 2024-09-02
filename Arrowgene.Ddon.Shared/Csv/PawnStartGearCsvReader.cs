using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class PawnStartGearCsvReader : CsvReaderWriter<PawnStartGearCsv>
    {
        protected override int NumExpectedItems => 16;

        protected override PawnStartGearCsv CreateInstance(string[] properties)
        {
            if (!byte.TryParse(properties[0], out byte job)) return null;
            if (!ushort.TryParse(properties[1], out ushort primary)) return null;
            if (!ushort.TryParse(properties[2], out ushort secondary)) return null;
            if (!ushort.TryParse(properties[3], out ushort head)) return null;
            if (!ushort.TryParse(properties[4], out ushort body)) return null;
            if (!ushort.TryParse(properties[5], out ushort bodyClothing)) return null;
            if (!ushort.TryParse(properties[6], out ushort arm)) return null;
            if (!ushort.TryParse(properties[7], out ushort leg)) return null;
            if (!ushort.TryParse(properties[8], out ushort legWear)) return null;
            if (!ushort.TryParse(properties[9], out ushort overWear)) return null;
            if (!ushort.TryParse(properties[10], out ushort jewelrySlot1)) return null;
            if (!ushort.TryParse(properties[11], out ushort jewelrySlot2)) return null;
            if (!ushort.TryParse(properties[12], out ushort jewelrySlot3)) return null;
            if (!ushort.TryParse(properties[13], out ushort jewelrySlot4)) return null;
            if (!ushort.TryParse(properties[14], out ushort jewelrySlot5)) return null;
            if (!ushort.TryParse(properties[15], out ushort lantern)) return null;

            return new PawnStartGearCsv
            {
                Job = (JobId) job,
                Primary = primary,
                Secondary = secondary,
                Head = head,
                Body = body,
                BodyClothing = bodyClothing,
                Arm = arm,
                Leg = leg,
                LegWear = legWear,
                OverWear = overWear,
                JewelrySlot1 = jewelrySlot1,
                JewelrySlot2 = jewelrySlot2,
                JewelrySlot3 = jewelrySlot3,
                JewelrySlot4 = jewelrySlot4,
                JewelrySlot5 = jewelrySlot5,
                Lantern = lantern,
            };
        }
    }
}

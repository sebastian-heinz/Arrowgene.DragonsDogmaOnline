using Arrowgene.Ddon.Shared.Csv;

public class WarpPointCsv : CsvReaderWriter<WarpPoint>
{
    protected override int NumExpectedItems => 2;

    protected override WarpPoint CreateInstance(string[] properties)
    {
        if (!uint.TryParse(properties[0], out uint warpPointId)) return null;
        if (!uint.TryParse(properties[1], out uint price)) return null;

        return new WarpPoint()
        {
            WarpPointId = warpPointId,
            Price = price
        };
    }
}
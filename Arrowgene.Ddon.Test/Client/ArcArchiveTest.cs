using System.IO;
using Arrowgene.Ddon.Client;
using Xunit;

namespace Arrowgene.Ddon.Test.Client;

public class ArcArchiveTest
{
    [Fact]
    public void TestArcFlags()
    {
        uint flags = 1073925413;
        uint sizeBits28__0_29 = flags & ((1 << 29) - 1);
        uint compressionBits3__29_31 = (flags >> 29) & ((1 << 3) - 1);
        ArcArchive.ArcCompression compression = (ArcArchive.ArcCompression)compressionBits3__29_31;

        uint reFlags = sizeBits28__0_29
                       | compressionBits3__29_31 << 29;

        Assert.True(flags == reFlags);
    }

    [Fact]
    public void TestArcPacking()
    {
        string arcTest = TestUtils.GetTestPath("arc_test.bin");
        byte[] arcBytes = File.ReadAllBytes(arcTest);
        ArcArchive archive = new ArcArchive();
        archive.Open(arcBytes);
        byte[] savedArcBytes = archive.Save();
        File.WriteAllBytes(arcTest + ".1.bin", savedArcBytes);

        archive.Open(savedArcBytes);
        byte[] savedArcBytesTwo = archive.Save();
        File.WriteAllBytes(arcTest + ".2.bin", savedArcBytesTwo);


        Assert.Equal(savedArcBytes, savedArcBytesTwo);
        Assert.Equal(arcBytes, savedArcBytes);
    }
}

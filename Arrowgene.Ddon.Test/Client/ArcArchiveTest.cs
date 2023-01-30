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
}

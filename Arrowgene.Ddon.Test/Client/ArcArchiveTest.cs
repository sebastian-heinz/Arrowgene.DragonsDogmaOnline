using System.Collections.Generic;
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
        Assert.Equal(ArcArchive.ArcCompression.Normal, compression);
    }

    [Fact]
    public void TestArcPacking()
    {
        string arcTest = TestUtils.GetTestPath("arc_test.bin");
        byte[] arcBytes = File.ReadAllBytes(arcTest);
        ArcArchive archiveExpected = new ArcArchive();
        archiveExpected.Open(arcBytes);

        byte[] savedArcBytes = archiveExpected.Save();
        ArcArchive archiveActual = new ArcArchive();
        archiveActual.Open(savedArcBytes);

        Assert.Equal(archiveExpected.MagicTag, archiveActual.MagicTag);
        Assert.Equal(archiveExpected.MagicId, archiveActual.MagicId);

        List<ArcArchive.ArcFile> expectedArcFiles = archiveExpected.GetFiles();
        List<ArcArchive.ArcFile> actualArcFiles = archiveExpected.GetFiles();
        for (int i = 0; i < expectedArcFiles.Count; i++)
        {
            ArcArchive.ArcFile expectedFile = expectedArcFiles[i];
            ArcArchive.ArcFile actualFile = actualArcFiles[i];
            
            Assert.Equal(expectedFile.Data, actualFile.Data);
            Assert.Equal(expectedFile.Index.IndexOffset, actualFile.Index.IndexOffset);
            Assert.Equal(expectedFile.Index.Name, actualFile.Index.Name);
            Assert.Equal(expectedFile.Index.Directory, actualFile.Index.Directory);
            Assert.Equal(expectedFile.Index.ArcPath, actualFile.Index.ArcPath);
            Assert.Equal(expectedFile.Index.Path, actualFile.Index.Path);
            Assert.Equal(expectedFile.Index.Extension, actualFile.Index.Extension);
            Assert.Equal(expectedFile.Index.JamCrc, actualFile.Index.JamCrc);
            Assert.Equal(expectedFile.Index.Offset, actualFile.Index.Offset);
            Assert.Equal(expectedFile.Index.CompressedSize, actualFile.Index.CompressedSize);
            Assert.Equal(expectedFile.Index.Size, actualFile.Index.Size);
            Assert.Equal(expectedFile.Index.Compression, actualFile.Index.Compression);
            Assert.Equal(expectedFile.Index.ArcExt, actualFile.Index.ArcExt);
        }
    }
}

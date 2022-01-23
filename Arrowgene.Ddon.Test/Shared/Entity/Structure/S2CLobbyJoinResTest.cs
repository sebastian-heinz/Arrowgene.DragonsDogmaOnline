using System.Collections;
using System.IO;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Xunit;

namespace Arrowgene.Ddon.Test.Shared.Entity.Structure
{
    public class S2CLobbyJoinResTest
    {
        [Fact]
        public void TestS2CLobbyJoinResTest()
        {
            // load data
            string testFile = TestUtils.GetTestPath("S2CLobbyJoinResTest.bin");
            byte[] bin = File.ReadAllBytes(testFile);
            IBuffer buffer;
            // deserialize & serialize
            buffer = new StreamBuffer(bin);
            buffer.SetPositionStart();

            S2CLobbyJoinRes deserialized = EntitySerializer.Get<S2CLobbyJoinRes>().Read(buffer);
            buffer = new StreamBuffer();
            EntitySerializer.Get<S2CLobbyJoinRes>().Write(buffer, deserialized);
            byte[] serialized = buffer.GetAllBytes();
            Assert.True(StructuralComparisons.StructuralEqualityComparer.Equals(bin, serialized));
        }
    }
}

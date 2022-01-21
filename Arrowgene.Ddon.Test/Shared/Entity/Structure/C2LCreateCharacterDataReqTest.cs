using System.Collections;
using System.IO;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Xunit;

namespace Arrowgene.Ddon.Test.Shared.Entity.Structure
{
    public class C2LCreateCharacterDataReqTest
    {
        [Fact]
        public void TestC2LCreateCharacterDataReqTestSerializer()
        {
            // load data
            string testFile = TestUtils.GetTestPath("C2LCreateCharacterDataReqTestData.bin");
            byte[] bin = File.ReadAllBytes(testFile);
            IBuffer buffer;
            // deserialize & serialize
            buffer = new StreamBuffer(bin);
            buffer.SetPositionStart();

            C2LCreateCharacterDataReq deserialized = EntitySerializer.Get<C2LCreateCharacterDataReq>().Read(buffer);
            buffer = new StreamBuffer();
            EntitySerializer.Get<C2LCreateCharacterDataReq>().Write(buffer, deserialized);
            byte[] serialized = buffer.GetAllBytes();
            Assert.True(StructuralComparisons.StructuralEqualityComparer.Equals(bin, serialized));
        }
    }
}

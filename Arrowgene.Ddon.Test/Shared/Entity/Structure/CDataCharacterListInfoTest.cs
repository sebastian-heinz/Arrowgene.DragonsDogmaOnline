using System.Collections;
using System.Collections.Generic;
using System.IO;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Xunit;

namespace Arrowgene.Ddon.Test.Shared.Entity.Structure
{
    public class CDataCharacterListInfoTest
    {
        [Fact]
        public void TestCDataCharacterListInfoSerializer()
        {
            // load data
            string testFile = TestUtils.GetTestPath("CDataCharacterListInfoTestData.bin");
            byte[] bin = File.ReadAllBytes(testFile);
            IBuffer buffer;
            // deserialize & serialize
            buffer = new StreamBuffer(bin);
            buffer.SetPositionStart();
            List<CDataCharacterListInfo> deserialized = EntitySerializer.Get<CDataCharacterListInfo>().ReadList(buffer);
            buffer = new StreamBuffer();
            EntitySerializer.Get<CDataCharacterListInfo>().WriteList(buffer, deserialized);
            byte[] serialized = buffer.GetAllBytes();
            Assert.True(StructuralComparisons.StructuralEqualityComparer.Equals(bin, serialized));
        }
    }
}

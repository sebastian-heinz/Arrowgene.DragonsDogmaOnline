using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Shared.Model;
using Xunit;
using Xunit.Abstractions;

namespace Arrowgene.Ddon.Test.Shared.Model
{
    public class ItemTest : TestBase
    {
        public ItemTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TestCharacterOperations()
        {
            Item item1 = new Item() { ItemId = 7553 };
            Item item2 = new Item() { ItemId = 13801 };
            Item item3 = new Item() { ItemId = 13807 };
            Item item4 = new Item() { ItemId = 55 };
            Item item5 = new Item() { ItemId = 9404 };
            Item item6 = new Item() { ItemId = 9405 };
            Item item7 = new Item() { ItemId = 9406 };
            Item item8 = new Item() { ItemId = 35 };

            int uniqueUIds = new Item[] {item1, item2, item3, item4, item5, item6, item7, item8}
                .Select(x => x.UId)
                .Distinct()
                .Count();

            Assert.Equal(8, uniqueUIds);
        }
    }
}
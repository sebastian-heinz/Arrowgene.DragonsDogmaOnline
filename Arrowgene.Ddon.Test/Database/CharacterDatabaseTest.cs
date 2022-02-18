using System.Collections.Generic;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Model;
using Xunit;
using Xunit.Abstractions;

namespace Arrowgene.Ddon.Test.Database
{
    public class CharacterDatabaseTest : TestBase
    {
        public CharacterDatabaseTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TestCharacterOperations()
        {
            DatabaseSetting setting = new DatabaseSetting();
            setting.WipeOnStartup = true;
            IDatabase database = DdonDatabaseBuilder.BuildSqLite(setting.SqLiteFolder, "character.test", true);
            
            

            Account account = database.CreateAccount("test", "test", "test");
            Assert.NotNull(account);

            Character c = new Character();
            c.FirstName = "Foo";
            c.LastName = "Bar";
            c.AccountId = account.Id;

            Assert.True(database.CreateCharacter(c));

            List<Character> characters = database.SelectCharactersByAccountId(account.Id);
            Assert.NotEmpty(characters);
            Assert.True(characters.Count == 2);  // TODO Rumi Injected

            c.FirstName = "NewName";
            Assert.True(database.UpdateCharacter(c));

            characters = database.SelectCharactersByAccountId(account.Id);
            Assert.NotEmpty(characters);
            Assert.True(characters.Count == 2);  // TODO Rumi Injected
            Assert.Equal("NewName", characters[1].FirstName); // TODO Rumi Injected at [0]

            Assert.True(database.DeleteCharacter(c.Id));
            
            characters = database.SelectCharactersByAccountId(account.Id);
            Assert.True(characters.Count == 1);  // TODO Rumi Injected
            //Assert.Empty(characters);
        }
    }
}

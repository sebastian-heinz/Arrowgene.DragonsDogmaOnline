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
            IDatabase database = DdonDatabaseBuilder.BuildSqLite(setting, true);

            Account account = database.CreateAccount("test", "test", "test");
            Assert.NotNull(account);

            Character c = new Character();
            c.FirstName = "Foo";
            c.LastName = "Bar";
            c.AccountId = account.Id;
            c.Job = JobId.Fighter;
            c.GameMode = GameMode.Normal;
            c.Storage.AddStorage(StorageType.StorageBoxNormal, new Storage(StorageType.StorageBoxNormal, 100));

            Assert.True(database.CreateCharacter(c));

            List<Character> characters = database.SelectCharactersByAccountId(account.Id, GameMode.Normal);
            Assert.NotEmpty(characters);
            Assert.True(characters.Count == 1);

            c.FirstName = "NewName";
            Assert.True(database.UpdateCharacterBaseInfo(c));

            characters = database.SelectCharactersByAccountId(account.Id, GameMode.Normal);
            Assert.NotEmpty(characters);
            Assert.True(characters.Count == 1);
            Assert.Equal("NewName", characters[0].FirstName);

            Assert.True(database.DeleteCharacter(c.CharacterId));
            
            characters = database.SelectCharactersByAccountId(account.Id, GameMode.Normal);
            Assert.True(characters.Count == 0);
            Assert.Empty(characters);
        }
    }
}

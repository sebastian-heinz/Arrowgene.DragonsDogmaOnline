﻿using System.Collections.Generic;
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
            IDatabase database = DdonDatabaseBuilder.BuildSqLite(setting.SqLiteFolder, "character.test");

            Account account = database.CreateAccount("test", "test", "test");
            Assert.NotNull(account);

            Character c = new Character();
            c.FirstName = "Foo";
            c.LastName = "Bar";
            c.AccountId = account.Id;

            Assert.True(database.CreateCharacter(c));

            List<Character> characters = database.SelectCharactersByAccountId(account.Id);
            Assert.NotEmpty(characters);
            Assert.True(characters.Count == 1);

            c.FirstName = "NewName";
            Assert.True(database.UpdateCharacter(c));

            characters = database.SelectCharactersByAccountId(account.Id);
            Assert.NotEmpty(characters);
            Assert.True(characters.Count == 1);
            Assert.Equal("NewName", characters[0].FirstName);

            Assert.True(database.DeleteCharacter(c.Id));
            
            characters = database.SelectCharactersByAccountId(account.Id);
            Assert.Empty(characters);
        }
    }
}
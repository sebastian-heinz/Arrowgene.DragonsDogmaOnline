using Arrowgene.Ddon.Cli;
using Xunit;

namespace Arrowgene.Ddon.Test.GameServer
{
    public class SettingTests
    {
        [Fact]
        public void Serialize_DefaultSetting_ObjectShouldMatchJson()
        {
            Setting setting = new Setting();

            string json = Setting.Serialize(setting);

            Assert.Contains("\"LogPath\": \"Logs\"", json);
        }

        [Fact]
        public void Deserialize_ValidJson_ShouldReturnCorrectObject()
        {
            string json = @"{
                ""LogPath"": ""Logs"",
                ""AssetPath"": ""C:\\Assets"",
                ""GameServerSetting"": {
                    ""ServerSetting"": {
                        ""Id"": 10,
                        ""Name"": ""CustomServerName"",
                        ""ServerPort"": 42000
                    }
                }
            }";

            Setting setting = Setting.Deserialize(json);

            Assert.Equal("Logs", setting.LogPath);
            Assert.Equal("C:\\Assets", setting.AssetPath);
            Assert.Equal("CustomServerName", setting.GameServerSetting.ServerSetting.Name);
            Assert.Equal(42000, setting.GameServerSetting.ServerSetting.ServerPort);
        }

        [Fact]
        public void Deserialize_MissingFields_ShouldAssignDefaultValues()
        {
            string json = "{\"LogPath\": \"Logs\"}";

            Setting setting = Setting.Deserialize(json);

            Assert.Equal("Logs", setting.LogPath);
            Assert.NotNull(setting.GameServerSetting);
            Assert.Equal("Game", setting.GameServerSetting.ServerSetting.Name);
            Assert.Equal(52000, setting.GameServerSetting.ServerSetting.ServerPort);
        }

        [Fact]
        public void RoundTrip_SerializeAndDeserialize_ShouldMaintainObjectState()
        {
            Setting originalSetting = new Setting();

            string json = Setting.Serialize(originalSetting);
            Setting deserializedSetting = Setting.Deserialize(json);
            Assert.Equal(originalSetting.LogPath, deserializedSetting.LogPath);
        }
    }
}

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
            Assert.Contains("\"GameLogicSetting\":", json);
            Assert.Contains("\"AdditionalProductionSpeedFactor\": 1", json);
            Assert.Contains("\"AdditionalCostPerformanceFactor\": 1", json);
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
                    },
                    ""GameLogicSetting"": {
                        ""AdditionalProductionSpeedFactor"": 1.5,
                        ""AdditionalCostPerformanceFactor"": 1.2
                    }
                }
            }";

            Setting setting = Setting.Deserialize(json);

            Assert.Equal("Logs", setting.LogPath);
            Assert.Equal("C:\\Assets", setting.AssetPath);
            Assert.Equal("CustomServerName", setting.GameServerSetting.ServerSetting.Name);
            Assert.Equal(42000, setting.GameServerSetting.ServerSetting.ServerPort);
            Assert.Equal(1.5, setting.GameServerSetting.GameLogicSetting.AdditionalProductionSpeedFactor);
            Assert.Equal(1.2, setting.GameServerSetting.GameLogicSetting.AdditionalCostPerformanceFactor);
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
            Assert.Equal(1.0, setting.GameServerSetting.GameLogicSetting.AdditionalProductionSpeedFactor);
        }

        [Fact]
        public void RoundTrip_SerializeAndDeserialize_ShouldMaintainObjectState()
        {
            Setting originalSetting = new Setting();
            originalSetting.GameServerSetting.GameLogicSetting.AdditionalProductionSpeedFactor = 2.0;

            string json = Setting.Serialize(originalSetting);
            Setting deserializedSetting = Setting.Deserialize(json);

            Assert.Equal(2.0, deserializedSetting.GameServerSetting.GameLogicSetting.AdditionalProductionSpeedFactor);
            Assert.Equal(originalSetting.LogPath, deserializedSetting.LogPath);
        }
    }
}

using Xunit;

namespace Arrowgene.Ddon.GameServer.Tests
{
    public class RpcManagerTest
    {
        [Theory]
        [InlineData(0, 100, "Empty (0)")]
        [InlineData(1, 100, "Light (1)")]
        [InlineData(10, 100, "Light (10)")]
        [InlineData(19, 100, "Light (19)")]
        [InlineData(20, 100, "Good (20)")]
        [InlineData(30, 100, "Good (30)")]
        [InlineData(39, 100, "Good (39)")]
        [InlineData(40, 100, "Normal (40)")]
        [InlineData(50, 100, "Normal (50)")]
        [InlineData(59, 100, "Normal (59)")]
        [InlineData(60, 100, "Busy (60)")]
        [InlineData(70, 100, "Busy (70)")]
        [InlineData(79, 100, "Busy (79)")]
        [InlineData(80, 100, "Heavy (80)")]
        [InlineData(90, 100, "Heavy (90)")]
        [InlineData(99, 100, "Heavy (99)")]
        [InlineData(100, 100, "Full (100)")]
        [InlineData(110, 100, "Full (110)")]
        public void GetTrafficName_ShouldReturnCorrectTrafficLabel(uint count, uint maxLoginNum, string expected)
        {
            // Act
            string result = RpcManager.GetTrafficName(count, maxLoginNum);

            // Assert
            Assert.Equal(expected, result);
        }        
    }
}

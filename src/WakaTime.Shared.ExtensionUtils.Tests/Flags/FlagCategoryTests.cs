using JetBrains.Annotations;
using WakaTime.Shared.ExtensionUtils.Flags;
using Xunit;

namespace WakaTime.Shared.ExtensionUtils.Tests.Flags
{
    [TestSubject(typeof(FlagCategory))]
    public class FlagCategoryTests : FlagsTestBase
    {
        [Fact]
        public void AddFlagCategory_WithDefaultValue_AddsFlagToFlags()
        {
            var heartbeat = CreateHeartbeatWithoutFlags();

            // Test with default values
            heartbeat.AddFlagCategory();
            Assert.Contains(heartbeat.Flags, f => f.Key == "--category" && f.Value.GetValue() == "coding");
        }

        [Fact]
        public void AddFlagCategory_WithCustomValue_AddsFlagToFlags()
        {
            var heartbeat = CreateHeartbeatWithoutFlags();

            // Test with custom values
            heartbeat.AddFlagCategory(HeartbeatCategory.WritingTests);
            Assert.Contains(heartbeat.Flags, f => f.Key == "--category" && f.Value.GetValue() == "writing tests");
        }

        [Fact]
        public void AddFlagCategory_WithDefaultValue_CorrectlyFormattedForCli()
        {
            var heartbeat = CreateHeartbeatWithoutFlags();
            heartbeat.AddFlagCategory();
            var flag = heartbeat.GetFlag("--category");
            Assert.Equal("--category\" \"coding", flag.GetFormattedForCli());
        }

        [Fact]
        public void AddFlagCategory_WithDefaultValue_CorrectlyFormattedForJson()
        {
            var heartbeat = CreateHeartbeatWithoutFlags();
            heartbeat.AddFlagCategory();
            var flag = heartbeat.GetFlag("--category");
            Assert.Equal("\"category\": \"coding\"", flag.GetFormattedForJson());
        }
    }
}
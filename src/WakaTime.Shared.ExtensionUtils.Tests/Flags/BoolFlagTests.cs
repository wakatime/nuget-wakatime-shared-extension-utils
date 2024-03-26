using JetBrains.Annotations;
using WakaTime.Shared.ExtensionUtils.Extensions;
using WakaTime.Shared.ExtensionUtils.Flags;
using Xunit;

namespace WakaTime.Shared.ExtensionUtils.Tests.Flags
{
    [TestSubject(typeof(Flag<bool>))]
    public class BoolFlagTests
    {
        private readonly WakaTime _wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);

        [Fact]
        public void AddBoolFlag_SetTrue_CorrectlyFormattedForCli()
        {
            var hb = _wakaTime.CreateHeartbeat()
                              .AddFlagWrite()
                              .AddFlagVerbose();

            var write = hb.GetFlag(FlagWrite.Name.Cli);
            var verbose = hb.GetFlag(FlagVerbose.Name.Cli);
            
            Assert.Equal("--write", write.GetFormattedForCli());
            Assert.Equal("--verbose", verbose.GetFormattedForCli());
        }
        
        [Fact]
        public void AddBoolFlag_SetFalse_CorrectlyFormattedForCli()
        {
            var hb = _wakaTime.CreateHeartbeat()
                              .AddFlagWrite(false)
                              .AddFlagVerbose(false);

            var write = hb.GetFlag(FlagWrite.Name.Cli);
            var verbose = hb.GetFlag(FlagVerbose.Name.Cli);
            
            Assert.Equal(string.Empty, write.GetFormattedForCli());
            Assert.Equal(string.Empty, verbose.GetFormattedForCli());
        }

        [Fact]
        public void AddBoolFlag_SetTrue_CorrectlyFormattedStringValue()
        {
            var hb = _wakaTime.CreateHeartbeat()
                              .AddFlagWrite()
                              .AddFlagVerbose();
            
            var write = hb.GetFlag(FlagWrite.Name.Cli);
            var verbose = hb.GetFlag(FlagVerbose.Name.Cli);
            
            Assert.Equal("true", write.GetValue());
            Assert.Equal("true", verbose.GetValue());
            Assert.Equal("true", write.GetValue(true));
            Assert.Equal("true", verbose.GetValue(true));
        }
        
        [Fact]
        public void AddBoolFlag_SetFalse_CorrectlyFormattedStringValue()
        {
            var hb = _wakaTime.CreateHeartbeat()
                              .AddFlagWrite(false)
                              .AddFlagVerbose(false);
            
            var write = hb.GetFlag(FlagWrite.Name.Cli);
            var verbose = hb.GetFlag(FlagVerbose.Name.Cli);
            
            Assert.Equal("false", write.GetValue());
            Assert.Equal("false", verbose.GetValue());
            Assert.Equal("false", write.GetValue(true));
            Assert.Equal("false", verbose.GetValue(true));
        }
    }
}
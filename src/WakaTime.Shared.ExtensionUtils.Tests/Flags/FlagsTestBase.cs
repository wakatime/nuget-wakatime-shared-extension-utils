using WakaTime.Shared.ExtensionUtils.Extensions;
using Xunit;

namespace WakaTime.Shared.ExtensionUtils.Tests.Flags
{
    public class FlagsTestBase
    {
        #region Fields

        protected readonly WakaTime WakaTime;

        #endregion

        #region Constructors

        public FlagsTestBase()
        {
            WakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            WakaTime.CommonFlags.ClearFlags();
        }

        #endregion

        protected Heartbeat CreateHeartbeatWithoutFlags()
        {
            var hb = WakaTime.CreateHeartbeat();
            hb.ClearFlags();
            return hb;
        }

        [Fact]
        public void ClearFlags_RemovesAllFlags()
        {
            var hb = CreateHeartbeatWithoutFlags();
            Assert.Empty(hb.Flags);
        }
    }
}
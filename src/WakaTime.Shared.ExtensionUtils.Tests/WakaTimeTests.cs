using System;
using JetBrains.Annotations;
using WakaTime.Shared.ExtensionUtils.Exceptions;
using WakaTime.Shared.ExtensionUtils.Extensions;
using WakaTime.Shared.ExtensionUtils.Flags;
using Xunit;

namespace WakaTime.Shared.ExtensionUtils.Tests
{
    [TestSubject(typeof(WakaTime))]
    public class WakaTimeTests
    {
        [Fact]
        public void WhenNew_HasKeyFlag()
        {
            // Arrange
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);

            // Act
            var keyFlag = wakaTime.CommonFlags.GetFlag(FlagKey.CliFlagName);

            // Assert
            Assert.NotNull(keyFlag);
        }
        
        [Fact]
        public void WhenNew_HasPluginFlag()
        {
            // Arrange
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);

            // Act
            var projectFlag = wakaTime.CommonFlags.GetFlag(FlagPlugin.CliFlagName);

            // Assert
            Assert.NotNull(projectFlag);
        }
        
        [Fact]
        public void WhenNewAndFlagNotSet_HasNoProjectFlag()
        {
            // Arrange
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);

            // Act
            var projectFlag = wakaTime.CommonFlags.GetFlag(FlagProject.CliFlagName);

            // Assert
            Assert.Null(projectFlag);
        }
        
        [Fact]
        public void WhenHeartbeatsCreatedAndFlagMissing_ThrowsException()
        {
            // Arrange
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);

            // Act
            // Assert
            Assert.Throws<AggregateException>(() => wakaTime.CreateHeartbeat().AddFlagLanguage("TestLanguage").Send(true));
        }
        
        [Fact]
        public void WhenHeartbeatsCreated_HasItemsInQueue()
        {
            // Arrange
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);

            // Act
            wakaTime.CreateHeartbeat().AddFlagEntity("TestEntity").AddFlagLanguage("TestLanguage").AddFlagProject("TestProject").Send(true);
            wakaTime.CreateHeartbeat().AddFlagEntity("TestEntity").AddFlagLanguage("TestLanguage").AddFlagProject("TestProject").Send(true);
            wakaTime.CreateHeartbeat().AddFlagEntity("TestEntity").AddFlagLanguage("TestLanguage").AddFlagProject("TestProject").Send(true);

            // Assert
            Assert.Equal(3, wakaTime.HeartbeatQueue.Count);
        }
    }
}
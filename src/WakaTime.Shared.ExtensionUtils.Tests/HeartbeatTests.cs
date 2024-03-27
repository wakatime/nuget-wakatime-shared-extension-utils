using System;
using JetBrains.Annotations;
using WakaTime.Shared.ExtensionUtils;
using WakaTime.Shared.ExtensionUtils.Exceptions;
using WakaTime.Shared.ExtensionUtils.Extensions;
using WakaTime.Shared.ExtensionUtils.Flags;
using Xunit;

namespace WakaTime.Shared.ExtensionUtils.Tests
{
    [TestSubject(typeof(Heartbeat))]
    public class HeartbeatTests
    {
        [Fact]
        public void Ctor_Always_ShouldCreateHeartbeat()
        {
            // Use valid WakaTime instance

            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            var heartbeat = wakaTime.CreateHeartbeat();

            Assert.NotNull(heartbeat);
        }

        [Theory]
        [InlineData(true,  false)] // Heartbeat is invalid, ThrowException is true
        [InlineData(true,  true)] // Heartbeat is valid, ThrowException is true
        [InlineData(false, false)] // Heartbeat is invalid, ThrowException is false
        [InlineData(false, true)] // Heartbeat is valid, ThrowException is false
        public void IsValid_Always_ShouldValidateHeartbeat(bool isHeartbeatValid, bool throwException)
        {
            // Arrange
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            var heartbeat = wakaTime.CreateHeartbeat();

            if (isHeartbeatValid)
            {
                // most of the required flags are set in constructor
                heartbeat.AddFlagEntity("file");
            }
            else
            {
                // Set only a partial list of flags here to ensure the invalidation. E.g.,
                heartbeat.AddFlagKey("key"); // override the default key, but don't set the entity
            }

            // Act
            bool isValid;

            try
            {
                isValid = heartbeat.IsValid(throwException);
            }
            catch (AggregateException)
            {
                isValid = false; // In case when the exception is expected
            }

            // Assert

            Assert.Equal(isHeartbeatValid, isValid);
        }
        
        [Fact]
        public void IsValid_WithMissingFlags_ShouldThrowException()
        {
            // Arrange
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            var heartbeat = wakaTime.CreateHeartbeat();

            // Assert
            Assert.Throws<AggregateException>(Act);
            return;

            // Act
            void Act() => heartbeat.IsValid(true);
        } 
        
        [Fact]
        public void IsValid_WithMissingFlags_ShouldThrowExceptionWithMissingFlags()
        {
            // Arrange
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            var heartbeat = wakaTime.CreateHeartbeat();

            // Assert
            var ex = Assert.Throws<AggregateException>(Act);
            Assert.NotEmpty(ex.InnerExceptions);
            Assert.All(ex.InnerExceptions, e => Assert.IsType<MissingFlagException>(e));
            return;

            // Act
            void Act() => heartbeat.IsValid(true);
        }
    }
}
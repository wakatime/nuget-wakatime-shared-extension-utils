using JetBrains.Annotations;
using System;
using Xunit;
using WakaTime.Shared.ExtensionUtils;
using WakaTime.Shared.ExtensionUtils.Extensions;
using WakaTime.Shared.ExtensionUtils.Flags;

namespace WakaTime.Shared.ExtensionUtils.Tests.Extensions
{
    [TestSubject(typeof(WakaTimeExtension))]
    public class WakaTimeExtensionTests
    {
        private readonly WakaTime _wakaTimeInstance;
        private const string TestFileName = "test.txt";
        private const string AlternateProjectName = "AlternateProject";
        private HeartbeatCategory _heartbeatCategory;
        private readonly string _heartbeatCategoryText;
        private EntityType _entityType;
        private readonly string _entityTypeText;

        public WakaTimeExtensionTests()
        {
            this._wakaTimeInstance = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            
            this._heartbeatCategory = HeartbeatCategory.Debugging;
            this._heartbeatCategoryText = Enum.GetName(typeof(HeartbeatCategory), _heartbeatCategory)?.ToLower();
            
            this._entityType = EntityType.App;
            this._entityTypeText = Enum.GetName(typeof(EntityType), _entityType)?.ToLower();
        }

        [Fact]
        public void CreateHeartbeat_ShouldSetBasicFlags()
        {
            var result = _wakaTimeInstance.CreateHeartbeat();
            
            Assert.NotNull(result);
            Assert.NotNull(result.GetFlag(FlagKey.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagPlugin.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagCategory.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagEntityType.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagTime.Name.Cli));
        }

        [Fact]
        public void CreateHeartbeat_ShouldSetFileName()
        {
            var result = _wakaTimeInstance.CreateHeartbeat(TestFileName);
            
            Assert.NotNull(result);
            Assert.NotNull(result.GetFlag(FlagKey.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagPlugin.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagCategory.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagEntityType.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagTime.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagEntity.Name.Cli));
            Assert.Equal(TestFileName, result.GetFlag(FlagEntity.Name.Cli)
                                              .GetValue());
        }

        [Fact]
        public void CreateHeartbeat_ShouldSetIsWrite()
        {
            var result = _wakaTimeInstance.CreateHeartbeat(TestFileName, true);
            
            Assert.NotNull(result);
            Assert.NotNull(result.GetFlag(FlagKey.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagPlugin.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagCategory.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagEntityType.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagTime.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagEntity.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagWrite.Name.Cli));
            
            Assert.Equal(TestFileName, result.GetFlag(FlagEntity.Name.Cli)
                                              .GetValue());
            Assert.True(result.GetFlag(FlagWrite.Name.Cli).GetValue() == "true");
        }

        [Fact]
        public void CreateHeartbeat_AlternateProjectSet_ShouldSetAlternateProject()
        {
            var result = _wakaTimeInstance.CreateHeartbeat(TestFileName, true, AlternateProjectName);
            
            Assert.NotNull(result);
            Assert.NotNull(result.GetFlag(FlagKey.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagPlugin.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagCategory.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagEntityType.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagTime.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagEntity.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagWrite.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagProjectAlternate.Name.Cli));
            Assert.Equal(TestFileName, result.GetFlag(FlagEntity.Name.Cli)
                                              .GetValue());
            Assert.True(result.GetFlag(FlagWrite.Name.Cli).GetValue() == "true");
            Assert.Equal(AlternateProjectName, result.GetFlag(FlagProjectAlternate.Name.Cli)
                                                     .GetValue());
        }
        
        [Fact]
        public void CreateHeartbeat_HeartbeatCategorySet_ShouldOverrideHeartbeatCategory()
        {
            var result = _wakaTimeInstance.CreateHeartbeat(TestFileName, true, AlternateProjectName, _heartbeatCategory, _entityType);

            Assert.NotNull(result);
            Assert.NotNull(result.GetFlag(FlagKey.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagPlugin.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagCategory.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagEntityType.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagTime.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagEntity.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagWrite.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagProjectAlternate.Name.Cli));
            Assert.Equal(TestFileName, result.GetFlag(FlagEntity.Name.Cli)
                                             .GetValue());
            Assert.True(result.GetFlag(FlagWrite.Name.Cli).GetValue() == "true");
            Assert.Equal(AlternateProjectName, result.GetFlag(FlagProjectAlternate.Name.Cli)
                                                     .GetValue());
            Assert.Equal(_heartbeatCategoryText, result.GetFlag(FlagCategory.Name.Cli)
                                                        .GetValue());
        }

        [Fact]
        public void CreateHeartbeat_EntityTypeSet_ShouldOverrideEntityType()
        {
            var result = _wakaTimeInstance.CreateHeartbeat(TestFileName, true, AlternateProjectName, _heartbeatCategory, _entityType);

            Assert.NotNull(result);
            Assert.NotNull(result.GetFlag(FlagKey.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagPlugin.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagCategory.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagEntityType.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagTime.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagEntity.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagWrite.Name.Cli));
            Assert.NotNull(result.GetFlag(FlagProjectAlternate.Name.Cli));
            Assert.Equal(TestFileName, result.GetFlag(FlagEntity.Name.Cli)
                                             .GetValue());
            Assert.True(result.GetFlag(FlagWrite.Name.Cli).GetValue() == "true");
            Assert.Equal(AlternateProjectName, result.GetFlag(FlagProjectAlternate.Name.Cli)
                                                     .GetValue());
            Assert.Equal(_heartbeatCategoryText, result.GetFlag(FlagCategory.Name.Cli)
                                                       .GetValue());
            Assert.Equal(_entityTypeText, result.GetFlag(FlagEntityType.Name.Cli)
                                                .GetValue());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using WakaTime.Shared.ExtensionUtils.Extensions;
using WakaTime.Shared.ExtensionUtils.Flags;
using WakaTime.Shared.ExtensionUtils.Helpers;
using Xunit;

namespace WakaTime.Shared.ExtensionUtils.Tests.Helpers
{
    [TestSubject(typeof(JsonSerializerHelper))]
    public class JsonSerializerHelperTests
    {
        #region Static Fields and Const

        private const EntityType EntityType01 = EntityType.File;
        private const EntityType EntityType02 = EntityType.Domain;

        private const HeartbeatCategory Category01 = HeartbeatCategory.Coding;
        private const HeartbeatCategory Category02 = HeartbeatCategory.Debugging;
        private const string AlternateLanguage = "unit-test-language-alternate";

        private const string Apikey = "307511FC-5CC8-4CDE-B900-1E81ECF9EA48";

        private const string Entity01 = "unit-test-entity-01";
        private const string Entity02 = "unit-test-entity-02";
        private const string Language = "unit-test-language";
        private const string Plugin = "unit-test-plugin";
        private const string Project = "unit-test-project";
        private const string ProjectAlternate = "unit-test-project-alternate";

        #endregion

        #region Fields

        private readonly DateTime _time01 = new DateTime(2021, 1, 1, 1, 1, 1); // unix timestamp: 1609462861.000000
        private readonly DateTime _time02 = new DateTime(2024, 1, 1, 1, 1, 2); // unix timestamp: 1704070862.000000
        private readonly WakaTime _wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);

        #endregion

        #region Constructors

        public JsonSerializerHelperTests()
        {
            _wakaTime.CommonFlags.AddFlagKey(Apikey)
                     .AddFlagPlugin(Plugin)
                     .AddFlagProject(Project)
                     .AddFlagProjectAlternate(ProjectAlternate)
                     .AddFlagLanguage(Language)
                     .AddFlagLanguageAlternate(AlternateLanguage);

            _wakaTime.CreateHeartbeat(Entity01)
                     .AddFlagCategory()
                     .AddFlagEntityType()
                     .AddFlagTime(_time01)
                     .Send();

            _wakaTime.CreateHeartbeat(Entity02)
                     .AddFlagCategory(Category02)
                     .AddFlagEntityType(EntityType02)
                     .AddFlagTime(_time02)
                     .Send();
        }

        #endregion

        [Fact]
        public void TestTo_SingleHeartbeat_CorrectlySerialized()
        {
            var flag = _wakaTime.HeartbeatQueue.First();
            string json = JsonSerializerHelper.ToJson(flag);

            Assert.NotNull(flag);
            Assert.Equal(_json01, json);
        }

        [Fact]
        public void ToJson_MultipleHeartbeats_CorrectlySerialized()
        {
            string json = JsonSerializerHelper.ToJson(_wakaTime.HeartbeatQueue);

            Assert.NotNull(json);
            Assert.Equal($"[{_json01},{_json02}]", json);
        }

        [Fact]
        public void ToJson_NoHeartbeats_EmptyArray()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var flagHolders = new List<FlagHolder>();
            string result = JsonSerializerHelper.ToJson(flagHolders);
            Assert.Equal("[]", result);
        }

        [Fact]
        public void ToJson_NullHeartbeatList_EmptyArray()
        {
            List<FlagHolder> flagHolders = null;
            // ReSharper disable once ExpressionIsAlwaysNull
            string result = JsonSerializerHelper.ToJson(flagHolders);
            Assert.Equal("[]", result);
        }

        [Fact]
        public void ToJson_SingleHeartbeat_SerialisedWithoutFlagsForExtraHeartbeats()
        {
            var wakatime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            // add all flags to the common flags
            wakatime.CommonFlags.AddFlagCategory(HeartbeatCategory.WritingTests)
                    .AddFlagEntity("jsonFlagNamesNotInExtraHeartbeat")
                    .AddFlagEntityType(EntityType.App)
                    .AddFlagKey(Apikey)
                    .AddFlagLanguage(Language)
                    .AddFlagLanguageAlternate(AlternateLanguage)
                    .AddFlagPlugin(Plugin)
                    .AddFlagProject(Project)
                    .AddFlagProjectAlternate(ProjectAlternate)
                    .AddFlagTime(DateTime.UtcNow)
                    .AddFlagVerbose()
                    .AddFlagWrite();
            
            var hb = wakatime.CreateHeartbeat(Entity01);
            string json = JsonSerializerHelper.ToJson(hb, true);
            
            var flagCategory = hb.GetFlag(FlagCategory.Name.Cli);
            var flagEntity = hb.GetFlag(FlagEntity.Name.Cli);
            var flagEntityType = hb.GetFlag(FlagEntityType.Name.Cli);
            var flagKey = hb.GetFlag(FlagKey.Name.Cli);
            var flagLanguage = hb.GetFlag(FlagLanguage.Name.Cli);
            var flagLanguageAlternate = hb.GetFlag(FlagLanguageAlternate.Name.Cli);
            var flagPlugin = hb.GetFlag(FlagPlugin.Name.Cli);
            var flagProject = hb.GetFlag(FlagProject.Name.Cli);
            var flagProjectAlternate = hb.GetFlag(FlagProjectAlternate.Name.Cli);
            var flagTime = hb.GetFlag(FlagTime.Name.Cli);
            var flagVerbose = hb.GetFlag(FlagVerbose.Name.Cli);
            var flagWrite = hb.GetFlag(FlagWrite.Name.Cli);
            
            Assert.NotNull(hb);
            Assert.Equal(flagCategory.ForExtraHeartbeat, json.Contains(flagCategory.Names.Json));
            Assert.Contains(flagCategory.Names.Json, json);
            
            Assert.Equal(flagEntity.ForExtraHeartbeat, json.Contains(flagEntity.Names.Json));
            Assert.Contains(flagEntity.Names.Json, json);
            
            Assert.Equal(flagEntityType.ForExtraHeartbeat, json.Contains(flagEntityType.Names.Json));
            Assert.Contains(flagEntityType.Names.Json, json);
            
            Assert.Equal(flagKey.ForExtraHeartbeat, json.Contains(flagKey.Names.Json));
            Assert.DoesNotContain(flagKey.Names.Json, json);
            
            Assert.Equal(flagLanguage.ForExtraHeartbeat, json.Contains(flagLanguage.Names.Json));
            Assert.Contains(flagLanguage.Names.Json, json);
            
            Assert.Equal(flagLanguageAlternate.ForExtraHeartbeat, json.Contains(flagLanguageAlternate.Names.Json));
            Assert.Contains(flagLanguageAlternate.Names.Json, json);
            
            Assert.Equal(flagPlugin.ForExtraHeartbeat, json.Contains(flagPlugin.Names.Json));
            Assert.DoesNotContain(flagPlugin.Names.Json, json);
            
            Assert.Equal(flagProject.ForExtraHeartbeat, json.Contains(flagProject.Names.Json));
            Assert.Contains(flagProject.Names.Json, json);
            
            Assert.Equal(flagProjectAlternate.ForExtraHeartbeat, json.Contains(flagProjectAlternate.Names.Json));
            Assert.Contains(flagProjectAlternate.Names.Json, json);
            
            Assert.Equal(flagTime.ForExtraHeartbeat, json.Contains(flagTime.Names.Json));
            Assert.Contains(flagTime.Names.Json, json);
            
            Assert.Equal(flagVerbose.ForExtraHeartbeat, json.Contains(flagVerbose.Names.Json));
            Assert.DoesNotContain(flagVerbose.Names.Json, json);
            
            Assert.Equal(flagWrite.ForExtraHeartbeat, json.Contains(flagWrite.Names.Json));
            Assert.DoesNotContain(flagWrite.Names.Json, json);
        }


        [Fact]
        public void JsonEscape_WithEmptyString_CorrectlyEscaped()
        {
            string testString = string.Empty;
            string result = JsonSerializerHelper.JsonEscape(testString);
            Assert.Equal(string.Empty, result);
        }


        [Theory]
        [InlineData("test string",  "test string")]
        [InlineData("test\\string", "test\\\\string")]
        [InlineData("test\"string", "test\\\"string")]
        [InlineData("test\bstring", "test\\bstring")]
        [InlineData("test\fstring", "test\\fstring")]
        [InlineData("test\nstring", "test\\nstring")]
        [InlineData("test\rstring", "test\\rstring")]
        [InlineData("test\tstring", "test\\tstring")]
        [InlineData(null,           null)]
        [InlineData("",             "")]
        public void JsonEscape_StringRequiresEscaping_CorrectlyEscaped(string testString, string expected)
        {
            string result = JsonSerializerHelper.JsonEscape(testString);
            Assert.Equal(expected, result);
        }
        
        // @formatter:off
        private readonly string _json01 = "{"+
                                          $"\"{FlagKey.Name.Json}\": \"{Apikey}\","+
                                          $"\"{FlagPlugin.Name.Json}\": \"{Plugin}\","+
                                          $"\"{FlagProject.Name.Json}\": \"{Project}\","+
                                          $"\"{FlagProjectAlternate.Name.Json}\": \"{ProjectAlternate}\","+
                                          $"\"{FlagLanguage.Name.Json}\": \"{Language}\","+
                                          $"\"{FlagLanguageAlternate.Name.Json}\": \"{AlternateLanguage}\","+
                                          $"\"{FlagCategory.Name.Json}\": \"{Category01.GetDescription()}\","+
                                          $"\"{FlagEntityType.Name.Json}\": \"{EntityType01.GetDescription()}\","+
                                          $"\"{FlagTime.Name.Json}\": \"1609462861.000000\","+
                                          $"\"{FlagEntity.Name.Json}\": \"{Entity01}\""+
                                          "}";
        
        private readonly string _json02 = "{"+
                                          $"\"{FlagKey.Name.Json}\": \"{Apikey}\","+
                                          $"\"{FlagPlugin.Name.Json}\": \"{Plugin}\","+
                                          $"\"{FlagProject.Name.Json}\": \"{Project}\","+
                                          $"\"{FlagProjectAlternate.Name.Json}\": \"{ProjectAlternate}\","+
                                          $"\"{FlagLanguage.Name.Json}\": \"{Language}\","+
                                          $"\"{FlagLanguageAlternate.Name.Json}\": \"{AlternateLanguage}\","+
                                          $"\"{FlagCategory.Name.Json}\": \"{Category02.GetDescription()}\","+
                                          $"\"{FlagEntityType.Name.Json}\": \"{EntityType02.GetDescription()}\","+
                                          $"\"{FlagTime.Name.Json}\": \"1704070862.000000\","+
                                          $"\"{FlagEntity.Name.Json}\": \"{Entity02}\""+
                                          "}";
        // @formatter:on
    }
}
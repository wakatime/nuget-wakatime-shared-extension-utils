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
using System;
using JetBrains.Annotations;
using WakaTime.Shared.ExtensionUtils.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WakaTime.Shared.ExtensionUtils.Extensions;
using WakaTime.Shared.ExtensionUtils.Flags;
using Xunit;

namespace WakaTime.Shared.ExtensionUtils.Tests.Helpers
{
    [TestSubject(typeof(JsonSerializerHelper))]
    public class JsonSerializerHelperTests
    {
        private readonly WakaTime _wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);

        private const string Apikey = "307511FC-5CC8-4CDE-B900-1E81ECF9EA48";
        private const string Plugin = "unit-test-plugin";
        private const string Project = "unit-test-project";
        private const string ProjectAlternate = "unit-test-project-alternate";
        private const string Language = "unit-test-language";
        private const string AlternateLanguage = "unit-test-language-alternate";

        private const HeartbeatCategory Category01 = HeartbeatCategory.Coding;
        private const HeartbeatCategory Category02 = HeartbeatCategory.Debugging;

        private const string Entity01 = "unit-test-entity-01";
        private const string Entity02 = "unit-test-entity-02";
        
        private const EntityType EntityType01 = EntityType.File;
        private const EntityType EntityType02 = EntityType.Domain;
        
        private readonly DateTime _time01 = new DateTime(2021, 1, 1, 1, 1, 1); // unix timestamp: 1609462861.000000
        private readonly DateTime _time02 = new DateTime(2024, 1, 1, 1, 1, 2); // unix timestamp: 1704070862.000000
        
        // @formatter:off
        private readonly string _json01 = $"{{"+
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
                                          $"}}";
        // @formatter:on

        public JsonSerializerHelperTests()
        {
            _wakaTime.CommonFlags.AddFlagKey(Apikey)
                     .AddFlagPlugin(Plugin)
                     .AddFlagProject(Project)
                     .AddFlagProjectAlternate(ProjectAlternate)
                     .AddFlagLanguage(Language)
                     .AddFlagLanguageAlternate(AlternateLanguage);

            _wakaTime.CreateHeartbeat(Entity01)
                     .AddFlagCategory(Category01).AddFlagEntityType(EntityType01).AddFlagTime(_time01).Send();
            
            _wakaTime.CreateHeartbeat(Entity02)
                     .AddFlagCategory(Category02).AddFlagEntityType(EntityType02).AddFlagTime(_time02).Send();
        }
        
        [Fact]
        public void TestTo_SingleHeartbeat_CorrectlySerialized()
        {
            var flag = _wakaTime.HeartbeatQueue.First();
            string json = JsonSerializerHelper.ToJson(flag);
            
            Assert.NotNull(flag);
            Assert.Equal(_json01, json);
        }

        // [Fact]
        // public void TestToJsonWithDataObfuscation()
        // {
        //     var flagHolders = new List<FlagHolder>
        //     {
        //         new FlagHolder
        //         {
        //             /* populate this object */
        //         }
        //     };
        //     var result = JsonSerializerHelper.ToJson(flagHolders, false, true);
        //     // Use Assert function to check if the result is as expected.
        // }

        // [Fact]
        // public void TestToJsonWithNoExtraHeartbeat()
        // {
        //     var flagHolders = new List<FlagHolder>
        //     {
        //         new FlagHolder
        //         {
        //             /* populate this object */
        //         }
        //     };
        //     var result = JsonSerializerHelper.ToJson(flagHolders, false);
        //     // Use Assert function to check if the result is as expected.
        // }

        [Fact]
        public void TestToJsonWithEmtpyHeartbeatList()
        {
            var flagHolders = new List<FlagHolder>();
            var result = JsonSerializerHelper.ToJson(flagHolders);
            // Use Assert function to check if the result is as expected.
        }

        [Fact]
        public void TestToJsonWithNullHeartbeatList()
        {
            List<FlagHolder> flagHolders = null;
            var result = JsonSerializerHelper.ToJson(flagHolders);
            // Use Assert function to check if the result is as expected.
        }


        [Fact]
        public void TestJsonEscapeWithEmptyString()
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
        public void JsonEscape_StringRequiresEscaping_CorrectlyEscaped(string testString,string expected)
        {
            string result = JsonSerializerHelper.JsonEscape(testString);
            Assert.Equal(expected, result);
        }
    }
}
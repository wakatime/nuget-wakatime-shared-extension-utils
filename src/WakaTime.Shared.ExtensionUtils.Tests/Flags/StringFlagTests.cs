using System;
using JetBrains.Annotations;
using WakaTime.Shared.ExtensionUtils.Extensions;
using WakaTime.Shared.ExtensionUtils.Flags;
using Xunit;

namespace WakaTime.Shared.ExtensionUtils.Tests.Flags
{
    [TestSubject(typeof(Flag<string>))]
    public class StringFlagTests
    {
        private readonly WakaTime _wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
        private const string ProjectName = "MyTestProject";
        private const string AlternateProjectName = "MyAlternateProject";
        private const string ApiKey = "92DEF62E-54DC-4BFF-A67A-F2A2C1EA083D";
        private const string ApiKeyObfuscated = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXX083D";

        private readonly string _cliProjectSetExpected = $"{FlagProject.Name.Cli}\" \"{ProjectName}"; // --project" "MyTestProject
        private readonly string _cliAlternateProjectSetExpected = $"{FlagProjectAlternate.Name.Cli}\" \"{AlternateProjectName}"; // --alternate-project" "MyAlternateProject
        
        private readonly string _jsonProjectSetExpected = $"\"{FlagProject.Name.Json}\": \"{ProjectName}\""; // "project": "MyTestProject"
        private readonly string _jsonAlternateProjectSetExpected = $"\"{FlagProjectAlternate.Name.Json}\": \"{AlternateProjectName}\""; // "alternate_project": "MyAlternateProject"
        
        private readonly string _cliApiKeyObfuscatedExpected = $"{FlagKey.Name.Cli}\" \"{ApiKeyObfuscated}"; // --key" "XXXXXXXX-XXXX
        
        [Fact]
        public void AddStringFlag_SetValue_CorrectlyFormattedForCli()
        {
            var hb = _wakaTime.CreateHeartbeat().AddFlagProject(ProjectName)
                              .AddFlagProjectAlternate(AlternateProjectName);
            
            var projectFlag = hb.GetFlag(FlagProject.Name.Cli);
            var alternateProjectFlag = hb.GetFlag(FlagProjectAlternate.Name.Cli);
            
            Assert.Equal(_cliProjectSetExpected, projectFlag.GetFormattedForCli());
            Assert.Equal(_cliAlternateProjectSetExpected, alternateProjectFlag.GetFormattedForCli());
        }
        
        [Fact]
        public void AddStringFlag_SetValueObfuscate_CorrectlyFormattedForCli()
        {
            var hb = _wakaTime.CreateHeartbeat()
                              .AddFlagKey(ApiKey);
            
            var keyFlag = hb.GetFlag(FlagKey.Name.Cli);
            
            Assert.Equal(_cliApiKeyObfuscatedExpected, keyFlag.GetFormattedForCli(true));
        }
        
        [Fact]
        public void AddStringFlag_NoValue_CorrectlyFormattedForCli()
        {
            var hb = _wakaTime.CreateHeartbeat().AddFlagProject(string.Empty)
                              .AddFlagProjectAlternate(string.Empty);
            
            var projectFlag = hb.GetFlag(FlagProject.Name.Cli);
            var alternateProjectFlag = hb.GetFlag(FlagProjectAlternate.Name.Cli);
            
            Assert.Equal(string.Empty, projectFlag.GetFormattedForCli());
            Assert.Equal(string.Empty, alternateProjectFlag.GetFormattedForCli());
        }
        
        [Fact]
        public void AddStringFlag_SetValue_CorrectlyFormattedForJson()
        {
            var hb = _wakaTime.CreateHeartbeat().AddFlagProject(ProjectName)
                              .AddFlagProjectAlternate(AlternateProjectName);
            
            var projectFlag = hb.GetFlag(FlagProject.Name.Cli);
            var alternateProjectFlag = hb.GetFlag(FlagProjectAlternate.Name.Cli);
            
            Assert.Equal(_jsonProjectSetExpected,          projectFlag.GetFormattedForJson());
            Assert.Equal(_jsonAlternateProjectSetExpected, alternateProjectFlag.GetFormattedForJson());
        }
        
        [Fact]
        public void AddStringFlag_NoValue_CorrectlyFormattedForJson()
        {
            var hb = _wakaTime.CreateHeartbeat().AddFlagProject(string.Empty)
                              .AddFlagProjectAlternate(string.Empty);
            
            var projectFlag = hb.GetFlag(FlagProject.Name.Cli);
            var alternateProjectFlag = hb.GetFlag(FlagProjectAlternate.Name.Cli);
            
            Assert.Equal(string.Empty,          projectFlag.GetFormattedForJson());
            Assert.Equal(string.Empty, alternateProjectFlag.GetFormattedForJson());
        }
        
        [Fact]
        public void AddStringFlag_SetValue_CorrectlyFormattedStringValue()
        {
            var hb = _wakaTime.CreateHeartbeat().AddFlagProject(ProjectName)
                              .AddFlagProjectAlternate(AlternateProjectName);
            
            var projectFlag = hb.GetFlag(FlagProject.Name.Cli);
            var alternateProjectFlag = hb.GetFlag(FlagProjectAlternate.Name.Cli);
            
            Assert.Equal(ProjectName, projectFlag.GetValue());
            Assert.Equal(AlternateProjectName, alternateProjectFlag.GetValue());
        }
        
        [Fact]
        public void AddStringFlag_NoValue_CorrectlyFormattedStringValue()
        {
            var hb = _wakaTime.CreateHeartbeat().AddFlagProject(string.Empty)
                              .AddFlagProjectAlternate(string.Empty);
            
            var projectFlag = hb.GetFlag(FlagProject.Name.Cli);
            var alternateProjectFlag = hb.GetFlag(FlagProjectAlternate.Name.Cli);
            
            Assert.Equal(string.Empty, projectFlag.GetFormattedForJson());
            Assert.Equal(string.Empty, alternateProjectFlag.GetFormattedForJson());
        }
    }
}
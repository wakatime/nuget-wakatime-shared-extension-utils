using System;
using System.Collections.Generic;
using WakaTime.Shared.ExtensionUtils;
using WakaTime.Shared.ExtensionUtils.Flags;
using Xunit;

namespace WakaTime.Shared.ExtensionUtils.Tests
{
    public class FlagHolderTests
    {
        [Fact]
        public void Ctor_Always_ShouldCreateFlagHolder()
        {
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            Assert.NotNull(wakaTime.CommonFlags);
        }

        [Fact]
        public void Ctor_Always_ShouldAddRequiredFlags()
        {
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            
            var keyFlag = wakaTime.CommonFlags.GetFlag(FlagKey.Name.Cli);
            var pluginFlag = wakaTime.CommonFlags.GetFlag(FlagPlugin.Name.Cli);
            
            Assert.NotNull(keyFlag);
            Assert.NotNull(pluginFlag);
        }

        [Fact]
        public void AddFlag_AddsFlag()
        {
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            
            wakaTime.CommonFlags.AddFlagLanguage("csharp");
            var languageFlag = wakaTime.CommonFlags.GetFlag(FlagLanguage.Name.Cli);
            
            Assert.NotNull(languageFlag);
        }

        [Fact]
        public void AddFlag_OverwriteFalse_DoesNotOverwrite()
        {
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            
            wakaTime.CommonFlags.AddFlagLanguage("langOrg");
            wakaTime.CommonFlags.AddFlagLanguage("langNew", false);
            
            var lanFlag = wakaTime.CommonFlags.GetFlag(FlagLanguage.Name.Cli);

            Assert.Equal("langOrg", lanFlag.GetValue());
        }

        [Fact]
        public void AddFlag_OverwriteTrue_OverwritesFlag()
        {
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            
            wakaTime.CommonFlags.AddFlagLanguage("langOrg");
            wakaTime.CommonFlags.AddFlagLanguage("langNew", true);
            
            var lanFlag = wakaTime.CommonFlags.GetFlag(FlagLanguage.Name.Cli);

            Assert.Equal("langNew", lanFlag.GetValue());
        }

        [Fact]
        public void RemoveFlag_Name_RemoveExistingFlag()
        {
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            
            wakaTime.CommonFlags.AddFlagLanguage("langOrg");
            Assert.NotNull(wakaTime.CommonFlags.GetFlag(FlagLanguage.Name.Cli));
            
            wakaTime.CommonFlags.RemoveFlag(FlagLanguage.Name.Cli);
            Assert.Null(wakaTime.CommonFlags.GetFlag(FlagLanguage.Name.Cli));
        }

        [Fact]
        public void RemoveFlags_Name_RemoveExistingFlags()
        {
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            
            wakaTime.CommonFlags.AddFlagLanguage("langOrg");
            wakaTime.CommonFlags.AddFlagProject("projOrg");
            Assert.NotNull(wakaTime.CommonFlags.GetFlag(FlagLanguage.Name.Cli));
            Assert.NotNull(wakaTime.CommonFlags.GetFlag(FlagProject.Name.Cli));
            
            wakaTime.CommonFlags.RemoveFlags(new List<string> { FlagLanguage.Name.Cli, FlagProject.Name.Cli });
            Assert.Null(wakaTime.CommonFlags.GetFlag(FlagLanguage.Name.Cli));
            Assert.Null(wakaTime.CommonFlags.GetFlag(FlagProject.Name.Cli));
        }

        [Fact]
        public void ClearFlags_RemovesAllFlags()
        {
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            
            wakaTime.CommonFlags.AddFlagLanguage("langOrg");
            wakaTime.CommonFlags.AddFlagProject("projOrg");
            Assert.NotEmpty(wakaTime.CommonFlags.Flags);
            
            wakaTime.CommonFlags.ClearFlags();
            Assert.Empty(wakaTime.CommonFlags.Flags);
        }

        [Fact]
        public void HasFlag_ReturnsTrue_WhenFlagExists()
        {
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            
            wakaTime.CommonFlags.AddFlagLanguage("langOrg");
            Assert.True(wakaTime.CommonFlags.HasFlag(FlagLanguage.Name.Cli));
        }

        [Fact]
        public void GetFlag_ReturnsFlag_IfItExists()
        {
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            
            wakaTime.CommonFlags.AddFlagLanguage("langOrg");
            var flag = wakaTime.CommonFlags.GetFlag(FlagLanguage.Name.Cli);
            
            Assert.NotNull(flag);
        }

        [Fact]
        public void GetFlag_ReturnsNull_IfFlagDoesNotExist()
        {
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            
            var flag = wakaTime.CommonFlags.GetFlag(FlagLanguage.Name.Cli);
            
            Assert.Null(flag);
        }

        [Fact]
        public void ToCliArgsArray_ReturnsArrayOfArguments()
        {
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            
            wakaTime.CommonFlags.AddFlagLanguage("langOrg");
            wakaTime.CommonFlags.AddFlagProject("projOrg");
            string[] args = wakaTime.CommonFlags.ToCliArgsArray();
            
            Assert.Contains("--language\" \"langOrg", args);
            Assert.Contains("--project\" \"projOrg", args);
        }
        
        [Fact]
        public void ToCliArgsArray_ReturnsEmptyArray_WhenNoFlags()
        {
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            
            wakaTime.CommonFlags.ClearFlags();
            string[] args = wakaTime.CommonFlags.ToCliArgsArray();
            
            Assert.Empty(args);
        }
        
        [Fact]
        public void ToJson_ReturnsJsonString()
        {
            var wakaTime = new WakaTime(Globals.Metadata, Globals.LoggerStub);
            
            wakaTime.CommonFlags.AddFlagLanguage("langOrg");
            wakaTime.CommonFlags.AddFlagProject("projOrg");
            string json = wakaTime.CommonFlags.ToJson();
            
            Assert.Contains("\"language\": \"langOrg", json);
            Assert.Contains("\"project\": \"projOrg", json);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakaTime.Shared.ExtensionUtils
{
    public enum HeartbeatCategory
    {
        [Description("coding")]
        Coding,

        [Description("building")]
        Building,

        [Description("indexing")]
        Indexing,

        [Description("debugging")]
        Debugging,

        [Description("running_tests")]
        RunningTests,

        [Description("writing_tests")]
        WritingTests,

        [Description("manual_testing")]
        ManualTesting,

        [Description("code_reviewing")]
        CodeReviewing,

        [Description("browsing")]
        Browsing,

        [Description("designing")]
        Designing
    }

    public enum EntityType
    {
        [Description("file")]
        File,

        [Description("domain")]
        Domain,

        [Description("app")]
        App
    }
}

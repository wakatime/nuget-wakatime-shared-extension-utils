using System.Runtime.Serialization;

namespace WakaTime.Shared.ExtensionUtils
{
    [DataContract]
    public class GithubReleaseApiResponse
    {
        [DataMember(Name = "tag_name")]
        public string TagName { get; set; }
    }
}

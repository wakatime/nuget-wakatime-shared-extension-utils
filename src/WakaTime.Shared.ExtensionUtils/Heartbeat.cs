namespace WakaTime.Shared.ExtensionUtils
{
    public class Heartbeat
    {
        public string Entity { get; set; }
        public string Timestamp { get; set; }
        public string Project { get; set; }
        public bool IsWrite { get; set; }
        public HeartbeatCategory Category { get; set; }
        public EntityType EntityType { get; set; }
    }

    /* Matheus Rocha (MattLebrao) on Aug 31 2020 @ 19h28:
     * 
     * According to cli file "https://github.com/wakatime/wakatime-cli/blob/435b6c667bc2e426f284cd1de38fb168ec82ebcf/cmd/root.go#L45":
     * 
     * Possible values for the category parameter are: "coding", "building", "indexing", "debugging", "running tests", "writing tests",
     *                                                 "manual testing", "code reviewing", "browsing" and "designing".
     * Considering it has a set of valid values it makes sense to make an enum out of it, and use it with "ToString().Replace('_', ' ')".
     */
    public enum HeartbeatCategory
    {
        coding,
        building,
        indexing,
        debugging,
        running_tests,
        writing_tests,
        manual_testing,
        code_reviewing,
        browsing,
        designing
    }

    /* Matheus Rocha (MattLebrao) on Aug 31 2020 @ 21h32:
     * 
     * According to cli file "https://github.com/wakatime/wakatime-cli/blob/435b6c667bc2e426f284cd1de38fb168ec82ebcf/cmd/root.go#L67":
     * 
     * Possible values for the entity type paramater are: "file", "domain", "app".
     * Considering it has a set of valid values it makes sense to make an enum out of it, and use it with "ToString()".
     */
    public enum EntityType
    {
        file,
        domain,
        app
    }
}
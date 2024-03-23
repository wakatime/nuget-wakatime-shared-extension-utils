namespace WakaTime.Shared.ExtensionUtils.Flags
{
    public interface ICliFlag
    {
        string Flag { get; }
        
        string ToString();
        string[] GetFlagWithValue();
    }
}
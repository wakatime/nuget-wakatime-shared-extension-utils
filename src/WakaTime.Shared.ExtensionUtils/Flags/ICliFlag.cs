namespace WakaTime.Shared.ExtensionUtils.Flags
{
    public interface ICliFlag
    {
        #region Properties

        string FlagName { get; }

        #endregion

        #region Abstract Members

        string[] GetFlagWithValue();

        string ToString();

        #endregion
    }
}
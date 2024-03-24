using System;

namespace WakaTime.Shared.ExtensionUtils.Exceptions
{
    public class MissingFlagException : ArgumentNullException
    {
        #region Constructors

        public MissingFlagException(string message) : base(message)
        {
        }

        public MissingFlagException(string flag, string message) : base(flag, message)
        {
        }

        public MissingFlagException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}
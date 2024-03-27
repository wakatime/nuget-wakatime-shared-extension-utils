using System;

namespace WakaTime.Shared.ExtensionUtils.Tests.Stubs
{
    public class LoggerStub : ILogger
    {
        #region Interfaces Implement

        /// <inheritdoc />
        public void Debug(string message)
        {
        }

        /// <inheritdoc />
        public void Error(string message, Exception ex = null)
        {
        }

        /// <inheritdoc />
        public void Warning(string message)
        {
        }

        /// <inheritdoc />
        public void Info(string message)
        {
        }

        #endregion
    }
}
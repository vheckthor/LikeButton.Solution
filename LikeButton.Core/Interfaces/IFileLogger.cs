using System;
using System.Collections.Generic;
using System.Text;

namespace LikeButton.Core.Interfaces
{
    public interface IFileLogger
    {
        public void LogError(string message, string filename, string logDirectory = null);

        public void LogInfo(string message, string filename, string logDirectory = null);

        public void LogWarning(string message, string filename, string logDirectory = null);
    }
}

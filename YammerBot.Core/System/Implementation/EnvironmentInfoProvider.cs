using System;
using System.IO;
using YammerBot.Core.System.Interface;

namespace YammerBot.Core.System.Implementation
{
    public class EnvironmentInfoProvider : IEnvironmentInfoProvider
    {
        public string DataDirectory { get { return new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName; } }
    }
}
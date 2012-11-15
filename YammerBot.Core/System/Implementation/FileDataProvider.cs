using System.Collections.Generic;
using System.IO;
using YammerBot.Core.System.Interface;

namespace YammerBot.Core.System.Implementation
{
    public class FileDataProvider:IFileDataProvider
    {
        public IEnumerable<string> ReadAllLines(string filePath)
        {
            return File.ReadAllLines(filePath);
        }

        public void WriteAllLines(string filePath, IEnumerable<string> lines)
        {
            File.WriteAllLines(filePath, lines);
        }

        public void WriteAllText(string filePath, string data)
        {
            File.WriteAllText(filePath, data);
        }
    }
}
using System.Collections.Generic;

namespace YammerBot.Core.System.Interface
{
    public interface IFileDataProvider
    {
        IEnumerable<string> ReadAllLines(string filePath);
        void WriteAllLines(string filePath, IEnumerable<string> lines);
        void WriteAllText(string filePath, string data);
    }
}
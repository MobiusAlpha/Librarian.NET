using System.IO;
using System.Net.NetworkInformation;

namespace Librarian.Core
{
    public interface IConfigurationManger
    {
        SettingsCollection<string> AppSettings { get; }
        SettingsCollection<ConnectionStringInfo> ConnectionStrings { get; }
        SettingsCollection<T> GetSection<T>(string sectionName) where T : class;
        bool Refresh();
        bool Refresh(string sectionName);
    }
}
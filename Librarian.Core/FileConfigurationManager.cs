using System.Collections.Generic;
using System.Linq;

namespace Librarian.Core
{
    public class FileConfigurationManager : IConfigurationManger
    {
        private readonly string _path;
        private readonly FileType _fileType;

        private IDictionary<string, IDictionary<string, object>> section;
        private readonly object sectionLock = new object();
        private IDictionary<string, ConnectionStringInfo> connectionStrings;
        private IDictionary<string, string> appSettings;

        public FileConfigurationManager(string path, FileType fileType)
        {
            _path = path;
            _fileType = fileType;
            section = new Dictionary<string, IDictionary<string, object>>();
            connectionStrings = load<ConnectionStringInfo>(path, fileType, "connectionStrings");
            appSettings = load<string>(path, fileType, "appSettings");
        }

        private static IDictionary<string, T> load<T>(string path, FileType fileType, string section) where T : class
        {
            switch (fileType)
            {
                case FileType.Xml:
                    return loadXml<T>(path, section);
                case FileType.Json:
                    return loadJson<T>(path, section);
                default:
                    return new Dictionary<string, T>();
            }
        }

        private static IDictionary<string, T> loadXml<T>(string path, string section) where T : class
        {

        }

        private static IDictionary<string, T> loadJson<T>(string path, string section) where T : class
        {

        }


        public SettingsCollection<string> AppSettings
        {
            get
            {
                return new SettingsCollection<string>(appSettings);
            }
        }

        public SettingsCollection<ConnectionStringInfo> ConnectionStrings
        {
            get
            {
                return new SettingsCollection<ConnectionStringInfo>(connectionStrings);
            }
        }

        public SettingsCollection<T> GetSection<T>(string sectionName) where T : class
        {
            lock (sectionLock)
            {
                if (!section.ContainsKey(sectionName))
                {
                    section[sectionName] = load<object>(_path, _fileType, sectionName);
                }
            }

            IDictionary<string, T> sectionVals = section[sectionName].ToDictionary(p => p.Key, q => q.Value as T);

            return new SettingsCollection<T>(sectionVals);
        }

        public bool Refresh()
        {
            lock (sectionLock)
            {
                section.Clear();
            }
            appSettings = load<string>(_path, _fileType, "appSettings");
            connectionStrings = load<ConnectionStringInfo>(_path, _fileType, "connectionStrings");

            return true;
        }

        public bool Refresh(string sectionName)
        {
            bool refreshed = false;

            switch (sectionName)
            {
                case "appSettings":
                    refreshed = true;
                    appSettings = load<string>(_path, _fileType, "appSettings");
                    break;
                case "connectionStrings":
                    refreshed = true;
                    connectionStrings = load<ConnectionStringInfo>(_path, _fileType, "connectionStrings");
                    break;
                default:
                    lock (sectionLock)
                    {
                        var section = load<object>(_path, _fileType, sectionName);
                        refreshed = section?.Any() ?? false;
                        this.section[sectionName] = section;
                    }
                    break;
            }

            return refreshed;
        }

        public enum FileType
        {
            Xml,
            Json
        }
    }
}
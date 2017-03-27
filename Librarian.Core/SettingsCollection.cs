using System.Collections.Generic;

namespace Librarian.Core
{
    public class SettingsCollection<T>
    {
        public T this[string key]
        {
            get
            {
                return Store[key];
            }
        }

        public IDictionary<string, T> Store;

        public SettingsCollection() : this(new Dictionary<string, T>())
        {

        }

        public SettingsCollection(IDictionary<string, T> values)
        {
            Store = new Dictionary<string, T>(values);
        }

        public int Count { get { return Store.Count; } }
    }
}
using System.Collections.Generic;

namespace TestImplementationDecoupling.ExternalCache
{
    public interface IExternalCache
    {
        void Add(string key, int value);
        bool ContainsKey(string key);
        int GetValue(string key);
    }

    public class DummyExternalCache : IExternalCache
    {
        private readonly Dictionary<string, int> _dictionary;

        public DummyExternalCache()
        {
            _dictionary = new Dictionary<string, int>();
        }

        public void Add(string key, int value) => _dictionary.Add(key, value);
        public bool ContainsKey(string key) => _dictionary.ContainsKey(key);
        public int GetValue(string key) => _dictionary[key];
    }
}

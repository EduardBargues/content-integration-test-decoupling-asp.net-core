
using TestImplementationDecoupling.ExternalCache;
using TestImplementationDecoupling.Services.Abstractions;

namespace TestImplementationDecoupling.Services
{
    public class CountCharactersServiceNewImplementation : ICountCharactersService
    {
        private readonly IExternalCache _cache;

        public CountCharactersServiceNewImplementation(IExternalCache cache)
        {
            _cache = cache;
        }

        public int CountCharacters(string word)
        {
            if (word == null)
                return 0;

            if (_cache.ContainsKey(word))
            {
                return _cache.GetValue(word);
            }
            else
            {
                int numberOfCharacters = word.Length;
                _cache.Add(word, numberOfCharacters);
                return numberOfCharacters;
            }
        }
    }
}

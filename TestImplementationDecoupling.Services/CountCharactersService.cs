
using TestImplementationDecoupling.Services.Abstractions;

namespace TestImplementationDecoupling.Services
{
    public class CountCharactersService : ICountCharactersService
    {
        public int CountCharacters(string word) => word != null ? word.Length : 0;
    }
}

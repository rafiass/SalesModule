using System.Collections;

namespace SalesModule
{
    internal interface ISuggestionProvider
    {
        IEnumerable GetSuggestions(string filter);
    }
}

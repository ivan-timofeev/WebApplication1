using System.Runtime.CompilerServices;
using System.Text;
using WebApplication1.Helpers.SearchEngine.Abstractions;
using WebApplication1.Helpers.SearchEngine.Models;

namespace WebApplication1.Helpers.SearchEngine;

public class SearchEngineQueryParser : ISearchEngineQueryParser
{
    private readonly ISearchEngineKeywordHandlerFinder _searchEngineKeywordHandlerFinder;

    public SearchEngineQueryParser(ISearchEngineKeywordHandlerFinder searchEngineKeywordHandlerFinder)
    {
        _searchEngineKeywordHandlerFinder = searchEngineKeywordHandlerFinder;
    }
    
    // search query example: a contains "hello"
    /*
     * a contains "hello", b contains "world", c is "abc"; order-by a asc;
     * a is 123
     * a is 1
     */
    
    public Queue<SearchEngineQueryUnit> ParseSearchQuery(string searchQuery)
    {
        var result = new Queue<SearchEngineQueryUnit>();
        var split = SmartSplit(searchQuery);
        
        var parsedData = new Queue<string>();
        var isOperationParsing = false;
        var currentOperation = string.Empty;

        foreach (var word in split)
        {
            if (!_searchEngineKeywordHandlerFinder.IsSupportedOperation(word))
            {
                parsedData.Enqueue(word);
                continue;
            }
            
            if (!isOperationParsing)
            {
                if (_searchEngineKeywordHandlerFinder.IsSupportedOperation(word))
                {
                    isOperationParsing = true;
                    currentOperation =  word;
                }
                continue;
            }

            // next keyword reached
            if (_searchEngineKeywordHandlerFinder.IsSupportedOperation(word))
            {
                var queryUnit = ParseQueryUnit(parsedData, currentOperation);
                result.Enqueue(queryUnit);
                
                currentOperation = word;
            }
        }

        // end reached
        if (currentOperation != string.Empty) 
        {
            var queryUnit = ParseQueryUnit(parsedData, currentOperation);
            result.Enqueue(queryUnit);
        }

        return result;
    }

    private SearchEngineQueryUnit ParseQueryUnit(Queue<string> parsedData, string currentOperation)
    {
        var handler = _searchEngineKeywordHandlerFinder.GetHandler(currentOperation);
        var value1 = parsedData.Dequeue();
        string? value2 = null;

        if (handler is ICanBeUnary)
        {
            parsedData.TryDequeue(out value2);
        }
        else
        {
            value2 = parsedData.Dequeue();
        }

        return new SearchEngineQueryUnit(handler, value1, value2);
    }

    private IEnumerable<string> SmartSplit(string source)
    {
        var result = new List<string>();
        var buffer = new StringBuilder(1024);

        var isQuoteStarted = false;
        
        foreach (var symbol in source)
        {
            if (symbol == '"')
            {
                if (isQuoteStarted)
                {
                    result.Add(buffer.ToString());
                    buffer.Clear();
                    isQuoteStarted = false;
                    continue;
                }
                
                isQuoteStarted = true;
                continue;
            }
            
            if (isQuoteStarted)
            {
                buffer.Append(symbol);
                continue;
            }

            if (symbol == '-')
            {
                buffer.Append(symbol);
                continue;
            }

            if (!char.IsLetterOrDigit(symbol))
            {
                result.Add(buffer.ToString());
                buffer.Clear();
                continue;
            }
            
            buffer.Append(symbol);
        }
        
        result.Add(buffer.ToString());

        return result
            .Where(x => x != "," && x != "" && x != " " && x != ";")
            .ToArray();
    }
}
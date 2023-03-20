using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Abstractions.Services.SearchEngine;
using WebApplication1.Services.SearchEngine;
using WebApplication1.Services.SearchEngine.DI;
using WebApplication1.Services.SearchEngine.Models;


BenchmarkRunner.Run<SearchEngineBenchmark>();

public class SearchEngineBenchmark
{
    [Params(10_000, 100_000, 1_000_000)]
    public int SourceCapacity;
    
    [IterationSetup]
    public void Setup()
    {
        _source = CreateSource(SourceCapacity);
        _searchEngine = CreateSearchEngine();
        _filter = CreateFilter();
    }
    
    private IQueryable<DummyEntity>? _source;
    private ISearchEngine? _searchEngine;
    private SearchEngineFilter? _filter;

    private static ISearchEngine CreateSearchEngine()
    {
        return new ServiceCollection()
            .AddSearchEngine()
            .BuildServiceProvider()
            .GetRequiredService<ISearchEngine>();
    }

    private static SearchEngineFilter CreateFilter()
    {
        return new SearchEngineFilterBuilder()
            .WithEquals(nameof(DummyEntity.Field1), "1", AttributeTypeEnum.Text, out _)
            .WithStartsWith(nameof(DummyEntity.Field2), "2", AttributeTypeEnum.Text, out _)
            .Build();
    }

    private static IQueryable<DummyEntity> CreateSource(int capacity)
    {
        var list = new List<DummyEntity>(capacity);

        for (var i = 0; i < capacity; i++)
        {
            list.Add(new DummyEntity(Random.Shared.Next().ToString(), Random.Shared.Next().ToString()));
        }

        return list.AsQueryable();
    }

    [Benchmark(Description = "SearchEngine_WithMaterialisation")]
    public void SearchEngine_WithMaterialisation()
    {
        _ = _searchEngine?.ExecuteEngine(_source!, _filter)
            .ToArray();
    }
    [Benchmark(Description = "EfCore_WithMaterialisation")]
    public void EfCore_WithMaterialisation()
    {
        _ = _source?.Where(x =>
                x.Field1 == "1"
                && x.Field2 != null && x.Field2.StartsWith("2"))
            .ToArray();
    }
}

record DummyEntity(string Field1, string? Field2);

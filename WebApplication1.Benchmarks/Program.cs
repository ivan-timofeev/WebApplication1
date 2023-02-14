using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Abstraction.Services.SearchEngine;
using WebApplication1.Services.SearchEngine;
using WebApplication1.Services.SearchEngine.DI;
using WebApplication1.Services.SearchEngine.Models;


BenchmarkRunner.Run<SearchEngineBenchmark>();

public class SearchEngineBenchmark
{
    private static IQueryable<DummyEntity> source_100K = CreateSource(100_000);
    private static IQueryable<DummyEntity> source_1M = CreateSource(1_000_000);
    
    private static ISearchEngine _searchEngine = CreateSearchEngine();
    private static SearchEngineFilter _filter = CreateFilter();
    
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

    [Benchmark(Description = "SearchEngine_100kSource")]
    public void SearchEngine_100kSource()
    {
        _ = _searchEngine.ExecuteEngine(source_100K, _filter);
    }
    [Benchmark(Description = "EfCore_100kSource")]
    public void EfCore_100kSource()
    {
        _ = source_100K.Where(x => x.Field1 == "1" && x.Field2.StartsWith("2"));
    }

    [Benchmark(Description = "SearchEngine_1mSource")]
    public void SearchEngine_1mSource()
    {
        _ = _searchEngine.ExecuteEngine(source_1M, _filter);
    }
    [Benchmark(Description = "EfCore_1mSource")]
    public void EfCore_1mSource()
    {
        _ = source_1M.Where(x => x.Field1 == "1" && x.Field2.StartsWith("2"));
    }

    [Benchmark(Description = "SearchEngine_100kSourceWithMaterialisation")]
    public void SearchEngine_100kSourceWithMaterialisation()
    {
        _ = _searchEngine.ExecuteEngine(source_100K, _filter)
            .ToArray();
    }
    [Benchmark(Description = "EfCore_100kSourceWithMaterialisation")]
    public void EfCore_100kSourceWithMaterialisation()
    {
        _ = source_100K.Where(x => x.Field1 == "1" && x.Field2.StartsWith("2"))
            .ToArray();
    }
    
    [Benchmark(Description = "SearchEngine_1mSourceWithMaterialisation")]
    public void SearchEngine_1mSourceWithMaterialisation()
    {
        _ = _searchEngine.ExecuteEngine(source_1M, _filter)
            .ToArray();
    }
    [Benchmark(Description = "EfCore_1mSourceWithMaterialisation")]
    public void EfCore_1mSourceWithMaterialisation()
    {
        _ = source_1M.Where(x => x.Field1 == "1" && x.Field2.StartsWith("2"))
            .ToArray();
    }
}

record DummyEntity(string Field1, string? Field2);

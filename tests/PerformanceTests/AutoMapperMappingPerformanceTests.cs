using AutoMapper;
using NBomber.CSharp;
using Serilog.Events;
using System.Linq;
using Test.DTO;
using Test.Objects;
using Test.ValueObjects;
using Xunit.Abstractions;

namespace PerformanceTests;

public class AutoMapperMappingPerformanceTests
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly Example _example;
    private readonly ExampleWithValueObject _exampleWithValueObjects;
    private readonly Clothes _clothes;

    public AutoMapperMappingPerformanceTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;

        _example = new()
        {
            Id = 1,
            Name = "test",
            Description = "test description",
            SecureData = "some data",
            Test = new TestValueObject("test")
        };

        _exampleWithValueObjects = new()
        {
            Id = 2,
            ValueObject = new TestValueObject("test"),
            Test = "test"
        };

        _clothes = new()
        {
            Id = Guid.NewGuid(),
            Brand = "nike",
            Model = "jordan",
            Color = "white",
            Category = "Category",
            Size = "XXL",
            Price = 99.99f,
            Quantity = 1,
            ImagesUrl = ["test url", "test url 2"]
        };
    }

    [Fact]
    public void map_classes_100_times_per_second()
    {
        var scenario = Scenario.Create("map classes", async context =>
        {
            try
            {
                var dto = AutoMapperService.Map<Example, ExampleDto>(_example);

                // logging dto's data
                //context.Logger.Information($"Id: {_example.Id}");
                //context.Logger.Information($"Name: {_example.Name}");
                //context.Logger.Information($"Description: {_example.Description}");
                //context.Logger.Information($"Test value: {_example.Test.Value}");

                return Response.Ok();
            }
            catch
            {
                return Response.Fail();
            }
        })
        .WithWarmUpDuration(TimeSpan.FromSeconds(1))
        .WithLoadSimulations(Simulation.Inject(rate: 100, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(2)));

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .Run();

        _outputHelper.WriteLine($"OK: {stats.AllOkCount}, FAILED: {stats.AllFailCount}");
        _outputHelper.WriteLine($"Latency: {stats.ScenarioStats[0].Ok.Latency.MeanMs}");
    }

    [Fact]
    public void map_classes_with_value_objects_100_times_per_second()
    {
        var scenario = Scenario.Create("map classes", async context =>
        {
            try
            {
                var dto = AutoMapperService.Map<ExampleWithValueObject, ExampleWithValueObjectDto>(_exampleWithValueObjects);

                return Response.Ok();
            }
            catch
            {
                return Response.Fail();
            }
        })
        .WithWarmUpDuration(TimeSpan.FromSeconds(1))
        .WithLoadSimulations(Simulation.Inject(rate: 100, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(2)));

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .Run();

        _outputHelper.WriteLine($"OK: {stats.AllOkCount}, FAILED: {stats.AllFailCount}");
        _outputHelper.WriteLine($"Latency: {stats.ScenarioStats[0].Ok.Latency.MeanMs}");
    }

    [Fact]
    public void map_classes_with_more_properties_100_times_per_second()
    {
        var scenario = Scenario.Create("map classes", async context =>
        {
            try
            {
                var dto = AutoMapperService.Map<Clothes, ClothesDto>(_clothes);

                return Response.Ok();
            }
            catch
            {
                return Response.Fail();
            }
        })
        .WithWarmUpDuration(TimeSpan.FromSeconds(1))
        .WithLoadSimulations(Simulation.Inject(rate: 100, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(2)));

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .Run();

        _outputHelper.WriteLine($"OK: {stats.AllOkCount}, FAILED: {stats.AllFailCount}");
        _outputHelper.WriteLine($"Latency: {stats.ScenarioStats[0].Ok.Latency.MeanMs}");
    }
}
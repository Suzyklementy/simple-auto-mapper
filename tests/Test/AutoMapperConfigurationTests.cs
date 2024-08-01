using AutoMapper;
using AutoMapper.Exceptions;
using Test.DTO;
using Test.Objects;
using Test.ValueObjects;

namespace Test;

[Collection("Sequential")]
public class AutoMapperConfigurationTests
{
    private readonly Example _example;

    public AutoMapperConfigurationTests()
    {
        AutoMapperConfiguration<Example, ExampleDto>.Maps.Clear();

        _example = new Example()
        {
            Id = 1,
            Name = "Test",
            Description = "Test Description",
            SecureData = "Secured Data",
            Test = new TestValueObject("Test")
        };
    }

    [Fact]
    public void configure_custom_map_and_map_classes_should_succeed()
    {
        AutoMapperConfiguration<Example, ExampleDto>.ConfigureMap(x => x.Name, x => x.SecureData);
        var dto = AutoMapperService.Map<Example, ExampleDto>(_example);

        Assert.NotNull(dto);
        Assert.Equal(_example.SecureData, dto.Name);
    }

    [Fact]
    public void delete_custom_map_should_succeed()
    {
        AutoMapperConfiguration<Example, ExampleDto>.ConfigureMap(x => x.Name, x => "Test");
        AutoMapperConfiguration<Example, ExampleDto>.DeleteMap(x => x.Name);

        Assert.Empty(AutoMapperConfiguration<Example, ExampleDto>.Maps);
    }

    [Fact]
    public void overwrite_custom_map_and_map_classes_should_succeed()
    {
        AutoMapperConfiguration<Example, ExampleDto>.ConfigureMap(x => x.Name, x => x.SecureData);
        AutoMapperConfiguration<Example, ExampleDto>.ConfigureMap(x => x.Name, x => "Test");

        var dto = AutoMapperService.Map<Example, ExampleDto>(_example);

        Assert.NotNull(dto);
        Assert.Single(AutoMapperConfiguration<Example, ExampleDto>.Maps.Where(x => x.PropertyName == "Name"));
    }
}
using AutoMapper;
using AutoMapper.Exceptions;
using Test.DTO;
using Test.Objects;
using Test.ValueObjects;
using Xunit.Abstractions;

namespace Test;

[Collection("Sequential")]
public class AutoMapperServiceTests
{
    private readonly Example _example;
    private readonly ExampleWithValueObject _exampleWithValueObject;

    public AutoMapperServiceTests()
    {
        AutoMapperConfiguration<Example, ExampleDto>.Maps.Clear();
        AutoMapperConfiguration<ExampleWithValueObject, ExampleWithValueObjectDto>.Maps.Clear();

        _example = new Example()
        {
            Id = 1,
            Name = "Test",
            Description = "Test Description",
            SecureData = "Secured Data",
            Test = new TestValueObject("Test")
        };

        _exampleWithValueObject = new ExampleWithValueObject()
        {
            Id = 2,
            ValueObject = new TestValueObject("Test"),
            Test = "Test"
        };
    }

    [Fact]
    public void map_classes_using_map_method_should_succeed()
    {
        var dto = AutoMapperService.Map<Example, ExampleDto>(_example);

        Assert.NotNull(dto);
        Assert.Equal(_example.Id, dto.Id);
        Assert.NotNull(dto.Name);
        Assert.NotNull(dto.Description);
        Assert.NotNull(dto.Test.Value);
    }

    [Fact]
    public void map_classes_with_value_objects_using_map_with_value_objects_method_should_succeed()
    {
        var dto = AutoMapperService.MapWithValueObjects<ExampleWithValueObject, ExampleWithValueObjectDto>(_exampleWithValueObject);

        Assert.NotNull(dto);
        Assert.Equal(_exampleWithValueObject.Id, dto.Id);
        Assert.NotNull(dto.ValueObject);
    }

    [Fact]
    public void map_class_without_properties_should_fail()
    {
        var exception = Record.Exception(() => AutoMapperService.Map<ExampleWithoutProperties, ExampleDto>(new ExampleWithoutProperties()));

        Assert.IsType<AutoMapperException>(exception);
        Assert.Contains("One of the given classes does not have any properties to map", exception.Message);
    }
}
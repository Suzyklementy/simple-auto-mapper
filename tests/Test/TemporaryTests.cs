using AutoMapper;
using AutoMapper.Exceptions;
using Test.DTO;
using Test.Objects;
using Test.ValueObjects;
using Xunit.Abstractions;

namespace Test;

[Collection("Sequential")]
public class TemporaryTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TemporaryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void test_auto_mapper_map_method_should_succed()
    {
        var example = new Example()
        {
            Id = 1,
            Name = "Test",
            Description = "Test Description",
            SecureData = "Secured Data",
            Test = new TestValueObject("Test")
        };

        var exampleDto = AutoMapperService.Map<Example, ExampleDto>(example);

        _testOutputHelper.WriteLine("ExampleDto properties values:");
        _testOutputHelper.WriteLine($"Id: {exampleDto.Id}");
        _testOutputHelper.WriteLine($"Name: {exampleDto.Name}");
        _testOutputHelper.WriteLine($"Description: {exampleDto.Description}");
        _testOutputHelper.WriteLine($"Test: {exampleDto.Test.Value}");

        Assert.False(exampleDto.Id == 0);
        Assert.NotNull(exampleDto.Name);
        Assert.NotNull(exampleDto.Description);
        Assert.NotNull(exampleDto.Test.Value);
    }

    [Fact]
    public void test_auto_mapper_try_map_method_should_succed()
    {
        var example = new Example()
        {
            Id = 1,
            Name = "Test",
            Description = "Test Description",
            SecureData = "Secured Data",
            Test = new TestValueObject("Test")
        };

        var exampleDto = AutoMapperService.TryMap<Example, ExampleDto>(example);

        _testOutputHelper.WriteLine("ExampleDto properties values:");
        _testOutputHelper.WriteLine($"Id: {exampleDto.Id}");
        _testOutputHelper.WriteLine($"Name: {exampleDto.Name}");
        _testOutputHelper.WriteLine($"Description: {exampleDto.Description}");
        _testOutputHelper.WriteLine($"Test: {exampleDto.Test.Value}");

        Assert.False(exampleDto.Id == 0);
        Assert.NotNull(exampleDto.Name);
        Assert.NotNull(exampleDto.Description);
        Assert.NotNull(exampleDto.Test.Value);
    }

    [Fact]
    public void test_auto_mapper_map_with_value_objects_method_should_succed()
    {
        var example = new ExampleWithValueObject()
        {
            Id = 2,
            ValueObject = new TestValueObject("Test"),
            Test = "Test"
        };

        var exampleDto = AutoMapperService.MapWithValueObjects<ExampleWithValueObject, ExampleWithValueObjectDto>(example);

        _testOutputHelper.WriteLine("ExampleDto properties values:");
        _testOutputHelper.WriteLine($"Id: {exampleDto.Id}");
        _testOutputHelper.WriteLine($"ValueObject: {exampleDto.ValueObject}");

        Assert.False(exampleDto.Id != 2);
        Assert.NotNull(exampleDto.ValueObject);
    }

    [Fact]
    public void map_real_classes_should_succed()
    {
        var clothes = new Clothes()
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

        var clothesDto = AutoMapperService.Map<Clothes, ClothesDto>(clothes);

        _testOutputHelper.WriteLine("ClothesDto properties values:");
        _testOutputHelper.WriteLine($"Id: {clothesDto.Id}");
        _testOutputHelper.WriteLine($"Brand: {clothesDto.Brand}");
        _testOutputHelper.WriteLine($"Model: {clothesDto.Model}");
        _testOutputHelper.WriteLine($"Color: {clothesDto.Color}");
        _testOutputHelper.WriteLine($"Category: {clothesDto.Category}");
        _testOutputHelper.WriteLine($"Size: {clothesDto.Size}");
        _testOutputHelper.WriteLine($"Price: {clothesDto.Price}");

        foreach(var image in clothesDto.ImagesUrl)
        {
            _testOutputHelper.WriteLine($"Image url: {image}");
        }

        Assert.NotNull(clothesDto);
    }

    [Fact]
    public void map_classes_without_properties_should_fail()
    {
        var example = new Example()
        {
            Id = 1,
            Name = "Test",
            Description = "Test Description",
            SecureData = "Secured Data",
            Test = new TestValueObject("Test")
        };

        var action = () => AutoMapperService.Map<Example, ExampleWithoutProperties>(example);

        Assert.Throws<AutoMapperException>(action);
    }

    [Fact]
    public void test_auto_mapper_configuration()
    {
        var example = new Example()
        {
            Id = 1,
            Name = "Test",
            Description = "Test Description",
            SecureData = "Secured Data",
            Test = new TestValueObject("Test")
        };

        AutoMapperConfiguration<Example, ExampleDto>.ConfigureMap(x => x.Name, x => "XD");
        var exampleDto = AutoMapperService.Map<Example, ExampleDto>(example);

        Console.WriteLine($"Description: {exampleDto.Description}");
    }

    [Fact]
    public void test_advance_auto_mapper_configuration()
    {
        var example = new Example()
        {
            Id = 1,
            Name = "Test",
            Description = "Test Description",
            SecureData = "Secured Data",
            Test = new TestValueObject("XDDD"),
            TestArray = [new TestValueObject("XDDD")]
        };

        AutoMapperConfiguration<Example, ExampleDto>.ConfigureMap(x => x.TestArray, x => null);
        var exampleDto = AutoMapperService.Map<Example, ExampleDto>(example);

        _testOutputHelper.WriteLine($"Name: {exampleDto.Name}");
    }
}
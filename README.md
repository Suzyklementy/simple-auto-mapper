# Simple AutoMapper

Simple AutoMapper is a .NET NuGet package that provides a service for automatic mapping objects (such as classes, records and structures) and it doesn't require any configuration to use.

## Installation

Use a [NuGet Package Manager](https://learn.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio) and search for *SimpleAutoMapperSuzyklementy* or use a CLI command.

```bash
dotnet add package SimpleAutoMapperSuzyklementy
```

## Usage

To use AutoMapper, refer to the static class *AutoMapperService* and use the selected method to map two objects. 

The first generic type (TOriginal) represents the base object from which values ​​will be mapped to the second generic type (TTarget) which represents the object to which values ​​will be mapped, e.g. DTO for the base class. The method parameter is the original TOriginal type object from which the values ​​will be mapped.

> [!IMPORTANT]
> Only values represented by property will be mapping.

### Example

```csharp
var example = new Example()
{
    Id = 1,
    Name = "Test",
    Description = "Test Description",
    SecureData = "Secured Data"
};

var exampleDto = AutoMapperService.Map<Example, ExampleDto>(example);
```

If some properties are present only in TOriginal they will be just ignored but, when some of them occur only in TTarget they will be null or default value.

## License

This project is licensed under the terms of the [MIT license](./LICENSE).

<br>

# Documentation

More details about project and technical documentation [here](https://github.com/Suzyklementy/simple-auto-mapper/wiki).
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

This project is open sorce and avaiable to use for everyone 

<br>

# Documentation

Technical documentation of SimpleAutoMapper NuGet Package

## AutoMapperService

AutoMapperService is a static component that provides methods to automatic mapping

### Methods

```csharp
Map<TOriginal, TTarget>(TOriginal originalObject)
```

TOriginal - Type of base object <br>
TTarget - Type of target object <br>
TOriginal originalObject - Instatnce of object type TOriginal <br>

This method mapped property values from *originalObject* type *TOriginal* to new object type *TTarget* then return it. <br><br>

```csharp
TryMap<TOriginal, TTarget>(TOriginal originalObject)
```

TOriginal - Type of base object <br>
TTarget - Type of target object <br>
TOriginal originalObject - Instatnce of object type TOriginal <br>

Works the same as *Map* method but ignores occur exceptions. It's less save than *Map* method but might be better in some cases. If something went wrong, it should return new empty TTarget object or an object with some properties unmapped. <br><br>

```csharp
MapWithValueObjects<TOriginal, TTarget>(TOriginal originalObject)
```

TOriginal - Type of base object <br>
TTarget - Type of target object <br>
TOriginal originalObject - Instatnce of object type TOriginal <br>

This method mapped property values from *originalObject* type *TOriginal* to new object type *TTarget* then return it but, it also work when property is type that contain another property in it (Value Object) but, it must have only one property in it otherwise this property will not be mapped. <br><br>

```csharp
TryMapWithValueObjects<TOriginal, TTarget>(TOriginal originalObject)
```

TOriginal - Type of base object <br>
TTarget - Type of target object <br>
TOriginal originalObject - Instatnce of object type TOriginal <br>

Less save variant of *MapWithValueObjects* method, works like *TryMap* method. <br><br>

> [!IMPORTANT]
> *TOriginal* and *TTarget* must have parameter less constructor, *TTarget's* properties must be public setter

## AutoMapperException

AutoMapperException is an exception that might occur while using AutoMapper. It shows a message about what types couldn't be mapped succesful and other details about exception.

### Example

```
AutoMapper.AutoMapperException : AutoMapper Exception: Something went wrong while trying to map type - (some type) to type - (some type) 
Exception details: (some details about exception)
```

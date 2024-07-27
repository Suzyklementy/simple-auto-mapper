using Test.ValueObjects;

namespace Test.Objects;

public class ExampleWithValueObject
{
    public int Id { get; set; }
    public TestValueObject ValueObject { get; set; }
    public string Test { get; set; }
}
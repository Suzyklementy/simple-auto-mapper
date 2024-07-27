using Test.ValueObjects;

namespace Test.Objects;

public class Example
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string SecureData { get; set; }
    public TestValueObject Test { get; set; }
}
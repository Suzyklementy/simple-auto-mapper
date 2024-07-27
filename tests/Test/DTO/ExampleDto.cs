using Test.ValueObjects;

namespace Test.DTO;

public class ExampleDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TestValueObject Test { get; set; }
}
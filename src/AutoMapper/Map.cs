namespace AutoMapper;

internal sealed class Map<T>
{
    public string PropertyName { get; }
    public Func<T, object?> MapExpression { get; }

    public Map(string propertyname, Func<T, object?> mapExpression)
    {
        PropertyName = propertyname;
        MapExpression = mapExpression;
    }
}
namespace AutoMapper;

public sealed class AutoMapperException : Exception
{
    public Type OriginalType { get; }
    public Type TargetType { get; }

    public AutoMapperException(Type originalType, Type targetTyp, string message) : base($"AutoMapper Exception: Something went wrong while trying to map type - {originalType} to type - {targetTyp} \n" +
        $"Exception details: {message}")
    {
        OriginalType = originalType;
        TargetType = targetTyp;
    }
}
namespace AutoMapper.Exceptions;

public sealed class AutoMapperConfigurationException : Exception
{
    public Type OriginalType { get; }
    public Type TargetType { get; }

    public AutoMapperConfigurationException(Type originalType, Type targetType, string message) : base($"AutoMapper Configuration Exception: Cannot configure mapping some properties from type - {originalType} to type - {targetType} \n" +
        $"Exception details: {message}") 
    {
        OriginalType = originalType;
        TargetType = targetType;
    }
}
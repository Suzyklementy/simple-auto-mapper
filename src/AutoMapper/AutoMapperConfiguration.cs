using AutoMapper.Exceptions;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoMapper;

public static class AutoMapperConfiguration<TOriginal, TTarget> 
    where TOriginal : new()
    where TTarget : new()
{
    internal static List<Map<TOriginal>> Maps { get; private set; } = new();

    public static void ConfigureMap(Expression<Func<TTarget, object>> targetExpression, Expression<Func<TOriginal, object>> valueResolverExpression)
    {
        var targetProperty = GetProperty(targetExpression) 
            ?? throw new AutoMapperConfigurationException(typeof(TOriginal), typeof(TTarget), "Cannot match mapping property with target type");
        var originalProperty = GetProperty(valueResolverExpression);
        var valueResolver = valueResolverExpression.Compile();

        try
        {
            DeleteMap(targetExpression);
        } 
        catch { }

        Type resolvedValueType;
        if (originalProperty is null)
        {
            resolvedValueType = valueResolver(new TOriginal()).GetType();
        }
        else
        {
            resolvedValueType = originalProperty.PropertyType;
        }

        Type targetType = targetProperty.PropertyType;
        var converter = TypeDescriptor.GetConverter(resolvedValueType);

        if (targetType == resolvedValueType)
        {
            var map = new Map<TOriginal>(targetProperty.Name, valueResolver);
            Maps.Add(map);
        }
        else if (converter.CanConvertTo(targetType)) 
        {
            Func<TOriginal, object> convertedFunc = obj => 
            { 
                return converter.ConvertTo(valueResolver(obj), targetType) ?? new object();  
            };

            var map = new Map<TOriginal>(targetProperty.Name, convertedFunc);
            Maps.Add(map);
        }
        else
        {
            throw new AutoMapperConfigurationException(typeof(TOriginal), typeof(TTarget), "Cannot configure map for those object cause type of target property does not match or is not convertable to input type");
        }
    }
    public static void DeleteMap(Expression<Func<TTarget, object>> targetExpression)
    {
        var targetProperty = GetProperty(targetExpression) 
            ?? throw new AutoMapperConfigurationException(typeof(TOriginal), typeof(TTarget), "Invalid expression");

        var map = Maps.FirstOrDefault(x => x.PropertyName == targetProperty.Name)
            ?? throw new AutoMapperConfigurationException(typeof(TOriginal), typeof(TTarget), "Map to delete not found");

        Maps.Remove(map);
    }

    private static PropertyInfo? GetProperty<T>(Expression<Func<T, object>> expression)
    {
        var memberExpression = expression.Body as MemberExpression ?? (expression.Body as UnaryExpression)?.Operand as MemberExpression;

        if (memberExpression is null)
        {
            return default;
        }

        if (memberExpression.Member is not PropertyInfo property)
        {
            return default;
        }

        var properties = typeof(T).GetProperties();
        if(!properties.Contains(property))
        {
            return default;
        }

        return property;
    }
}
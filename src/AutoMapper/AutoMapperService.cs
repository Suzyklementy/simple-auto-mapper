using System.ComponentModel;
using System.Reflection;

namespace AutoMapper;

public static class AutoMapperService
{
    public static TTarget Map<TOriginal, TTarget>(TOriginal originalObject)
        where TOriginal : new()
        where TTarget : new() 
    {
        var originalProperties = typeof(TOriginal).GetProperties();
        var targetProperties = typeof(TTarget).GetProperties();

        if(originalProperties.Length == 0 || targetProperties.Length == 0)
        {
            throw new AutoMapperException(typeof(TOriginal), typeof(TTarget), "One of the given classes does not have any properties to map");
        }

        TTarget mappedTarget = new();
        for (int i = 0; i < originalProperties.Length; i++)
        {
            for (int j = 0; j < targetProperties.Length; j++)
            {
                try
                {
                    var originalProperty = originalProperties[i];
                    var targetProperty = targetProperties[j];

                    if (originalProperty.Name.ToLower() == targetProperty.Name.ToLower())
                    {
                        MapProperty(originalProperty, targetProperty, originalProperty.GetValue(originalObject), ref mappedTarget);
                    }
                }
                catch(Exception e)
                {
                    throw new AutoMapperException(typeof(TOriginal), typeof(TTarget), e.Message);
                }
            }
        }

        return mappedTarget;
    }

    public static TTarget TryMap<TOriginal, TTarget>(TOriginal originalObject)
        where TTarget : new()
        where TOriginal : new()
    {
        var originalProperties = typeof(TOriginal).GetProperties();
        var targetProperties = typeof(TTarget).GetProperties();

        TTarget mappedTarget = new();
        for (int i = 0; i < originalProperties.Length; i++)
        {
            for (int j = 0; j < targetProperties.Length; j++)
            {
                try
                {
                    var originalProperty = originalProperties[i];
                    var targetProperty = targetProperties[j];

                    if (originalProperty.Name.ToLower() == targetProperty.Name.ToLower())
                    {
                        MapProperty(originalProperty, targetProperty, originalProperty.GetValue(originalObject), ref mappedTarget);
                    }
                }
                catch
                {
                }
            }
        }

        return mappedTarget;
    }

    public static TTarget MapWithValueObjects<TOriginal, TTarget>(TOriginal originalObject)
        where TTarget : new()
        where TOriginal : new()
    {
        var originalProperties = typeof(TOriginal).GetProperties();
        var targetProperties = typeof(TTarget).GetProperties();

        if (originalProperties.Length == 0 || targetProperties.Length == 0)
        {
            throw new AutoMapperException(typeof(TOriginal), typeof(TTarget), "One of the given classes does not have any properties to map");
        }

        TTarget mappedTarget = new();
        for (int i = 0; i < originalProperties.Length; i++)
        {
            for (int j = 0; j < targetProperties.Length; j++)
            {
                try
                {
                    var originalProperty = originalProperties[i];
                    var targetProperty = targetProperties[j];

                    if (originalProperty.Name.ToLower() == targetProperty.Name.ToLower())
                    {
                        MapValueObjectProperty(originalProperty, targetProperty, originalProperty.GetValue(originalObject), ref mappedTarget);
                    }
                }
                catch (Exception e)
                {
                    throw new AutoMapperException(typeof(TOriginal), typeof(TTarget), e.Message);
                }
            }
        }

        return mappedTarget;
    }

    public static TTarget TryMapWithValueObjects<TOriginal, TTarget>(TOriginal originalObject)
       where TTarget : new()
       where TOriginal : new()
    {
        var originalProperties = typeof(TOriginal).GetProperties();
        var targetProperties = typeof(TTarget).GetProperties();

        TTarget mappedTarget = new();
        for (int i = 0; i < originalProperties.Length; i++)
        {
            for (int j = 0; j < targetProperties.Length; j++)
            {
                try
                {
                    var originalProperty = originalProperties[i];
                    var targetProperty = targetProperties[j];

                    if (originalProperty.Name.ToLower() == targetProperty.Name.ToLower())
                    {
                        MapValueObjectProperty(originalProperty, targetProperty, originalProperty.GetValue(originalObject), ref mappedTarget);
                    }
                }
                catch
                {
                }
            }
        }

        return mappedTarget;
    }

    private static void MapProperty<T>(PropertyInfo originalProperty, PropertyInfo targetProperty, object? value, ref T mappedTarget)
    {
        if (originalProperty.PropertyType == targetProperty.PropertyType)
        {
            targetProperty.SetValue(mappedTarget, value);
            return;
        }

        var converter = TypeDescriptor.GetConverter(originalProperty.PropertyType);
        if (converter.CanConvertTo(targetProperty.PropertyType))
        {
            var convertedValue = converter.ConvertTo(value, targetProperty.PropertyType);
            targetProperty.SetValue(mappedTarget, convertedValue);
            return;
        }
    }

    private static void MapValueObjectProperty<T>(PropertyInfo originalProperty, PropertyInfo targetProperty, object? value, ref T mappedTarget)
    {
        if (originalProperty.PropertyType == targetProperty.PropertyType)
        {
            targetProperty.SetValue(mappedTarget, value);
            return;
        }

        var converter = TypeDescriptor.GetConverter(originalProperty.PropertyType);
        if (converter.CanConvertTo(targetProperty.PropertyType))
        {
            var convertedValue = converter.ConvertTo(value, targetProperty.PropertyType);
            targetProperty.SetValue(mappedTarget, convertedValue);
            return;
        }

        var valueObjectProperties = originalProperty.PropertyType.GetProperties();
        if (valueObjectProperties.Length != 1 || value is null)
        {
            return;
        }

        var property = valueObjectProperties[0];
        if (property.PropertyType == targetProperty.PropertyType)
        {
            targetProperty.SetValue(mappedTarget, property.GetValue(value));
            return;
        }

        var valueObjectConverter = TypeDescriptor.GetConverter(property);
        if (valueObjectConverter.CanConvertTo(targetProperty.PropertyType))
        {
            var convertedValue = valueObjectConverter.ConvertTo(property.GetValue(value), targetProperty.PropertyType);
            targetProperty.SetValue(mappedTarget, convertedValue);
        }
    }
}
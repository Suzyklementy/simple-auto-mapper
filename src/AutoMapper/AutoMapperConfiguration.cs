using AutoMapper.Exceptions;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoMapper;

public static class AutoMapperConfiguration<TOriginal, TTarget> 
    where TOriginal : new()
    where TTarget : new()
{
    internal static List<Map<TOriginal>> Maps { get; private set; } = new();


    public static void DeleteMap<T>(Expression<Func<TTarget, T>> targetExpression)
    {
        var targetProperty = GetProperty(targetExpression) 
            ?? throw new AutoMapperConfigurationException(typeof(TOriginal), typeof(TTarget), "Invalid expression");

        var map = Maps.FirstOrDefault(x => x.PropertyName == targetProperty.Name)
            ?? throw new AutoMapperConfigurationException(typeof(TOriginal), typeof(TTarget), "Map to delete not found");

        Maps.Remove(map);
    }

    public static void ConfigureMap<T>(Expression<Func<TTarget, T>> targetExpression, Expression<Func<TOriginal, T>> valueResolverExpression)
    {
        var targetProperty = GetProperty(targetExpression)
            ?? throw new AutoMapperConfigurationException(typeof(TOriginal), typeof(TTarget), "Cannot match mapping property with target type");
        var valueResolver = valueResolverExpression.Compile();

        try
        {
            DeleteMap(targetExpression);
        }
        catch { }

        Func<TOriginal, object?> convertedFunc = obj =>
        {
            return valueResolver(obj);
        };

        var map = new Map<TOriginal>(targetProperty.Name, convertedFunc);
        Maps.Add(map);
       
    }

    private static PropertyInfo? GetProperty<TIn, TOut>(Expression<Func<TIn, TOut>> expression)
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

        return property;
    }
}
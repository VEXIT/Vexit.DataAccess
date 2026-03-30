
/*****************************************
 * 
 *  Copyright       :   © VEXIT® 2010, www.vexit.com
 *  Author          :   Vex Tatarevic
 *  Date Created    :   2010-09-17 : LoadList, LoadEntity 
 *                      2012-09-17 : Map
 *  
 *  Description     :   This utility class provides data parsing, loading and mapping operations
 *  
 *  Updates         :   2021-08-02  -   Added MapFields methods which only map provided fields
 * 
 *****************************************/



using System.Reflection;

namespace Vexit.DataAccess.Utils;

/// <summary>
///  This utility class provides data parsing, loading and mapping operations
/// </summary>
public class DataMapper
{

    public static TTarget? Map<TSource, TTarget>(TSource sourceObject, string excludeFields)
    {
        if (sourceObject == null)
        {
            return default;
        }
        return Map<TSource, TTarget>(sourceObject, null, excludeFields);
    }

    public static TTarget? Map<TSource, TTarget>(TSource sourceObject)
    {
        return Map<TSource, TTarget>(sourceObject, null, null);
    }


    /// <summary>
    ///     Maps values of properties of sourceObject to properties of targetObject
    ///     You can use this in MVC programming when mapping Entity to ViewModel and back
    ///     Limitations: Only maps flat properties (first level/no nesting) that have matching types
    ///     fieldNameMappings : If sourceObject has different names for some fields from targetObject, then pass a dictionary of field mappings that defines which source field to map to which target field
    ///                         example field mapping : new Dictionary<string, string>(){ { "DateCreated", "DateSent" } }
    /// </summary>
    public static TTarget? Map<TSource, TTarget>(TSource sourceObject, Dictionary<string, string>? fieldNameMappings, string? excludeFields = null)
    {
        if (sourceObject == null)
        {
            return default;
        }
        TTarget? targetObject = (TTarget?)Activator.CreateInstance(typeof(TTarget?));
        Map(sourceObject, ref targetObject, fieldNameMappings, excludeFields);
        return targetObject;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    /// <param name="sourceObject"></param>
    /// <param name="targetObject"></param>
    /// <param name="fieldNameMappings">f sourceObject has different names for some fields from targetObject, then pass a dictionary of field mappings that defines which source field to map to which target field
    ///                         example field mapping : new Dictionary<string, string>(){ { "DateCreated", "DateSent" } }</param>
    /// <param name="excludeFields">csv string of field names for the fields that should not be mapped</param>
    public static TTarget Map<TSource, TTarget>(TSource sourceObject, ref TTarget targetObject, Dictionary<string, string>? fieldNameMappings = null, string? excludeFields = null)
    {
        if (sourceObject != null && targetObject != null)
        {
            Type sourceType = sourceObject.GetType();
            Type targetType = targetObject.GetType();
            var sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in sourceProperties)
            {
                string propertyName = fieldNameMappings != null && fieldNameMappings.ContainsKey(property.Name) ? fieldNameMappings[property.Name] : property.Name;

                // if the field is excluded, ignore it
                var ignoredFields = excludeFields != null ? excludeFields.Split(',') : new string[] { };
                var ignoreField = ignoredFields.Length > 0 && ignoredFields.Contains(propertyName);

                var targetProperty = targetType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

                if (targetProperty != null
                      && targetProperty.CanWrite
                      && targetProperty.PropertyType.IsAssignableFrom(GetUnderlyingType(property.PropertyType))
                      && !ignoreField)
                {
                    //if (property.GetValue(sourceObject, null) != null)
                    targetProperty.SetValue(targetObject, property.GetValue(sourceObject, null), null);
                }
            }
        }
        return targetObject;
    }

    /// <summary>
    ///  Map all fields except the excluded fields supplied.
    /// </summary>
    public static TTarget Map<TSource, TTarget>(TSource sourceObject, ref TTarget targetObject, string excludeFields)
    {
        return Map(sourceObject, ref targetObject, null, excludeFields);
    }


    /// <summary>
    ///  Map only supplied fields
    /// </summary>
    public static TTarget MapFields<TSource, TTarget>(TSource sourceObject, ref TTarget targetObject, params string[] fields)
    {
        if (sourceObject != null && targetObject != null)
        {
            Type sourceType = sourceObject.GetType();
            Type targetType = targetObject.GetType();

            var sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in sourceProperties)
            {
                string propertyName = property.Name;
                var targetProperty = targetType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                var mapField = fields.Contains(propertyName);

                if (mapField
                    && targetProperty != null
                    && targetProperty.CanWrite
                    && targetProperty.PropertyType.IsAssignableFrom(GetUnderlyingType(property.PropertyType))
                    )
                {
                    targetProperty.SetValue(targetObject, property.GetValue(sourceObject, null), null);
                }
            }
        }
        return targetObject;
    }
    /// <summary>
    ///  Map only supplied fields
    /// </summary>
    public static TTarget? MapFields<TSource, TTarget>(TSource sourceObject, params string[] fields)
    {
        if (sourceObject == null)
        {
            return default;
        }
        TTarget? targetObject = (TTarget?)Activator.CreateInstance(typeof(TTarget?));
        MapFields(sourceObject, ref targetObject, fields);
        return targetObject;
    }


    /// <summary>
    ///  Map list of one type to a list of another
    ///                         example field mapping : new Dictionary<string, string>(){ { "DateCreated", "DateSent" } }
    /// </summary>
    /// <returns></returns>
    public static List<TTarget>? MapList<TSource, TTarget>(List<TSource> sourceList, Dictionary<string, string>? fieldNameMappings = null)
    {
        var targetList = (List<TTarget>?)Activator.CreateInstance(typeof(List<TTarget>));
        if (sourceList == null || targetList == null)
        {
            return default;
        }
        foreach (var item in sourceList)
        {
            var targetItem = Map<TSource, TTarget>(item, fieldNameMappings);
            if (targetItem != null)
            {
                targetList.Add(targetItem);
            }
        }
        return targetList;
    }
    public static List<TTarget> MapList<TSource, TTarget>(ICollection<TSource> sourceList, Dictionary<string, string>? fieldNameMappings = null)
    {
        return MapList<TSource, TTarget>(sourceList, fieldNameMappings);
    }





    public static Type? GetUnderlyingType(Type type)
    {
        bool isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        return isNullable ? Nullable.GetUnderlyingType(type) : type;
    }

    public static bool IsNullable<T>(T obj)
    {
        if (obj == null) return true; // obvious
        Type type = typeof(T);
        if (!type.IsValueType) return true; // ref-type
        if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
        return false; // value-type
    }


}

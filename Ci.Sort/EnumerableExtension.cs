using Ci.Sort.Enums;
using Ci.Sort.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Ci.Sort
{
    public static class EnumerableExtension
    {
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> query, SortOrder sort)
        {
            PropertyDescriptor GetKeyProperty()
            {
                var properties = GetAllProperties(typeof(T));

                var keyProp = new PropertyDescriptorCollection(properties.ToArray())
                    .Find(sort.Key, true);
                return keyProp;
            }

            List<PropertyDescriptor> GetAllProperties(Type baseType)
            {
                var resList = new List<PropertyDescriptor>();
                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(baseType))
                {
                    resList.Add(propertyDescriptor);
                    var declaredProperties = propertyDescriptor.ComponentType.GetTypeInfo().DeclaredProperties.ToList();
                    if (propertyDescriptor.Attributes.Count == 0 && declaredProperties.Count != 0)
                    {
                        foreach (var propertyInfo in declaredProperties)
                        {
                            var fields = ((TypeInfo)propertyInfo.PropertyType).DeclaredFields;
                            resList.AddRange(PropertyDescriptor(fields.First()).Cast<PropertyDescriptor>());
                        }
                    }
                }

                return resList;
            }

            if (sort == null)
                sort = new SortOrder();

            if (string.IsNullOrWhiteSpace(sort.Key))
            {
                sort.Key = typeof(T)
                    .GetProperties()
                    .OrderByDescending(x => x.Name == "Id")
                    .First()
                    .Name;
            }

            var prop = GetKeyProperty();

            query = sort.Order == Order.Ascending
                ? query.OrderBy(x => prop.GetValue(x))
                : query.OrderByDescending(x => prop.GetValue(x));
            return query;
        }

        public static PropertyDescriptor PropertyDescriptor(this PropertyInfo propertyInfo)
        {
            return TypeDescriptor.GetProperties(propertyInfo.DeclaringType)[propertyInfo.Name];
        }

        public static PropertyDescriptorCollection PropertyDescriptor(this FieldInfo fieldInfo)
        {
            return TypeDescriptor.GetProperties(fieldInfo.DeclaringType);
        }
    }
}

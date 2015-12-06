using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;

namespace TypeLess.Helpers
{
    internal static class ObjectFactory
    {
        public static T CreateObject<T>()
        {
            ConstructorInfo ctor = typeof(T).GetTypeInfo().DeclaredConstructors.Where(x => !x.GetParameters().Any()).FirstOrDefault();
            if (ctor != null)
            {
                return (T)ctor.Invoke(null);
            }

            return default(T);
        }

        public static T CreateObject<T>(Type type, params object[] parameters)
        {
            if (type != null)
            {
                Type[] types = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (parameters[i] != null)
                    {
                        types[i] = parameters[i].GetType();
                    }
                }
                ConstructorInfo ctor = type.GetTypeInfo().DeclaredConstructors.Where(x => x.GetParameters().All(y => types.Any(z => y.ParameterType == z))).FirstOrDefault();

                if (ctor != null)
                {
                    return (T)ctor.Invoke(parameters);
                }
            }
            return default(T);
        }

        public static void ShallowCopyProperties<T>(T source, T target, TypeInfo sourceType = null) where T : class {
            
            var sourceT = source.GetType().GetTypeInfo();
            var targetT = target.GetType().GetTypeInfo();

            if (sourceType != null) {
                sourceT = sourceType;
                targetT = sourceType;
            }

            if (sourceT.BaseType.Namespace != "System") {
                ShallowCopyProperties(source, target, sourceT.BaseType.GetTypeInfo());
            }
            
            var sourceProps = sourceT.DeclaredProperties;
            foreach (var prop in sourceProps)
            {
                var targetProp = targetT.GetDeclaredProperty(prop.Name);

                if (targetProp == null) {
                    continue;
                }

                var info = targetProp.PropertyType.GetTypeInfo();

                if (!prop.CanRead || !targetProp.CanWrite) {
                    continue;
                }
                
                if (info.IsClass && info.Namespace != "System")
                {
                    continue;
                }

                if (info.IsGenericType && info.Name != "Nullable`1")
                {
                    continue;
                }

                targetProp.SetValue(target, prop.GetValue(source));
            }

            var sourceFields = sourceT.DeclaredFields;
            foreach (var field in sourceFields)
            {
                var targetField = targetT.GetDeclaredField(field.Name);

                if (targetField == null)
                {
                    continue;
                }

                if (targetField.IsStatic || targetField.IsInitOnly) {
                    continue;
                }

                var info = targetField.FieldType.GetTypeInfo();

                if (info.IsClass && info.Namespace != "System")
                {
                    continue;
                }

                if (info.IsGenericType && info.Name != "Nullable`1")
                {
                    continue;
                }

                targetField.SetValue(target, field.GetValue(source));
            }

        }

    }
}

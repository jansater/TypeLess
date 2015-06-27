using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TypeLess.Net
{

    public class RandomData
    {
        private Random _rand = new Random();
        private int index = 1;
        private Dictionary<string, string> keys = new Dictionary<string, string>();
        private List<string> assemblyList = new List<string>();

        public static SortedDictionary<string, object> CreateDictionaryFromObject<T>(T obj)
        {
            var dict = new SortedDictionary<string, object>();
            var t = typeof(T);
            var properties = t.GetProperties();
            foreach (var prop in properties)
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    dict.Add(prop.Name, prop.GetValue(obj, null));
                }

            }
            return dict;
        }

        
        public static T Create<T>(bool allowNullValues=true) where T : new()
        {
            RandomData helper = new RandomData();
            var t = new T();
            helper.Fill(t, allowNullValues);
            return t;
        }

        public static List<T> CreateList<T>(int size) where T : new()
        {
            RandomData helper = new RandomData();
            List<T> list = new List<T>(size);
            for (int i = 0; i < size; i++)
            {
                var t = new T();
                helper.Fill(t);
                list.Add(t);
            }
            return list;
        }

        private static object CreateInstanceOfType(Type t, params object[] args)
        {
            RandomData rd = new RandomData();
            var hasParams = args != null && args.Length > 0;

            var ctors = t.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            ConstructorInfo ctorWithLessParams = null;
            if (hasParams)
            {
                ctorWithLessParams = ctors.Where(x => x.GetParameters().Count() == args.Length).FirstOrDefault();
            }
            else
            {
                ctorWithLessParams = ctors.OrderBy(x => x.GetParameters().Count()).FirstOrDefault();
            }

            if (ctorWithLessParams == null)
            {
                throw new Exception("Could not find ctor for type " + t.Name);
            }

            if (!hasParams)
            {
                var parameters = ctorWithLessParams.GetParameters();
                var instantiatedParameters = parameters.Select(x =>
                {
                    try
                    {
                        if (x.ParameterType == typeof(string))
                        {
                            return rd.GetRandomString();
                        }
                        else if (x.ParameterType == typeof(DateTime)) {
                            return rd.GetDateAfter(DateTime.Now);
                        }
                        else if (x.ParameterType.IsPrimitive || x.ParameterType.IsEnum)
                        {
                            return Activator.CreateInstance(x.ParameterType);
                        }
                        else
                        {
                            return CreateInstanceOfType(x.ParameterType);
                        }
                    }
                    catch
                    {
                        return Activator.CreateInstance(x.ParameterType);
                    }
                   
                }).ToArray();
                RandomData helper = new RandomData();
                foreach (var item in instantiatedParameters)
                {
                    helper.Fill(item);
                }
                return ctorWithLessParams.Invoke(instantiatedParameters);
            }
            else
            {
                return ctorWithLessParams.Invoke(args);
            }

        }

        public static object Create(Type t, params object[] args)
        {
            RandomData helper = new RandomData();
            var instance = CreateInstanceOfType(t, args);
            helper.Fill(instance);
            return instance;
        }

        public static T Create<T>(Action<T> modMockFunc) where T : new()
        {
            RandomData helper = new RandomData();
            var t = new T();
            helper.Fill(t);
            modMockFunc(t);
            return t;
        }

        public static IEnumerable<T> Create<T>(int count, Action<T, int> modMockFunc) where T : new()
        {
            RandomData helper = new RandomData();

            for (int i = 0; i < count; i++)
            {
                var t = new T();
                helper.Fill(t);
                modMockFunc(t, i);
                yield return t;
            }
        }

        public RandomData()
        {
            this.MaxIndexValue = 100;
        }

        public int MaxIndexValue { get; set; }

        private void GenerateCode(object obj, bool allowNullValues=true)
        {

            var asEnumerable = obj as IEnumerable;
            if (asEnumerable != null)
            {

                foreach (var item in asEnumerable)
                {
                    GenerateCode(item);
                }

                return;
            }

            var props = obj.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo property in props)
            {
                if (property.PropertyType.IsAbstract || !property.CanRead || !property.CanWrite)
                {
                    continue;
                }

                //Decide to dig deeper, if a PropertyType belongs to the assemblies that we are interested in, we go deep.
                if (!isSystemType(property.PropertyType) && !property.PropertyType.IsEnum)
                {
                    index = index + 1;
                    //Avoid infinite Loop situation
                    if (index < this.MaxIndexValue)
                    {
                        object propertyInstance = property.GetValue(obj, null);
                        if (propertyInstance == null)
                        {
                            //Dynamically create Property Instance
                            if (property.PropertyType.IsArray)
                            {
                                Array arr = Array.CreateInstance(property.PropertyType.GetElementType(), 3);
                                for (int i = 0; i < 3; i++)
                                {
                                    propertyInstance = Activator.CreateInstance(arr.GetType().GetElementType());
                                    GenerateCode(propertyInstance);
                                    arr.SetValue(propertyInstance, i);
                                }
                                if (property.CanWrite)
                                    property.SetValue(obj, arr, null);

                            }
                            else if (!property.PropertyType.IsAbstract)
                            {
                                propertyInstance = CreateInstanceOfType(property.PropertyType);
                                if (property.CanWrite)
                                    property.SetValue(obj, propertyInstance, null);
                                GenerateCode(propertyInstance);
                            }
                        }
                    }
                }
                else if (property.PropertyType.IsArray)
                {
                    Array arr = Array.CreateInstance(property.PropertyType.GetElementType(), 3);
                    object propertyInstance = property.GetValue(obj, null);
                    for (int i = 0; i < 3; i++)
                    {
                        if (isSystemType(arr.GetType().GetElementType()))
                        {
                            if (arr.GetType().GetElementType() == typeof(string))
                            {
                                var s = GetRandomString();
                                arr.SetValue(s, i);
                            }
                            else if (property.PropertyType == typeof(DateTime))
                            {
                                arr.SetValue(GetDateAfterLastDecade(), i);
                            }
                            else if (property.PropertyType == typeof(int))
                            {
                                arr.SetValue(GetRandomInt(), i);
                            }
                        }
                        else
                        {
                            propertyInstance = Activator.CreateInstance(arr.GetType().GetElementType());
                            GenerateCode(propertyInstance);
                            arr.SetValue(propertyInstance, i);
                        }
                    }
                    if (property.CanWrite)
                        property.SetValue(obj, arr, null);
                }
                else if (property.PropertyType == typeof(string))
                {
                    var s = GetRandomString();
                    if (property.CanWrite)
                        property.SetValue(obj, s, null);
                }
                else if (property.PropertyType.IsEnum || (property.PropertyType.IsGenericType && property.PropertyType.GetGenericArguments()[0].IsEnum))
                {
                    property.SetValue(obj, GetRandomEnum(property.PropertyType,allowNullValues), null);
                }
                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    if (allowNullValues && isNullable(property.PropertyType))
                    {
                        if (shouldGenerateNullValue())
                        {
                            continue;
                        }

                    }

                    if (property.CanWrite)
                        property.SetValue(obj, GetDateAfterLastDecade(), null);
                }
                else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                {
                    if (allowNullValues && isNullable(property.PropertyType))
                    {
                        if (shouldGenerateNullValue())
                        {
                            continue;
                        }

                    }

                    if (property.CanWrite)
                        property.SetValue(obj, GetRandomInt(), null);
                }
                else if (property.PropertyType == typeof(long) || property.PropertyType == typeof(long?))
                {
                    if (allowNullValues && isNullable(property.PropertyType))
                    {
                        if (shouldGenerateNullValue())
                        {
                            continue;
                        }

                    }

                    if (property.CanWrite)
                        property.SetValue(obj, GetRandomLong(), null);
                }
                else if (property.PropertyType == typeof(float) || property.PropertyType == typeof(float?))
                {
                    if (allowNullValues && isNullable(property.PropertyType))
                    {
                        if (shouldGenerateNullValue())
                        {
                            continue;
                        }

                    }


                    if (property.CanWrite)
                        property.SetValue(obj, GetRandomFloat(), null);
                }
                else if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
                {
                    if (allowNullValues && isNullable(property.PropertyType))
                    {
                        if (shouldGenerateNullValue())
                        {
                            continue;
                        }

                    }

                    if (property.CanWrite)
                        property.SetValue(obj, GetRandomDouble(), null);
                }
                else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                {
                    if (allowNullValues && isNullable(property.PropertyType))
                    {
                        if (shouldGenerateNullValue())
                        {
                            continue;
                        }

                    }

                    if (property.CanWrite)
                        property.SetValue(obj, GetRandomBoolean(), null);
                }
                else if (property.PropertyType == typeof(TimeSpan) || property.PropertyType == typeof(TimeSpan?))
                {
                    if (allowNullValues && isNullable(property.PropertyType))
                    {
                        if (shouldGenerateNullValue())
                        {
                            continue;
                        }

                    }

                    if (property.CanWrite)
                        property.SetValue(obj, TimeSpan.FromSeconds(GetRandomDouble()), null);
                }
                else if (property.PropertyType.IsEnum)
                {
                    if (property.CanWrite)
                        property.SetValue(obj, GetRandomEnum(property.PropertyType, allowNullValues), null);
                }

            }
        }

        public bool GetRandomBoolean()
        {
            return _rand.Next(0, 1) == 1 ? true : false;
        }

        private bool shouldGenerateNullValue()
        {
            return _rand.Next(0, 1) == 1;
        }

        private bool isNullable(Type type)
        {
            return type.FullName.Contains("Nullable");
        }

        private bool isSystemType(Type t)
        {
            return (t.Namespace == "System" ||
            t.Namespace.StartsWith("System") ||
            t.Module.ScopeName == "CommonLanguageRuntimeLibrary");
        }

        private string[] _possibleStrings = { "text1", "text2", "text3", "text4", "text5", "text6", "text7", "text8", "text9", "text10", "text11", "text12" };



        public string GetRandomString()
        {

            int n = _rand.Next(_possibleStrings.Length);
            return _possibleStrings[n];
        }

        public double GetRandomDouble()
        {

            return _rand.NextDouble() * GetRandomInt()+1;
        }

        public float GetRandomFloat()
        {

            return (float)_rand.NextDouble() * GetRandomInt()+1;
        }

        public int GetRandomInt()
        {

            return _rand.Next(5); //_rand.Next(Int32.MaxValue);
        }

        public DateTime GetDateBetween(DateTime min, DateTime max)
        {
            var newT = Math.Abs(GetRandomLong() % (max.Ticks - min.Ticks));
            return min.AddTicks(newT);
        }

        public DateTime GetDateAfter(DateTime min)
        {
            return GetDateBetween(min, DateTime.UtcNow);
        }

        public DateTime GetDateAfterLastDecade()
        {
            return GetDateBetween(DateTime.UtcNow.AddYears(-10), DateTime.UtcNow);
        }

        public object GetRandomEnum(Type t, bool allowNullValues = true)
        {
            if (!t.IsEnum && !(t.IsGenericType && t.GetGenericArguments()[0].IsEnum))
                throw new ArgumentException("type must be an enum");

            if (isNullable(t))
            {
                t = t.GetGenericArguments()[0];
                if (allowNullValues && shouldGenerateNullValue())
                {
                    return null;
                }

            }

            var values = Enum.GetValues(t);
            var indexToChoose = _rand.Next(values.Length);
            return values.GetValue(indexToChoose);
        }

        public T GetRandomEnum<T>(bool allowNullValues=true)
        {
            return (T)GetRandomEnum(typeof(T), allowNullValues);
        }

        public long GetRandomLong()
        {
            byte[] buf = new byte[8];
            _rand.NextBytes(buf);
            return BitConverter.ToInt64(buf, 0);
        }

        public void Fill(object obj, bool allowNullValues = true)
        {
            index = 1;
            GenerateCode(obj,allowNullValues);
        }


    }


}



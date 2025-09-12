using Newtonsoft.Json;
using System;
using System.Collections;
using System.Reflection;
using System.Threading;


public class ConvertHelper
{
    // https://blog.csdn.net/jayzai/article/details/50667435
    public static object ChangeType(object obj, Type conversionType)
    {
        return ChangeType(obj, conversionType, Thread.CurrentThread.CurrentCulture);
    }
    public static object ChangeType(object obj, Type conversionType, IFormatProvider provider)
    {
        #region Nullable
        Type nullableType = Nullable.GetUnderlyingType(conversionType);
        if (nullableType != null)
        {
            return obj == null ? null : Convert.ChangeType(obj, nullableType, provider);
        }
        #endregion

        #region Enum
        if (typeof(System.Enum).IsAssignableFrom(conversionType))
        {
            return Enum.Parse(conversionType, obj.ToString());
        }

        #endregion
        #region Collection
        if (typeof(IEnumerable).IsAssignableFrom(conversionType) &&
            !typeof(IDictionary).IsAssignableFrom(conversionType) &&
            conversionType != typeof(string))
        {
            string objStr = ChangeType(obj, typeof(string), provider) as string;
            // 不是json数组或者对象，均当做数组，强制加上数组符号
            if (!objStr.StartsWith("[") && !objStr.StartsWith("{"))
            {
                objStr = "[" + objStr + "]";
            }
            return JsonConvert.DeserializeObject(objStr, conversionType);
        }

        #endregion

        #region Object
        try
        {
            return Convert.ChangeType(obj, conversionType, provider);
        }
        catch (Exception ex) when (ex is FormatException or InvalidCastException or
        ArgumentException or OverflowException)
        {

            // 判断是否存在指定构造函数
            ConstructorInfo constructor = conversionType.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                types: new Type[] { obj.GetType() },
                modifiers: null
                );
            if (constructor != null)
            {
                return constructor.Invoke(new object[] { obj });
            }

            // 尝试通过字符串反序列化
            string objStr = ChangeType(obj, typeof(string), provider) as string;
            if (conversionType.IsClass || conversionType.IsValueType)
            {
                try
                {
                    return JsonConvert.DeserializeObject(objStr, conversionType);
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                throw;
            }
        }
        #endregion
    }
}


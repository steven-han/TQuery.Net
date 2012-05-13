using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace TQuery.Net
{
    public class EnumHelper
    {
        private static readonly Hashtable ht = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 从缓存中获取枚举对应的List
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> GetCachedDictionary(Type type)
        {
            if (ht.Contains(type))
            {
                return ht[type] as IList<KeyValuePair<string, string>>;
            }
            else
            {
                IList<KeyValuePair<string, string>> dict = GetDictionary(type);
                ht[type] = dict;
                return dict;
            }
        }

        public static IList<KeyValuePair<string, string>> GetDictionary(Type type)
        {
            if (!type.IsEnum)
            {
                throw new InvalidOperationException("错误的枚举类型");
            }
            FieldInfo[] fields = type.GetFields();
            IList<KeyValuePair<string, string>> dict = new List<KeyValuePair<string, string>>();
            foreach (var item in fields)
            {
                if (item.FieldType.IsEnum == false) { continue; }
                string desription = string.Empty;
                object[] objs = item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs != null && objs.Length > 0)
                {
                    DescriptionAttribute da = (DescriptionAttribute)objs[0];
                    desription = da.Description;
                }
                else
                {
                    desription = item.Name;
                }
                dict.Add(new KeyValuePair<string, string>(((int)Enum.Parse(type, item.Name)).ToString(), desription));
            }
            return dict;
        }

        public static string GetCachedDescription(Type type, string fieldName)
        {
            IList<KeyValuePair<string, string>> dictionary = GetCachedDictionary(type);
            foreach (KeyValuePair<string, string> keyValuePair in dictionary)
            {
                if (keyValuePair.Key == ((int)(Enum.Parse(type, fieldName))).ToString())
                {
                    return keyValuePair.Value;
                }
            }
            return fieldName;
        }

        public static string GetCachedFieldName(Type type, string description)
        {
            IList<KeyValuePair<string, string>> dictionary = GetCachedDictionary(type);
            foreach (KeyValuePair<string, string> keyValuePair in dictionary)
            {
                if (keyValuePair.Value == description)
                {
                    return keyValuePair.Key;
                }
            }
            return description;
        }

        ///<summary>
        /// 根据枚举类型获取描述
        ///</summary>
        ///<param name="enumSubitem">类型</param>
        ///<returns>描述</returns>
        public static string GetEnumDescription(Enum value)
        {
            // Get the Description attribute value for the enum value
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// 从缓存中获取枚举对应的List[添加一空白列]
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> GetCachedDictionaryBlank(Type type)
        {
            if (ht.Contains(type))
            {
                return ht[type] as IList<KeyValuePair<string, string>>;
            }
            IList<KeyValuePair<string, string>> dict = GetDictionary(type);
            dict.Insert(0, new KeyValuePair<string, string>((-1).ToString(), string.Empty));
            ht[type] = dict;
            return dict;
        }

        public static KeyValuePair<string, string> GetEnum(Type type, string fieldName)
        {
            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>();
            IList<KeyValuePair<string, string>> dictionary = GetCachedDictionary(type);
            foreach (KeyValuePair<string, string> keyValuePair in dictionary)
            {
                if (keyValuePair.Key == ((int)(Enum.Parse(type, fieldName))).ToString())
                {
                    kvp = keyValuePair;
                    break;
                }
            }
            return kvp;
        }
    }
}
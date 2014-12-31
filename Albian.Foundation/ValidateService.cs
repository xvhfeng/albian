using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Albian.Foundation.Verify
{
    public class ValidateService
    {
        public static bool IsNullOrEmpty(object o)
        {
            return string.IsNullOrEmpty(o.ToString());
        }

        public static bool IsNullOrEmpty(string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrEmpty<T>(IList<T> list)
        {
            return null == list ||  0 == list.Count;
        }

        public static bool IsNullOrEmpty<TKey, TValue>(IDictionary<TKey, TValue> dic)
        {
            return null == dic ||  0 == dic.Count;
        }
        public static bool IsNullOrEmpty(Hashtable ht)
        {
            return null == ht || 0 == ht.Count;
        }

        public static bool IsNull(Hashtable ht)
        {
            return null == ht;
        }

        public static bool IsNull(object o)
        {
            return o == null;
        }

        public static bool IsNull(string str)
        {
            return null == str;
        }

        public static bool IsNull<T>(IList<T> list)
        {
            return null == list;
        }

        public static bool IsNull<TKey, TValue>(IDictionary<TKey, TValue> dic)
        {
            return null == dic;
        }


        public static bool IsEmpty(Hashtable ht)
        {
            return null != ht && 0 != ht.Count;
        }

        public static bool IsOrEmpty(object o)
        {
            return null != o ;
        }

        public static bool IsEmpty(string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static bool IsEmpty<T>(IList<T> list)
        {
            return null != list && 0 != list.Count;
        }

        public static bool IsEmpty<TKey, TValue>(IDictionary<TKey, TValue> dic)
        {
            return null != dic && 0 != dic.Count;
        }
    }
}

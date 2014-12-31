#region

using System;
using System.Reflection;
using log4net;

#endregion

namespace Albian.Foundation
{
    public class TypeService
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string GetFullTypeName(Type type)
        {
            if (null == type)
            {
                throw new ArgumentNullException("type");
            }
            string className = type.FullName;
            string assemblyName = type.Assembly.FullName;
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new Exception("the assembly name is empry or null.");
            }
            string[] strs = assemblyName.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            if (0 >= strs.Length)
            {
                throw new Exception("split the assembly is error.");
            }
            return string.Format("{0},{1}", className, strs[0]);
        }

        public static string GetFullTypeName<T>(T target)
        {
            return GetFullTypeName(target.GetType());
        }
        public static string GetFullTypeName<T>()
        {
            return GetFullTypeName(typeof(T));
        }

        public static T CreateInstance<T>(string typeFullName)
        {
             Type type = Type.GetType(typeFullName, true);
             object o = Activator.CreateInstance(type);
            return (T) o;
        }

        /// <summary>
        /// Objects the generator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        public static V CreateInstance<T, V>()
            where T : class, V, new()
            where V : class
        {
            try
            {
                V target = new T();
                return target;
            }
            catch (Exception exc)
            {
                if (null != Logger)
                    Logger.WarnFormat("Create instance is error.Info{0},StackTrace:{1}", exc.Message, exc.StackTrace);
                return null;
            }
        }

        public static bool IsSubclassOf(string typeFullName, Type father)
        {
            Type type = Type.GetType(typeFullName, true);
            return type.IsSubclassOf(father);
        }
    }
}
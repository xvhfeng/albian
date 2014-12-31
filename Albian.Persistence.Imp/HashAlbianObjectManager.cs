#region

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Albian.Kernel.Service;
using Albian.Persistence.Imp.Parser.Impl;

#endregion

namespace Albian.Persistence.Imp
{
    [Serializable]
    public delegate string HashAlbianObjectHandler<T>(T target) where T : IAlbianObject;

    public class HashAlbianObjectManager
    {
        private static readonly Hashtable _hashAlbianObjectHandlers = Hashtable.Synchronized(new Hashtable());

        public static void RegisterHandler(string routingName,string typeFullName,HashAlbianObjectHandler<IAlbianObject> splitHandler)
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            //string typeFullName = AssemblyManager.GetFullTypeName<T>();
            if (null == splitHandler)
                return;
            string key = GetHashTableKey(routingName, typeFullName);
            if (!_hashAlbianObjectHandlers.ContainsKey(key))
            {
                _hashAlbianObjectHandlers.Add(key, splitHandler);
            }
            else
            {
                _hashAlbianObjectHandlers[key] = splitHandler;
            }
        }

        private static string GetHashTableKey(string routingName, string typeFullName)
        {
            return string.Format("{0}{1}", typeFullName, routingName);
        }
       
        public static void RegisterHandler(string typeFullName,HashAlbianObjectHandler<IAlbianObject> splitHandler)
        {
            string routingName = string.Empty;
            if (string.IsNullOrEmpty(routingName))
            {
                routingName = PersistenceParser.DefaultRoutingName;
            }
            //Type[] types = splitHandler.GetType().GetGenericArguments();
            RegisterHandler(routingName,typeFullName,splitHandler);
        }

        public static HashAlbianObjectHandler<IAlbianObject> GetHandler(string typeFullName)
        {
            string routingName = string.Empty;
            if (string.IsNullOrEmpty(routingName))
            {
                routingName = PersistenceParser.DefaultRoutingName;
            }
            //string typeFullName = AssemblyManager.GetFullTypeName<T>();
            string key = GetHashTableKey(routingName, typeFullName);
            object target = _hashAlbianObjectHandlers[key];
            if (null != target)
                return (HashAlbianObjectHandler<IAlbianObject>)target;
            return null;
        }

        public static HashAlbianObjectHandler<IAlbianObject> GetHandler(string routingName, string typeFullName)
        {
            if (string.IsNullOrEmpty(routingName))
            {
                throw new ArgumentNullException("routingName");
            }
            //string typeFullName = AssemblyManager.GetFullTypeName<T>();
            string key = GetHashTableKey(routingName, typeFullName);
            object target = _hashAlbianObjectHandlers[key];
            if (null != target)
                return (HashAlbianObjectHandler<IAlbianObject>)target;
            return null;
        }
    }
}
#region

using System;
using System.Runtime.CompilerServices;
using Albian.Kernel;
using Albian.Kernel.BasedAlgorithm;

#endregion

namespace Albian.Persistence.Imp
{
    public class AlbianObjectFactory
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static T CreateInstance<T>()
            where T :class, IAlbianObject,new()
        {
            T instance = new T {IsNew = true};
            return instance;
        }

        private static int _idx = 0;
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string CreateId()
        {
            return CreateId(string.Empty);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string CreateId(string mark)
        {
            if (10000 == _idx) _idx = 0;
            string id = string.Format("{0}{1}{2}{3:0000}", Settings.AppId, mark.PadLeft(6, '0'), DateTime.Now.ToString("yyyyMMddHHmmssffff"), _idx);
            _idx++;
            return id;
        }

        public static ulong GenerateHashCode(IAlbianObject target)
        {
            if (string.IsNullOrEmpty("target"))
            {
                throw new ArgumentNullException("target");
            }
            return Hash.GenerateCode(target.Id);
        }
    }
}
using Albian.Kernel.Service;

namespace Albian.Kernel.AlbianCache
{
    public interface IAlbianCachedService : IAlbianService
    {
        bool Exist(string groupName, string key);
        object Get(string groupName, string key);
        void Insert(string groupName, string key, object value, int seconds);
        void Set(string groupName, string key, object value, int seconds);
        void Update(string groupName, string key, object value, int seconds);
        void Remove(string groupName, string key);
        void Remove(string groupName);
    }
}
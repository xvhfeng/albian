namespace Albian.Foundation.Cached
{
    public interface IAlbianCached
    {
        bool Exist(string key);
        object Get(string key);
        void Insert(string key, object value, int seconds);
        void Set(string key, object value, int seconds);
        void Update(string key, object value, int seconds);
        void Remove(string key);
        void Remove();
    }
}
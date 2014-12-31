namespace Albian.Foundation.Chunk
{
    public interface IAlbianChunk
    {
        bool Exist(string key);
        object Get(string key);
        void Insert(string key, object value);
        void Set(string key, object value);
        void Update(string key, object value);
        void Remove(string key);
        void Remove();
    }
}
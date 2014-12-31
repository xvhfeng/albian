namespace Albian.Kernel.AlbianCache
{
    public class CacheGroup : ICacheGroup
    {
        private string _name;

        private int _size;

        private string _servers;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public string Servers
        {
            get { return _servers; }
            set { _servers = value; }
        }
    }
}
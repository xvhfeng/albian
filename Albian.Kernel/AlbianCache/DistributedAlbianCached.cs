using System;
using System.Reflection;
using Albian.Foundation.Cached;
using BeIT.MemCached;
using log4net;

namespace Albian.Kernel.AlbianCache
{
    public class DistributedAlbianCached : IAlbianCached
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string _name = null;
        private MemcachedClient _client;
        public DistributedAlbianCached(string[] servers)
            : this("DistributedAlbianCached", servers)
        {
            //_name = "DistributedAlbianCached";
            //_client = MemcachedClient.GetInstance("DistributedAlbianCached");
        }

        public DistributedAlbianCached(string name,string[] servers)
        {
            _name = name;
            MemcachedClient.Setup(name,servers);
            _client = MemcachedClient.GetInstance(name);
        }

        public virtual bool Exist(string key)
        {
            throw new NotImplementedException("not implementd the exist function for distribute cached.");
        }

        public virtual object Get(string key)
        {
            if(null != _client)
            {
                return _client.Get(key);
            }
            if(null != _logger)
            {
                _logger.Error("the distribute client is null.");
            }
            return null;
        }

        public virtual void Insert(string key, object value, int seconds)
        {
            if(null != _client)
            {
                _client.Add(key, value, DateTime.Now.AddSeconds(seconds));
            }
            if (null != _logger)
            {
                _logger.Error("the distribute client is null.");
            }
        }

        public virtual void Set(string key, object value, int seconds)
        {
            if (null != _client)
            {
                _client.Set(key, value, DateTime.Now.AddSeconds(seconds));
            }
            if (null != _logger)
            {
                _logger.Error("the distribute client is null.");
            }
        }

        public virtual void Update(string key, object value, int seconds)
        {
            if (null != _client)
            {
                _client.Replace(key, value, DateTime.Now.AddSeconds(seconds));
            }
            if (null != _logger)
            {
                _logger.Error("the distribute client is null.");
            }
        }

        public virtual void Remove(string key)
        {
            if (null != _client)
            {
                _client.Delete(key);
            }
            if (null != _logger)
            {
                _logger.Error("the distribute client is null.");
            }
        }

        public virtual void Remove()
        {
            if (null != _client)
            {
                _client.FlushAll();
            }
            if (null != _logger)
            {
                _logger.Error("the distribute client is null.");
            }
        }
    }
}
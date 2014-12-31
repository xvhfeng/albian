using System;
using System.Xml;
using Albian.Foundation;
using Albian.Kernel.Service.Impl;
using System.Collections.Generic;
using Albian.Foundation.Cached;


namespace Albian.Kernel.AlbianCache
{
    public abstract class FreeAlbianCachedService : FreeAlbianService,IAlbianCachedService,IConfigParser
    {
        private static IDictionary<string, IAlbianCached> _service;
        protected static IDictionary<string,IAlbianCached> CachedService
        {
            get
            {
                return _service;
            }
        }
        public abstract bool Exist(string groupName, string key);
        public abstract object Get(string groupName, string key);
        public abstract void Insert(string groupName, string key, object value, int seconds);
        public abstract void Set(string groupName, string key, object value, int seconds);
        public abstract void Update(string groupName, string key, object value, int seconds);
        public abstract void Remove(string groupName, string key);
        public abstract void Remove(string groupName);

        public override void  Loading()
        {
            Init("config/cache.config");
 	        base.Loading();
        }

        public void Init(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }
            try
            {
                XmlDocument doc = XmlFileParserService.LoadXml(filePath);
                XmlNodeList nodes = XmlFileParserService.Analyze(doc, "AlbianCache");
                if (1 != nodes.Count) //root node
                {
                    throw new Exception("Analyze the Objects node is error in the cached.config");
                }

                _service = ParserCached(nodes[0]);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        protected abstract IDictionary<string, IAlbianCached> ParserCached(XmlNode node);
        protected abstract IList<ICacheGroup> ParserMemcachedGroups(XmlNodeList nodes);
        protected abstract ICacheGroup ParserMemcachedGroup(XmlNode node);
        protected abstract IList<ICacheGroup> ParserLocalGroups(XmlNodeList nodes);
        protected abstract ICacheGroup ParserLocalGroup(XmlNode node);
    }
}
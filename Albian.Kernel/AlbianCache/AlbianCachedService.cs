using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Albian.Foundation.Cached;
using Albian.Kernel.Service.Impl;
using log4net;
using System;
using Albian.Foundation.Verify;
using Albian.Foundation;

namespace Albian.Kernel.AlbianCache
{
    public class AlbianCachedService : FreeAlbianCachedService
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region IAlbianCachedService ≥…‘±

        public override bool Exist(string groupName,string key)
        {
            if(!CachedService.ContainsKey(groupName))
            {
                if(null != _logger)
                {
                    _logger.WarnFormat("the grouopname:{0} is not exist", groupName);
                }
                throw new Exception("No exist the cached group name.");
            }
            return CachedService[groupName].Exist(key);
        }

        public override object Get(string groupName, string key)
        {
            if (!CachedService.ContainsKey(groupName))
            {
                if (null != _logger)
                {
                    _logger.WarnFormat("the grouopname:{0} is not exist", groupName);
                }
                throw new Exception("No exist the cached group name.");
            }
            return CachedService[groupName].Get(key);
        }

        public override void Insert(string groupName, string key, object value, int seconds)
        {
            if (!CachedService.ContainsKey(groupName))
            {
                if (null != _logger)
                {
                    _logger.WarnFormat("the grouopname:{0} is not exist", groupName);
                }
                throw new Exception("No exist the cached group name.");
            }
            CachedService[groupName].Insert(key,value,seconds);
        }

        public override void Set(string groupName, string key, object value, int seconds)
        {
            if (!CachedService.ContainsKey(groupName))
            {
                if (null != _logger)
                {
                    _logger.WarnFormat("the grouopname:{0} is not exist", groupName);
                }
                throw new Exception("No exist the cached group name.");
            }
            CachedService[groupName].Set(key, value, seconds);
        }

        public override void Update(string groupName, string key, object value, int seconds)
        {
            if (!CachedService.ContainsKey(groupName))
            {
                if (null != _logger)
                {
                    _logger.WarnFormat("the grouopname:{0} is not exist", groupName);
                }
                throw new Exception("No exist the cached group name.");
            }
            CachedService[groupName].Update(key, value, seconds);
        }

        public override void Remove(string groupName, string key)
        {
            if (!CachedService.ContainsKey(groupName))
            {
                if (null != _logger)
                {
                    _logger.WarnFormat("the grouopname:{0} is not exist", groupName);
                }
                throw new Exception("No exist the cached group name.");
            }
            CachedService[groupName].Remove(key);
        }

        public override void Remove(string groupName)
        {
            if (!CachedService.ContainsKey(groupName))
            {
                if (null != _logger)
                {
                    _logger.WarnFormat("the grouopname:{0} is not exist", groupName);
                }
                throw new Exception("No exist the cached group name.");
            }
            CachedService[groupName].Remove();
        }

        #endregion

        protected override IDictionary<string, IAlbianCached> ParserCached(XmlNode node)
        {
            IDictionary<string, IAlbianCached> dic = new Dictionary<string, IAlbianCached>();

            XmlNodeList memcacehdNodes = node.SelectNodes("Memcached/Group");
            IList<ICacheGroup> distGroups = ParserMemcachedGroups(memcacehdNodes);
            foreach (var group in distGroups)
            {
                IAlbianCached cached = new DistributedAlbianCached(group.Name, group.Servers.Split(new[] { ';' },StringSplitOptions.RemoveEmptyEntries));
                dic.Add(group.Name, cached);
            }

            XmlNodeList localNodes = node.SelectNodes("Local/Group");
            IList<ICacheGroup> localGroups = ParserLocalGroups(localNodes);
            foreach (var group in localGroups)
            {
                IAlbianCached cached = new AlbianCached(group.Name, group.Size);
                dic.Add(group.Name, cached);
            }
            return dic;
        }
        protected override IList<ICacheGroup> ParserMemcachedGroups(XmlNodeList nodes)
        {
            if(ValidateService.IsNullOrEmpty(nodes))
            {
                
            }
            IList<ICacheGroup> groups = new List<ICacheGroup>();
            foreach (XmlNode node in nodes)
            {
                ICacheGroup group = ParserMemcachedGroup(node);
                if(null == group)
                {
                    continue;
                }
                groups.Add(group);
            }
            return groups;
        }
        protected override ICacheGroup ParserMemcachedGroup(XmlNode node)
        {
            if(ValidateService.IsNull(node))
            {
                throw new ArgumentNullException("node");
            }
            object oName;
            object oServers;
            if(!XmlFileParserService.TryGetAttributeValue(node, "Name", out oName))
            {
                if(null != _logger)
                {
                    _logger.Error("no the name of cache group.");
                }
                return null;
            }
            ICacheGroup group = new CacheGroup();
            group.Name = oName.ToString();
            if (!XmlFileParserService.TryGetAttributeValue(node, "Servers", out oServers))
            {
                if (null != _logger)
                {
                    _logger.Error("no the servers of cache group.");
                }
                return null;
            }
            group.Servers = oServers.ToString();
            return group;
        }
        protected override IList<ICacheGroup> ParserLocalGroups(XmlNodeList nodes)
        {
            if (ValidateService.IsNullOrEmpty(nodes))
            {

            }
            IList<ICacheGroup> groups = new List<ICacheGroup>();
            foreach (XmlNode node in nodes)
            {
                ICacheGroup group = ParserLocalGroup(node);
                if (null == group)
                {
                    continue;
                }
                groups.Add(group);
            }
            return groups;
        }
        protected override ICacheGroup ParserLocalGroup(XmlNode node)
        {
            if (ValidateService.IsNull(node))
            {
                throw new ArgumentNullException("node");
            }
            object oName;
            object oSize;
            if (!XmlFileParserService.TryGetAttributeValue(node, "Name", out oName))
            {
                if (null != _logger)
                {
                    _logger.Error("no the name of cache group.");
                }
                return null;
            }
            ICacheGroup group = new CacheGroup();
            group.Name = oName.ToString();
            if (!XmlFileParserService.TryGetAttributeValue(node, "Size", out oSize))
            {
                if (null != _logger)
                {
                    _logger.Error("no the servers of cache group.");
                }
                return null;
            }
            group.Size = int.Parse(oSize.ToString());
            return group;
        }
    
    }
}
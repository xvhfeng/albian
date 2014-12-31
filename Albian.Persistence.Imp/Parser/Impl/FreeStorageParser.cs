#region

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Albian.Kernel;
using Albian.Kernel.Service.Impl;
using Albian.Persistence.Imp.ConnectionPool;
using Albian.Persistence.Model;
using log4net;

#endregion

namespace Albian.Persistence.Imp.Parser.Impl
{
    public abstract class FreeStorageParser : FreeAlbianService, IStorageParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region IStorageParser Members

        public void Init(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }
            try
            {
                XmlDocument doc = XmlFileParser.LoadXml(filePath);
                XmlNodeList nodes = XmlFileParser.Analyze(doc, "Storages");
                if (1 != nodes.Count) //root node
                {
                    throw new Exception("Analyze the Storages node is error in the Storage.config");
                }

                IDictionary<string, IStorageAttribute> dic = ParserStorages(nodes[0]);
                if (null == dic)
                {
                    if (null != Logger)
                        Logger.Error("no storage attribute in the config file.");
                    throw new Exception("no storage attribute in the config file.");
                }
                foreach (KeyValuePair<string, IStorageAttribute> kv in dic)
                {
                    if (kv.Value.Pooling)
                        DbConnectionPoolManager.CreatePool(kv.Value.Name, kv.Value.DatabaseStyle, kv.Value.MinPoolSize,
                                                           kv.Value.MaxPoolSize);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        #endregion

        protected abstract IDictionary<string, IStorageAttribute> ParserStorages(XmlNode node);
        protected abstract IStorageAttribute ParserStorage(XmlNode nodes);
    }
}
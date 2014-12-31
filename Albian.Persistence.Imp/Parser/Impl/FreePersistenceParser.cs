#region

using System;
using System.Collections.Generic;
using System.Xml;
using Albian.Kernel;
using Albian.Kernel.Service.Impl;
using Albian.Persistence.Model;

#endregion

namespace Albian.Persistence.Imp.Parser.Impl
{
    public abstract class FreePersistenceParser : FreeAlbianService, IPersistenceParser
    {
        #region IPersistenceParser Members

        public void Init(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath");
            }
            try
            {
                XmlDocument doc = XmlFileParser.LoadXml(filePath);
                XmlNodeList nodes = XmlFileParser.Analyze(doc, "AlbianObjects");
                if (1 != nodes.Count) //root node
                {
                    throw new Exception("Analyze the Objects node is error in the Persistence.config");
                }

                ParserObjects(nodes[0]);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        #endregion

        protected abstract IObjectAttribute ParserObject(XmlNode entityNode);

        protected abstract IMemberAttribute ParserMember(string typeFullName, XmlNode memberNode);

        protected abstract IRoutingAttribute ParserRouting(string defaultTableName, XmlNode routingNode);

        protected abstract IList<IObjectAttribute> ParserObjects(XmlNode entitiesNode);

        protected abstract IDictionary<string, IMemberAttribute> ParserMembers(string typeFullName,
                                                                               XmlNodeList memberNodes);

        protected abstract IDictionary<string, IRoutingAttribute> ParserRoutings(string defaultTableName,
                                                                                 XmlNodeList routingNodes);

        protected abstract ICacheAttribute ParserCache(XmlNode node);
    }
}
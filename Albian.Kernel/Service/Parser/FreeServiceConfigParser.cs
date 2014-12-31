using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Albian.Foundation;
using Albian.Kernel.Service.Impl;
using log4net;

namespace Albian.Kernel.Service.Parser
{
    public abstract class FreeServiceConfigParser : FreeAlbianService, IConfigParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static IDictionary<string, IAlbianServiceAttrbuite> _services;

        public static IDictionary<string, IAlbianServiceAttrbuite> ServiceConfigInfo
        {
            get { return _services; }
        }

        public void Init(string path)
        {
            XmlDocument doc = XmlFileParserService.LoadXml(path);
            if (null == doc)
            {
                if (null != Logger)
                    Logger.Error("Init the Service.config is fail.");
                throw new ServiceException("Init the Service.config is fail.");
            }
            XmlNodeList nodes = doc.SelectNodes("Services/Service");
            if (null == nodes || 0 == nodes.Count)
            {
                if (null != Logger)
                    Logger.Error("There is not 'service' items in the service.config");
                throw new ServiceException("There is not 'service' items in the service.config");
            }
            _services = ServicesParser(nodes);
        }

        public abstract IDictionary<string, IAlbianServiceAttrbuite> ServicesParser(XmlNodeList nodes);
        public abstract IAlbianServiceAttrbuite ServiceParser(XmlNode node);
    }
}
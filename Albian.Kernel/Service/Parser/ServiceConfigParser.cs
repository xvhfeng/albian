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
    public class ServiceConfigParser : FreeServiceConfigParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override IDictionary<string, IAlbianServiceAttrbuite> ServicesParser(XmlNodeList nodes)
        {
            if (null == nodes || 0 == nodes.Count)
            {
                if (null != Logger)
                    Logger.Error("There is not 'Service' config item in the service.config.");
                throw new ArgumentNullException("nodes");
            }
            IDictionary<string, IAlbianServiceAttrbuite> services = new Dictionary<string, IAlbianServiceAttrbuite>();
            foreach (XmlNode node in nodes)
            {
                IAlbianServiceAttrbuite serviceAttr = ServiceParser(node);
                services.Add(serviceAttr.Id, serviceAttr);
            }
            return services;
        }

        public override IAlbianServiceAttrbuite ServiceParser(XmlNode node)
        {
            if (null == node)
            {
                throw new ArgumentNullException("node");
            }
            object oId;
            object oType;

            if (!XmlFileParserService.TryGetAttributeValue(node, "Type", out oType))
            {
                if (null != Logger)
                    Logger.Error("There is not 'Implement' config item in the service.config.");
                throw new ServiceException("There is not 'Implement' config item in the service.config.");
            }

            if (!XmlFileParserService.TryGetAttributeValue(node, "Id", out oId))
            {
                if (null != Logger)
                    Logger.ErrorFormat("There is not 'Id' config-item with the {0} in the service.config.",oType);
                throw new ServiceException(string.Format("There is not 'Id' config-item with the {0} in the service.config.", oType));
            }
            IAlbianServiceAttrbuite serviceAttr = new AlbianServiceAttrbuite
                                                      {
                                                          Id = oId.ToString(),
                                                          Type = oType.ToString(),
                                                      };
            return serviceAttr;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Collections;
using Albian.Foundation;
using Albian.Kernel.Service.Parser;
using log4net;

namespace Albian.Kernel.Service
{
    [AlbianKernel]
    public class AlbianServiceRouter
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public static T GetService<T>(string serviceId)
           where T : class, IAlbianService
        {
            object service = AlbianBootService.AlbianService.Get(serviceId);
            if (null == service)
            {
                if (null != Logger)
                    Logger.WarnFormat("The {0} service is null.", serviceId);
                return null;
            }
            return (T)service;

        }

        public static T GetService<T>(string serviceId,bool isNew)
            where T : class, IAlbianService
        {
            if (!isNew)
                return GetService<T>(serviceId);

            IDictionary<string, IAlbianServiceAttrbuite> serviceInfos = FreeServiceConfigParser.ServiceConfigInfo;
            if (serviceInfos.ContainsKey(serviceId))
            {
                if (null != Logger)
                    Logger.WarnFormat("There is not {0} serice info.", serviceId);
                return null;
            }
            IAlbianServiceAttrbuite serviceInfo = serviceInfos[serviceId];
            Type type = Type.GetType(serviceInfo.Type);
            IAlbianService service = (IAlbianService)Activator.CreateInstance(type);
            service.BeforeLoading();
            service.Loading();
            service.AfterLoading();
            return (T)service;
        }

        
    }
}
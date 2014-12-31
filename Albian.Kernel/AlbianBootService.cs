using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using Albian.Foundation.Chunk;
using Albian.Kernel.Service;
using Albian.Kernel.Service.Parser;
using log4net;
using System.Threading;

namespace Albian.Kernel
{
    [AlbianKernel]
    public class AlbianBootService
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static AlbianState _state = AlbianState.UnStart;

        private static IAlbianChunk _service;
        internal static IAlbianChunk AlbianService
        {
            get { return _service; }
        }

        public static void Start()
        {
            new Thread(
                delegate()
                {
                    _state = AlbianState.Starting;
                    _service = new AlbianChunk("AlbianService");
                    IConfigParser parser = new ServiceConfigParser();
                    parser.Init("config/service.config");
                    IDictionary<string, IAlbianServiceAttrbuite> serviceInfos = FreeServiceConfigParser.ServiceConfigInfo;
                    bool isSuccess = true;
                    IDictionary<string, IAlbianServiceAttrbuite> failServicesInfos = new Dictionary<string, IAlbianServiceAttrbuite>();
                    int failCountBeforeTimes = 0;
                    while (true)
                    {
                        isSuccess = true;
                        if (0 != failServicesInfos.Count)
                        {
                            if (failCountBeforeTimes == failServicesInfos.Count)
                            {
                                if (null != Logger)
                                {
                                    Logger.ErrorFormat("Refer to each other when service loading!");
                                    foreach (KeyValuePair<string, IAlbianServiceAttrbuite> kv in failServicesInfos)
                                    {
                                        Logger.ErrorFormat("Refer to each other!id:{0},Type:{1}", kv.Value.Id, kv.Value.Type);
                                    }
                                    Logger.Error("Please examine the service id above the line!");
                                }
                                _state = AlbianState.UnLoading;
                                throw new ServiceException("Refer to each other!");
                            }

                            failCountBeforeTimes = failServicesInfos.Count;
                            serviceInfos.Clear();
                            foreach (KeyValuePair<string, IAlbianServiceAttrbuite> kv in failServicesInfos)
                            {
                                serviceInfos.Add(kv.Key, kv.Value);
                            }
                            failServicesInfos.Clear();
                        }

                        foreach (KeyValuePair<string, IAlbianServiceAttrbuite> kv in serviceInfos)
                        {
                            try
                            {
                                Type impl = Type.GetType(kv.Value.Type);
                                IAlbianService service = (IAlbianService)Activator.CreateInstance(impl);
                                service.State = ServiceState.Loading;
                                service.Loading();
                                service.State = ServiceState.Running;
                                _service.Set(kv.Key, service);
                            }
                            catch
                            {
                                isSuccess = false;
                                failServicesInfos.Add(kv.Key, kv.Value);
                            }
                        }
                        if (isSuccess)
                        {
                            _state = AlbianState.Running;
                            break;
                        }
                    }
                }
                ).Start();
        }

        public static void RequestHandler()
        {
            if (AlbianState.Running == _state)
            {
                return;
            }
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write("Albian is not ready,Please wait a minute or contact administrators!");
            HttpContext.Current.Response.End();
        }
    }
}

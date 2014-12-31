using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Albian.Kernel;
using log4net;
using log4net.Config;
using System.Threading;

namespace AllTest.Web
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                AlbianBootService.Start();
                if (null != _logger)
                    _logger.Info("应用程序启动！");
            }
            catch (Exception exc)
            {
                if (null != _logger)
                    _logger.Error(exc.Message);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            AlbianBootService.RequestHandler();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
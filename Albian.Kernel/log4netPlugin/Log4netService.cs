using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Albian.Kernel.Service.Impl;
using log4net.Config;
using Albian.Foundation;

namespace Albian.Kernel.log4netPlugin
{
    public class Log4netService : FreeAlbianService
    {
        public override void Loading()
        {
            XmlConfigurator.Configure(new FileInfo(PathService.GetFullPath(@"config/log4net.config"))); 
            base.Loading();
        }
    }
}

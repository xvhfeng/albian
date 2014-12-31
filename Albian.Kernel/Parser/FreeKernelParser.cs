using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Albian.Foundation;
using Albian.Kernel.Service.Impl;
using log4net;

namespace Albian.Kernel.Parser.Impl
{
    public abstract class FreeKernelParser : FreeAlbianService, IConfigParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Init(string path)
        {
            PropertiesFileParserService service = new PropertiesFileParserService(path);
            ParserAppId(service);
        }

        protected abstract void ParserAppId(PropertiesFileParserService service);
       
    }
}
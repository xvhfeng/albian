using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Albian.Foundation;

namespace Albian.Kernel.Parser.Impl
{
    public class KernelParser : FreeKernelParser
    {
        protected override void ParserAppId(PropertiesFileParserService service)
        {
            KernelSettings.AppId = service.GetValue("AppId");
            KernelSettings.AppName = service.GetValue("AppName");
            KernelSettings.Key = service.GetValue("Key");
            return;
        }

        public override void Loading()
        {
            Init(@"config/kernel.properties");
            base.Loading();
        }
    }
}
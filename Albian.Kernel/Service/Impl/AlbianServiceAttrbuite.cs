using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albian.Kernel.Service.Impl
{
    public class AlbianServiceAttrbuite : IAlbianServiceAttrbuite
    {
        private string _id = string.Empty;
        private string _implement = string.Empty;
        private string _interface = string.Empty;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Implement
        {
            get { return _implement; }
            set { _implement = value; }
        }

        public string Interface
        {
            get { return _interface; }
            set { _interface = value; }
        }
    }
}
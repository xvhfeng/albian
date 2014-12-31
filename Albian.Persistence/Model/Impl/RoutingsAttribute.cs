using System.Collections.Generic;

namespace Albian.Persistence.Model.Impl
{
    public class RoutingsAttribute : IRoutingsAttribute
    {
        #region IRoutingsAttribute ≥…‘±

        public string AlbianObject { get; set; }

        public IRoutingMapping RoutingMapping
        {
            get;
            set;
        }
        public IDictionary<string, IRoutingAttribute> WriterRoutings { get; set; }

        public IDictionary<string, IRoutingAttribute> ReaderRoutings { get; set; }

        #endregion
    }
}
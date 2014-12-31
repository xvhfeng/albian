using System.Collections.Generic;

namespace Albian.Persistence.Model
{
    public interface IRoutingsAttribute
    {
        string AlbianObject
        { get; set;}

        IRoutingMapping RoutingMapping
        {
            get;
            set;
        }

        IDictionary<string, IRoutingAttribute> WriterRoutings
        {
            get;
            set;
        }

        IDictionary<string, IRoutingAttribute> ReaderRoutings
        {
            get;
            set;
        }
    }
}
using System.Collections.Generic;
using Albian.Persistence.Model;

namespace Albian.Persistence
{
    public interface IRoutingMapping
    {
        IDictionary<string, IRoutingAttribute> MappingWriterRouting(IDictionary<string, IRoutingAttribute> writerRoutings, IAlbianObject albianObject);

        IRoutingAttribute MappingReaderRouting(IDictionary<string, IRoutingAttribute> readerRoutings, IFilterCondition[] where, IOrderByCondition[] orderby);

        string MappingWriterTable(IRoutingAttribute routing, IAlbianObject albianObject);

        string MappingReaderTable(IRoutingAttribute routing, IFilterCondition[] where, IOrderByCondition[] orderby);
    }
}
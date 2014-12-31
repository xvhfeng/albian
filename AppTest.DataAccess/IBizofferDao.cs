using System.Collections.Generic;
using Albian.Persistence;
using AppTest.Model;
using AppTest.Model.Imp;

namespace AppTest.DataAccess
{
    public interface IBizofferDao
    {
        bool Create(IList<IAlbianObject> bizoffer);
        bool Modify(IList<IAlbianObject> bizoffer);
        IBizOffer Load(string routerName,string id);
        IBizOffer Find(string routerName,string id);
        IList<BizOffer> FindBizoffer();
    }
}
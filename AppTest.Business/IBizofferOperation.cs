using System.Collections.Generic;
using Albian.Kernel.Service;
using AppTest.Model;
using AppTest.Model.Imp;

namespace AppTest.Business
{
    public interface IBizofferOperation : IAlbianService
    {
        IList<BizOffer> FindBizoffer();
        bool Create(IBizOffer bizoffer);
        bool Modify(IBizOffer bizoffer);
        IBizOffer FindBizOffer(string id);
        IBizOffer LoadBizOffer(string id);
    }
}
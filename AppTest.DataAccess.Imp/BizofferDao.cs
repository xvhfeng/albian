using System;
using System.Collections.Generic;
using Albian.Persistence;
using Albian.Persistence.Imp;
using Albian.Persistence.Imp.Model;
using AppTest.Model;
using AppTest.Model.Imp;

namespace AppTest.DataAccess.Imp
{
    public class BizofferDao :IBizofferDao
    {

        public bool Create(IList<IAlbianObject> bizoffer)
        {
           return PersistenceService.Save(bizoffer);
        }

        public virtual bool Modify(IList<IAlbianObject> bizoffer)
        {
            return PersistenceService.Modify(bizoffer);
        }

        public IBizOffer Load(string routerName,string id)
        {
            return PersistenceService.LoadObject<BizOffer>(routerName,id);
        }

        public virtual IBizOffer Find(string routerName,string id)
        {
            return PersistenceService.FindObject<BizOffer>(routerName,id);
        }

        public IList<BizOffer> FindBizoffer()
        {
            return PersistenceService.FindObjects<BizOffer>("CreateTimeRouting",new OrderByCondition[]{
            new OrderByCondition()
            {
                PropertyName = "CreateTime",
                SortStyle = Albian.Persistence.Enum.SortStyle.Desc,
            }
            });
        }


   }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Albian.Kernel.Service;
using Albian.Kernel.Service.Impl;
using Albian.Persistence;
using AppTest.DataAccess;
using AppTest.DataAccess.Imp;
using AppTest.Model;
using AppTest.Model.Imp;
using Albian.Persistence.Imp;

namespace AppTest.Business.Imp
{
    public class BizofferOperation :FreeAlbianService, IBizofferOperation
    {
        #region Implementation of IBizofferOperation

        public override void Loading()
        {

            HashAlbianObjectManager.RegisterHandler(AssemblyManager.GetFullTypeName(typeof(LogInfo)), HashAlbianObjectHandlerLog);
            base.Loading();
        }
        private string HashAlbianObjectHandlerLog(IAlbianObject target)
        {
            ILogInfo user = (ILogInfo)target;
            return string.Format("_{0}", user.Style == InfoStyle.Publish ? "bizoffer" : string.Empty);
        }


        public IList<BizOffer> FindBizoffer()
        {
            IBizofferDao dao = AlbianServiceRouter.ObjectGenerator<BizofferDao, IBizofferDao>();
            return dao.FindBizoffer();
        }

        public bool Create(IBizOffer bizoffer)
        {
            ILogInfo log = AlbianObjectFactory.CreateInstance<LogInfo>();
            log.Content = string.Format("创建发布单，发布单id为:{0}", bizoffer.Id);
            log.CreateTime = DateTime.Now;
            log.Creator = bizoffer.Id;
            log.Id = AlbianObjectFactory.CreateId("Log");
            log.Style = InfoStyle.Publish;

            IList<IAlbianObject> list = new List<IAlbianObject> {bizoffer, log};
            IBizofferDao dao = AlbianServiceRouter.ObjectGenerator<BizofferDao, IBizofferDao>();
            return dao.Create(list);
        }

        public virtual bool Modify(IBizOffer bizoffer)
        {
            ILogInfo log = AlbianObjectFactory.CreateInstance<LogInfo>();
            log.Content = string.Format("修改发布单，发布单id为:{0}", bizoffer.Id);
            log.CreateTime = DateTime.Now;
            log.Creator = bizoffer.Id;
            log.Id = AlbianObjectFactory.CreateId("Log");
            log.Style = InfoStyle.Publish;

            IList<IAlbianObject> list = new List<IAlbianObject> { bizoffer, log };
            IBizofferDao dao = AlbianServiceRouter.ObjectGenerator<BizofferDao, IBizofferDao>();
            return dao.Modify(list);
        }

        public virtual IBizOffer FindBizOffer(string id)
        {
            IBizofferDao dao = AlbianServiceRouter.ObjectGenerator<BizofferDao, IBizofferDao>();
            return dao.Find("IdRouting", id);
            
        }

        public virtual IBizOffer LoadBizOffer(string id)
        {
            IBizofferDao dao = AlbianServiceRouter.ObjectGenerator<BizofferDao, IBizofferDao>();
            return dao.Load("IdRouting", id);
        }

        #endregion
    }
}
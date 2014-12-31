using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Albian.Kernel;
using Albian.Kernel.Service.Impl;
using Albian.Persistence;
using Albian.Persistence.Imp;
using AppTest.DataAccess;
using AppTest.DataAccess.Imp;
using AppTest.Model;
using AppTest.Model.Imp;
using Albian.Kernel.Service;

namespace AppTest.Business.Imp
{
    public class UserOperation :FreeAlbianService,IUserOperation
    {

        public override void Loading()
        {
            HashAlbianObjectManager.RegisterHandler("1thRouting", AssemblyManager.GetFullTypeName(typeof(User)), HashAlbianObjectHandlerUser);
            HashAlbianObjectManager.RegisterHandler( AssemblyManager.GetFullTypeName(typeof(LogInfo)), HashAlbianObjectHandlerByCreatrUser);
            base.Loading();
        }

        private string HashAlbianObjectHandlerUser(IAlbianObject target)
        {
            return string.Format("_{0}", Math.Abs(target.Id.GetHashCode() % 3));
        }
        private string HashAlbianObjectHandlerByCreatrUser(IAlbianObject target)
        {
            ILogInfo user = (ILogInfo)target;
            return string.Format("_{0}", user.Style == InfoStyle.Registr || user.Style == InfoStyle.Modify ? "user":string.Empty);
        }



        public bool Create(IUser user)
        {
            IUserDao dao = AlbianServiceRouter.ObjectGenerator<UserDao,IUserDao>();
            ILogInfo log = AlbianObjectFactory.CreateInstance<LogInfo>();
            log.Content = string.Format("创建用户，用户id为:{0}", user.Id);
            log.CreateTime = DateTime.Now;
            log.Creator = user.Id;
            log.Id = AlbianObjectFactory.CreateId("Log");
            log.Style = InfoStyle.Registr;
            IList<IAlbianObject> infos = new List<IAlbianObject> {user, log};
            return dao.Create(infos);
        }

        public bool Modify(string id,string nickName)
        {
            IUserDao dao = AlbianServiceRouter.ObjectGenerator<UserDao, IUserDao>();
            IUser user = dao.Load(id);
            user.Nickname = nickName;
            user.LastMofidyTime = DateTime.Now;
            user.LastModifier = id;

            ILogInfo log = AlbianObjectFactory.CreateInstance<LogInfo>();
            log.Content = string.Format("修改用户，用户id为:{0}", user.Id);
            log.CreateTime = DateTime.Now;
            log.Creator = user.Id;
            log.Id = AlbianObjectFactory.CreateId("Log");
            log.Style = InfoStyle.Modify;
            IList<IAlbianObject> infos = new List<IAlbianObject> { user, log };
            return dao.Modify(infos);
        }
    }
}

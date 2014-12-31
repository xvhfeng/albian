using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Albian.Kernel.Service.Impl;
using Albian.Persistence;
using Albian.Persistence.Imp;
using AppTest.Model;
using AppTest.Model.Imp;

namespace AppTest.DataAccess.Imp
{
    public class UserDao :IUserDao
    {
        #region Implementation of IUserDao

        public virtual bool Create(IList<IAlbianObject> userInfo)
        {
            return PersistenceService.Create(userInfo);
        }

        public virtual bool Modify(IList<IAlbianObject> userInfo)
        {
            return PersistenceService.Save(userInfo);
        }

        public virtual bool Delete(IList<IAlbianObject> userInfo)
        {
            return PersistenceService.Remove(userInfo);
        }

        public virtual IUser Find(string userId)
        {
            return PersistenceService.FindObject<User>("1thRouting", userId);
        }

        public virtual IUser Load(string userId)
        {
            return PersistenceService.LoadObject<User>("1thRouting", userId);
        }

        #endregion
    }
}

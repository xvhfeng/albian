using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Albian.Kernel.Service;
using Albian.Persistence;
using AppTest.Model;

namespace AppTest.DataAccess
{
    public interface IUserDao
    {
        bool Create(IList<IAlbianObject> userInfo);
        bool Modify(IList<IAlbianObject> userInfo);
        bool Delete(IList<IAlbianObject> userInfo);
        IUser Find(string userId);
        IUser Load(string userId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Albian.Kernel.Service;
using AppTest.Model;

namespace AppTest.Business
{
    public interface IUserOperation : IAlbianService
    {
        bool Create(IUser user);
        bool Modify(string id, string nickName);
    }
}

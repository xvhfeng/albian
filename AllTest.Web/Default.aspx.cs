using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Albian.Kernel;
using Albian.Kernel.Service.Impl;
using AppTest.Business;
using AppTest.Model;
using Albian.Persistence.Imp;
using AppTest.Model.Imp;

namespace AllTest.Web
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            IUser user = AlbianObjectFactory.CreateInstance<User>();
            user.Id = AlbianObjectFactory.CreateId("User");
            user.CreateTime = DateTime.Now;
            user.Creator = user.Id;
            user.LastModifier = user.Id;
            user.LastMofidyTime = DateTime.Now;
            user.Mail = txtMail.Text;
            user.Mobile = txtMobile.Text;
            user.Nickname = txtNickName.Text;
            user.Password = txtPassword.Text;
            user.RegistrDate = DateTime.Now;
            user.UserName = txtUserName.Text;
            bool isSuccess = AlbianServiceRouter.GetService<IUserOperation>().Create(user);
            txtID.Text = user.Id;

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            bool isSuccess = AlbianServiceRouter.GetService<IUserOperation>().Modify(txtID.Text,txtNickName.Text);
        }
    }
}

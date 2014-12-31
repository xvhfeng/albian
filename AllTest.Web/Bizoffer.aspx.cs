using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Albian.Kernel.Service.Impl;
using Albian.Persistence.Imp;
using AppTest.Business;
using AppTest.Model;
using AppTest.Model.Imp;
using System.Diagnostics;

namespace AllTest.Web
{
    public partial class Bizoffer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IList<BizOffer> bizoffers = AlbianServiceRouter.GetService<IBizofferOperation>().FindBizoffer();
                gv.DataSource = bizoffers;
                gv.DataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IBizOffer bizoffer = AlbianObjectFactory.CreateInstance<BizOffer>();
            bizoffer.Id = AlbianObjectFactory.CreateId("BOFF");
            bizoffer.CreateTime = DateTime.Now;
            bizoffer.Creator = txtSellerId.Text;
            bizoffer.Description = txtDesc.Text;
            bizoffer.Discount = null;
            bizoffer.IsDiscount = null;
            bizoffer.LastModifier = txtSellerId.Text;
            bizoffer.LastModifyTime = DateTime.Now;
            bizoffer.LastPrice = decimal.Parse(txtPrice.Text);
            bizoffer.Name = txtName.Text;
            bizoffer.Price = decimal.Parse(txtPrice.Text);
            bizoffer.SellerId = txtSellerId.Text;
            bizoffer.SellerName = txtSellerName.Text;
            bizoffer.State = BizofferState.Create;
            if (AlbianServiceRouter.GetService<IBizofferOperation>().Create(bizoffer))
            {
                txtId.Text = bizoffer.Id;
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Save",
                                                       "<script language=\"javascript\" type=\"text/javascript\">alert(\"Create Success!\");</script>");
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Save",
                                                       "<script language=\"javascript\" type=\"text/javascript\">alert(\"Create Fail!\");</script>");
            }
            sw.Stop();
            Response.Write(sw.ElapsedMilliseconds);


        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            IBizOffer bizoffer = AlbianServiceRouter.GetService<IBizofferOperation>().LoadBizOffer(txtId.Text);
            bizoffer.Creator = txtSellerId.Text;
            bizoffer.Description = txtDesc.Text;
            bizoffer.Discount = null;
            bizoffer.IsDiscount = null;
            bizoffer.LastModifier = txtSellerId.Text;
            bizoffer.LastModifyTime = DateTime.Now;
            bizoffer.LastPrice = decimal.Parse(txtPrice.Text);
            bizoffer.Name = txtName.Text;
            bizoffer.Price = decimal.Parse(txtPrice.Text);
            bizoffer.SellerId = txtSellerId.Text;
            bizoffer.SellerName = txtSellerName.Text;
            bizoffer.State = BizofferState.Create;
            if (AlbianServiceRouter.GetService<IBizofferOperation>().Modify(bizoffer))
            {
                txtId.Text = bizoffer.Id;
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Save",
                                                       "<script language=\"javascript\" type=\"text/javascript\">alert(\"Modify Success!\");</script>");
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Save",
                                                       "<script language=\"javascript\" type=\"text/javascript\">alert(\"Modify Fail!\");</script>");
            }

        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            IBizOffer bizoffer = AlbianServiceRouter.GetService<IBizofferOperation>().FindBizOffer(txtId.Text);
            txtDesc.Text = bizoffer.Description;
            txtId.Text = bizoffer.Id;
            txtName.Text = bizoffer.Name;
            txtPrice.Text = bizoffer.Price.ToString();
            txtSellerId.Text = bizoffer.SellerId;
            txtSellerName.Text = bizoffer.SellerName;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            IBizOffer bizoffer = AlbianServiceRouter.GetService<IBizofferOperation>().LoadBizOffer(txtId.Text);
            txtDesc.Text = bizoffer.Description;
            txtId.Text = bizoffer.Id;
            txtName.Text = bizoffer.Name;
            txtPrice.Text = bizoffer.Price.ToString();
            txtSellerId.Text = bizoffer.SellerId;
            txtSellerName.Text = bizoffer.SellerName;

        }
    }
}

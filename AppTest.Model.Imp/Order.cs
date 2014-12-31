using System;
using Albian.Persistence.Model.Impl;

namespace AppTest.Model.Imp
{
    public class Order : FreeAlbianObject,IOrder
    {
        #region Implementation of IOrder

        private string _bizofferId;

        private string _buyerId;

        private string _buyerName;

        private string _seller;

        private decimal _price;

        private OrderState _state;

        private string _creator;

        private DateTime _lastModifyTime;

        private string _lastModifier;

        private DateTime _createTime;

        public string BizofferId
        {
            get { return _bizofferId; }
            set { _bizofferId = value; }
        }

        public string BuyerId
        {
            get { return _buyerId; }
            set { _buyerId = value; }
        }

        public string BuyerName
        {
            get { return _buyerName; }
            set { _buyerName = value; }
        }

        public string Seller
        {
            get { return _seller; }
            set { _seller = value; }
        }

        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public OrderState State
        {
            get { return _state; }
            set { _state = value; }
        }

        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }

        public DateTime LastModifyTime
        {
            get { return _lastModifyTime; }
            set { _lastModifyTime = value; }
        }

        public string LastModifier
        {
            get { return _lastModifier; }
            set { _lastModifier = value; }
        }

        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        #endregion
    }
}
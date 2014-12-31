using System;
using Albian.Persistence;

namespace AppTest.Model
{
    public interface IOrder : IAlbianObject
    {
        string BizofferId { get; set; }
        string BuyerId { get; set; }
        string BuyerName { get; set; }
        string Seller { get; set; }
        decimal Price { get; set; }
        OrderState State { get; set; }
        string Creator { get; set; }
        DateTime LastModifyTime { get; set; }
        string LastModifier { get; set; }
        DateTime CreateTime { get; set; }
    }
}
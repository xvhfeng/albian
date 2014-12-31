using System;
using Albian.Persistence;

namespace AppTest.Model
{
    public interface IBizOffer : IAlbianObject
    {
        string Name { get; set; }

        string SellerId { get; set; }
        string SellerName { get; set; }

        DateTime CreateTime { get; set; }
        /// <summary>
        /// 发布单状态
        /// </summary>
        BizofferState State { get; set; }
        /// <summary>
        /// 原始价格
        /// </summary>
        decimal Price { get; set; }

        /// <summary>
        /// 是否打折
        /// </summary>
        bool? IsDiscount { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        decimal? Discount { get; set; }
        /// <summary>
        /// 交易价格
        /// </summary>
        decimal LastPrice { get; set; }

        string Creator { get; set; }

        DateTime LastModifyTime { get; set; }

        string LastModifier { get; set; }
        /// <summary>
        /// 发布单产品描述
        /// </summary>
        string Description { get; set; }
    }
}
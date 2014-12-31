using System;
using Albian.Persistence;

namespace AppTest.Model
{
    public interface ILogInfo : IAlbianObject
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        InfoStyle Style { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        string Creator { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        string Content { get; set; }
        /// <summary>
        /// 日志备注
        /// </summary>
        string Remark { get; set; }
    }
}
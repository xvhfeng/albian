using System;
using Albian.Persistence;

namespace AppTest.Model
{
    public interface IUser : IAlbianObject
    {
        string UserName { get; set; }
        string Password { get; set; }
        /// <summary>
        /// 注册日期
        /// </summary>
        DateTime RegistrDate { get; set; }
        DateTime CreateTime { get; set; }
        DateTime LastMofidyTime { get; set; }
        string Creator { get; set; }
        string LastModifier { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        string Nickname { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        string Mobile { get; set; }
        string Mail { get; set; }
    }
}
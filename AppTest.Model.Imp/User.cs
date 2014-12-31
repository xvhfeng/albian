using System;
using Albian.Persistence.Model.Impl;

namespace AppTest.Model.Imp
{
    public class User : FreeAlbianObject, IUser
    {
        #region Implementation of IUser

        private string _userName;

        private string _password;

        private DateTime _registrDate;

        private DateTime _createTime;

        private DateTime _lastMofidyTime;

        private string _creator;

        private string _lastModifier;

        private string _nickname;

        private string _mobile;

        private string _mail;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime RegistrDate
        {
            get { return _registrDate; }
            set { _registrDate = value; }
        }

        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        public DateTime LastMofidyTime
        {
            get { return _lastMofidyTime; }
            set { _lastMofidyTime = value; }
        }

        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }

        public string LastModifier
        {
            get { return _lastModifier; }
            set { _lastModifier = value; }
        }

        /// <summary>
        /// 昵称
        /// </summary>
        [AlbianMember(Length=20)]
        public string Nickname
        {
            get { return _nickname; }
            set { _nickname = value; }
        }

        /// <summary>
        /// 手机
        /// </summary>
        [AlbianMember(Length=15)]
        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

        public string Mail
        {
            get { return _mail; }
            set { _mail = value; }
        }

        #endregion
    }
}
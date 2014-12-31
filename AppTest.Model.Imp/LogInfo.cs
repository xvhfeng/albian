using System;
using Albian.Persistence.Model.Impl;

namespace AppTest.Model.Imp
{
    public class LogInfo : FreeAlbianObject,ILogInfo
    {
        #region Implementation of ILogInfo

        private InfoStyle _style;

        private DateTime _createTime;

        private string _creator;

        private string _content;

        private string _remark;

        /// <summary>
        /// 日志类型
        /// </summary>
        public InfoStyle Style
        {
            get { return _style; }
            set { _style = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>
        /// 日志备注
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        #endregion
    }
}
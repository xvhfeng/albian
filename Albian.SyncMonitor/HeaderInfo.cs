namespace FastDFS.Client.Component
{
    /// <summary>
    /// 头部信息
    /// </summary>
    public class HeaderInfo
    {
        private long _length;

        /// <summary>
        /// 得到或者设置头部的长度.
        /// </summary>
        /// <value>头部内容长度.</value>
        public long Length
        {
            set { _length = value; }
            get { return _length; }
        }

        private byte _errorNo;

        /// <summary>
        /// 得到或者设置错误号.
        /// </summary>
        /// <value>错误号.</value>
        public byte ErrorNo
        {
            set { _errorNo = value; }
            get { return _errorNo; }
        }

        /// <summary>
        /// 初始化 <see cref="HeaderInfo"/> 对象.
        /// </summary>
        /// <param name="errorNo">错误号.</param>
        /// <param name="length">头内容长度.</param>
        public HeaderInfo(byte errorNo, long length)
        {
            _errorNo = errorNo;
            _length = length;
        }
    }
}
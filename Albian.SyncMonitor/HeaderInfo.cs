namespace FastDFS.Client.Component
{
    /// <summary>
    /// ͷ����Ϣ
    /// </summary>
    public class HeaderInfo
    {
        private long _length;

        /// <summary>
        /// �õ���������ͷ���ĳ���.
        /// </summary>
        /// <value>ͷ�����ݳ���.</value>
        public long Length
        {
            set { _length = value; }
            get { return _length; }
        }

        private byte _errorNo;

        /// <summary>
        /// �õ��������ô����.
        /// </summary>
        /// <value>�����.</value>
        public byte ErrorNo
        {
            set { _errorNo = value; }
            get { return _errorNo; }
        }

        /// <summary>
        /// ��ʼ�� <see cref="HeaderInfo"/> ����.
        /// </summary>
        /// <param name="errorNo">�����.</param>
        /// <param name="length">ͷ���ݳ���.</param>
        public HeaderInfo(byte errorNo, long length)
        {
            _errorNo = errorNo;
            _length = length;
        }
    }
}
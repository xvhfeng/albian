using Albian.Kernel.Service.Impl;

namespace Albian.Security.Impl
{
    public abstract class FreeEncrypt :FreeAlbianService, IEncrypt
    {
        #region IEncrypt Members

        public abstract string EncryptString(string value);
        public abstract string EncryptString(string value, int deepth);
        public abstract string DecryptString(string value);
        public abstract string DecryptString(string value, int deepth);
        public abstract string MD5(string input, MD5Mode bit, bool isUpper);
        public abstract string MD5(string input, MD5Mode bit);
        public abstract string SHA1(string value);

        #endregion

        protected abstract void GetKey();
    }
}
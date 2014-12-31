namespace Albian.Security.Imp
{
    public abstract class AbstractEncrypt : IEncrypt
    {
        #region IEncrypt Members

        public abstract string EncryptString(string value);
        public abstract string EncryptString(string value, int deepth);
        public abstract string DecryptString(string value);
        public abstract string DecryptString(string value, int deepth);

        #endregion

        protected abstract void GetKey();
    }
}
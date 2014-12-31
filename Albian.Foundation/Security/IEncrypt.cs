namespace Albian.Foundation.Security
{
    public interface IEncrypt
    {
        string EncryptString(string value);
        string EncryptString(string value, int deepth);
        string DecryptString(string value);
        string DecryptString(string value, int deepth);
        string MD5(string input, MD5Mode bit, bool isUpper);
        string MD5(string input, MD5Mode bit);
        string SHA1(string value);
    }
}
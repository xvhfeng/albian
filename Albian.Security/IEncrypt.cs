using Albian.Kernel.Service;

namespace Albian.Security
{
    public interface IEncrypt : IAlbianService
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
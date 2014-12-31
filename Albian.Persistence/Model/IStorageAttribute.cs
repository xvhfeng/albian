#region

using Albian.Persistence.Enum;

#endregion

namespace Albian.Persistence.Model
{
    public interface IStorageAttribute
    {
        string Name { get; set; }
        string Server { get; set; }
        string Database { get; set; }
        string Uid { get; set; }
        string Password { get; set; }
        int MinPoolSize { get; set; }
        int MaxPoolSize { get; set; }
        int Timeout { get; set; }
        bool Pooling { get; set; }
        bool IntegratedSecurity { get; set; }
        DatabaseStyle DatabaseStyle { get; set; }
        string Charset { get; set; }
        bool Transactional { get; set; }
        bool IsHealth { get; set; }
    }
}
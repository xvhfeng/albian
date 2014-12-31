using Albian.Kernel.Service;

namespace Albian.Persistence.Imp.Notify
{
    /// <summary>
    /// 数据库连接异常时通知机制
    /// </summary>
    public interface IConnectionNotify : IAlbianService
    {
        void SendMessage(string msg);
    }
}
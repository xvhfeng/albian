namespace Albian.Persistence.Model
{
    public interface ICacheAttribute
    {
        /// <summary>
        /// 是否启用缓存
        /// </summary>
        bool Enable { get; set; }

        /// <summary>
        /// 缓存时间
        /// </summary>
        int LifeTime { get; set; }
    }
}
namespace Albian.Persistence.Model.Impl
{
    public class CacheAttribute : ICacheAttribute
    {
        #region Implementation of ICacheAttribute

        private bool _enable = true;

        private int _lifeTime = 300;

        /// <summary>
        /// 是否启用缓存
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        /// 缓存时间
        /// </summary>
        public int LifeTime
        {
            get { return _lifeTime; }
            set { _lifeTime = value; }
        }

        #endregion
    }
}
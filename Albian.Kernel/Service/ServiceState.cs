using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albian.Kernel.Service
{
    public enum ServiceState
    {
        /// <summary>
        /// 未启动
        /// </summary>
        Normal,
        /// <summary>
        /// 正在加载
        /// </summary>
        Loading,
        /// <summary>
        /// 运行中
        /// </summary>
        Running,
        /// <summary>
        /// 正在卸载
        /// </summary>
        Unloading,
        /// <summary>
        /// 卸载完毕
        /// </summary>
        Unloaded,
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Albian.Foundation.ThreadPool
{
    internal struct WorkItem //值类型
    {
        internal readonly object _arg;
        internal readonly WaitCallback _callback;
        internal ExecutionContext _context;

        internal WorkItem(WaitCallback work, object obj)
        {
            _callback = work;
            _arg = obj;
            _context = null;
        }

        /// <summary>
        /// 调用代理方法
        /// </summary>
        internal void Invoke()
        {
            if (_context == null)
                _callback(_arg);
            else
                ExecutionContext.Run(_context, ContextInvoke, null);
        }

        /// <summary>
        /// 执行代理方法
        /// </summary>
        /// <param name="obj"></param>
        private void ContextInvoke(object obj)
        {
            _callback(_arg);
        }
    }
}
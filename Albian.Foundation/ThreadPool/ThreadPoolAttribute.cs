using System;
using System.Collections.Generic;
using System.Text;

namespace Albian.Foundation.ThreadPool
{
    public class ThreadPoolAttribute
    {
        private int _size = 15;
        private bool _flowExecutionContext = true;
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public bool FlowExecutionContext
        {
            get { return _flowExecutionContext; }
            set { _flowExecutionContext = value; }
        }
    }
}
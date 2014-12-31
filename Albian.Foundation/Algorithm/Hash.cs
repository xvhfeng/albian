using System;

namespace Albian.Foundation.BasedAlgorithm
{
    public class Hash
    {
        public static ulong GenerateCode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            int len = value.Length;
            ulong llen = (ulong)len;
            int step = (len >> 5) + 1;
            for (int i = len; i >= step; i -= step)
                llen = llen ^ ((llen << 5) + (llen >> 2) + value[i - 1]);
            return llen;
        }
    }
}
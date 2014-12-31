using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Albian.Persistence;

namespace Albian.SyncMonitor
{
    public static class MutexMonitor
    {
        public static void Enter(string locker)
        {
        }

        public static void Enter(string[] lockers)
        {

        }

        public static void Exit(string locker)
        {
        }

        public static void Exit(string[] lockers)
        {
        }

        public static bool TryEnter(string locker)
        {
            return true;
        }

        public static bool TryEnter(string[] lockers)
        {
            return true;
        }

        public static void Enter(IAlbianObject obj)
        {
        }

        public static void Enter(IAlbianObject[] objs)
        {

        }

        public static void Exit(IAlbianObject obj)
        {
        }

        public static void Exit(IAlbianObject[] objs)
        {
        }

        public static bool TryEnter(IAlbianObject obj)
        {
            return true;
        }

        public static bool TryEnter(IAlbianObject[] objs)
        {
            return true;
        }
    }
}

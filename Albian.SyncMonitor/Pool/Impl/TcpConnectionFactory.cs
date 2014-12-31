//using System;
//using System.Reflection;
//using log4net;

//namespace Albian.SyncMonitor.Pool.Impl
//{
//    /// <summary>
//    /// ���ӳش�������
//    /// </summary>
//    public class TcpConnectionFactory<T> : IPoolableObjectFactory<T>
//        where T : TcpConnection, new()
//    {

//        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
//        /// <summary>
//        /// ��������
//        /// </summary>
//        public T CreateObject()
//        {
//            T obj = Activator.CreateInstance<T>();
//            obj.IsFromPool = true;
//            obj.BatchId = FastDFSService.BatchId;
//            return obj;
//        }

//        /// <summary>
//        /// ���ٶ���.
//        /// </summary>
//        public void DestroyObject(T obj)
//        {
//            if (obj.Connected)
//            {
//                obj.GetStream().Close();
//                obj.Close();
//            }
//            if (obj is IDisposable)
//            {
//                ((IDisposable)obj).Dispose();
//            }
//        }

//        /// <summary>
//        /// ��鲢ȷ������İ�ȫ
//        /// </summary>
//        public bool ValidateObject(T obj)
//        {
//            return obj.Connected;
//        }

//        /// <summary>
//        /// ���������д��ö���. 
//        /// </summary>
//        public void ActivateObject(T obj,string ipAdderess,int port)
//        {
//            try
//            {
//                if (obj.Connected) return;
//                obj.IpAddress = ipAdderess;
//                obj.Port = port;
//                obj.ReceiveTimeout = FastDFSService.NetworkTimeout;
//                obj.Connect(); 
//            }
//            catch(Exception exc)
//            {
//                //if (null != _logger)_logger.WarnFormat("���ӳؼ������ʱ�����쳣,�쳣��ϢΪ:{0}",exc.Message);
//            }
//        }

//        /// <summary>
//        /// ж���ڴ�������ʹ�õĶ���.
//        /// </summary>
//        public void PassivateObject(T obj)
//        {
//            //if (obj.Connected) obj.Close();
//        }
//    }
//}
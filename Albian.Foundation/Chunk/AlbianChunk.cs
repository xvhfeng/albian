using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Collections;
using System.Threading;
using Albian.Foundation.Verify;
using log4net;

namespace Albian.Foundation.Chunk
{
    public class AlbianChunk : IAlbianChunk
    {
        private Hashtable _ht = new Hashtable();
        private object _locker = new object();
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private string _name = "albian";
          public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public AlbianChunk()
        {
        }

        public AlbianChunk(string name)
        {
            _name = name;
        }


        public bool Exist(string key)
        {
            if (ValidateService.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", string.Format("key is null or empty in the {0} chunk.", _name));
            }
            lock (_locker)
            {
                return _ht.ContainsKey(key);
            }
        }

        public object Get(string key)
        {
            if (ValidateService.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", string.Format("key is null or empty in the {0} chunk.", _name));
            }
            lock(_locker)
            {
                if (_ht.ContainsKey(key))
                {
                    IAlbianChunkObject obj = (IAlbianChunkObject)_ht[key];
                    return obj.Value;
                }
                return null ;
            }
        }

        public void Insert(string key, object value)
        {
            if (ValidateService.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", string.Format("key is null or empty in the {0} chunk.", _name));
            }
            lock (_locker)
            {
                if (_ht.ContainsKey(key))
                {
                    throw new AlbianChunkException(string.Format("the {0} cached is exist in the {1} chunk.", key,
                                                                _name));
                }
                else
                {
                    IAlbianChunkObject obj = new AlbianChunkObject
                                           {
                                               Key = key,
                                               Value = value,
                                               CreateTime = DateTime.Now
                                           };
                    _ht.Add(key, obj);
                }
            }
        }

        public void Set(string key, object value)
        {
            if (ValidateService.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", string.Format("key is null or empty in the {0} chunk.", _name));
            }
            lock (_locker)
            {
                if (_ht.ContainsKey(key))
                {
                    IAlbianChunkObject obj = new AlbianChunkObject
                                           {
                                               Key = key,
                                               Value = value,
                                               CreateTime = DateTime.Now
                                           };
                    _ht[key] = obj;
                }
                else
                {
                    IAlbianChunkObject obj = new AlbianChunkObject
                                           {
                                               Key = key,
                                               Value = value,
                                               CreateTime = DateTime.Now
                                           };
                    _ht.Add(key, obj);
                }
            }
        }

        public void Update(string key, object value)
        {
            if (ValidateService.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", string.Format("key is null or empty in the {0} chunk.", _name));
            }
            lock (_locker)
            {
                if (!_ht.ContainsKey(key))
                {
                    throw new AlbianChunkException(string.Format("the {0} cached is not exist in the {1} chunk.", key,
                                                                 _name));
                }
                IAlbianChunkObject obj = new AlbianChunkObject
                                       {
                                           Key = key,
                                           Value = value,
                                           CreateTime = DateTime.Now
                                       };
                _ht[key] = obj;
            }
        }

        public void Remove(string key)
        {
            if (ValidateService.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", string.Format("key is null or empty in the {0} chunk.", _name));
            }
            lock (_locker)
            {
                if (_ht.ContainsKey(key))
                {
                    _ht.Remove(key);

                }
            }
        }

        public void Remove()
        {
            lock (_locker)
            {
                _ht.Clear();
                if (null != _logger)
                {
                    _logger.FatalFormat("clean the {0} chunk is success.",_name);
                }
            }
        }
    }
}

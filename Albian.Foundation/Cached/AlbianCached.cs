using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using Albian.Foundation.Verify;
using log4net;

namespace Albian.Foundation.Cached
{
    public class AlbianCached : IAlbianCached
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Hashtable _ht;
        private readonly object _locker = new object();

        private string _name = "albian";

        public AlbianCached() : this("albian",10000)
        {

        }

        public AlbianCached(string name) :this(name,10000)
        {
            
        }
        
        public AlbianCached(int items) : this("albian", items)
        {
        }
        
        public AlbianCached(string name, int items)
        {
            _name = name;
            _ht = new Hashtable(items);
            Cleanup();
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #region IAlbianCached Members

        public bool Exist(string key)
        {
            if (ValidateService.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", string.Format("key is null or empty in the {0} cahced.", _name));
            }
            lock (_locker)
            {
                if (_ht.ContainsKey(key))
                {
                    WeakReference objWeak = (WeakReference)_ht[key];
                    if(null == objWeak || !objWeak.IsAlive || null == objWeak.Target)
                    {
                        _ht.Remove(key);
                        return false;
                    }
                    IAlbianCachedObject obj = (IAlbianCachedObject) objWeak.Target;
                    if (((int) DateTime.Now.Subtract(obj.CreateTime).TotalSeconds) <= obj.Timespan)
                    {
                        if (_logger != null)
                        {
                            _logger.WarnFormat("key:{0} in {1} cached is expired.", key, _name);
                        }
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }

        public object Get(string key)
        {
            if (ValidateService.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", string.Format("key is null or empty in the {0} cahced.", _name));
            }
            lock (_locker)
            {
                if (_ht.ContainsKey(key))
                {
                    WeakReference objWeak = (WeakReference)_ht[key];
                    if (null == objWeak || !objWeak.IsAlive || null == objWeak.Target)
                    {
                        _ht.Remove(key);
                        return null;
                    }
                    IAlbianCachedObject obj = (IAlbianCachedObject)objWeak.Target;
                    return ((int) DateTime.Now.Subtract(obj.CreateTime).TotalSeconds) <= obj.Timespan ? obj.Value : null;
                    ;
                }
                return null;
            }
        }

        public void Insert(string key, object value, int seconds)
        {
            if (ValidateService.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", string.Format("key is null or empty in the {0} cahced.", _name));
            }
            if (0 >= seconds)
            {
                throw new ArgumentNullException("seconds", string.Format("seconds is less or eq 0 in the {0} cahced.", _name));
            }

            lock (_locker)
            {
                if (_ht.ContainsKey(key))
                {
                    WeakReference objWeak = (WeakReference)_ht[key];
                    if (null == objWeak || !objWeak.IsAlive || null == objWeak.Target)
                    {
                        IAlbianCachedObject obj = new AlbianCachedObject
                                                      {
                                                          Timespan = seconds,
                                                          Key = key,
                                                          Value = value,
                                                          CreateTime = DateTime.Now
                                                      };
                        _ht[key] = WeakReferenceObject.CreateWeakReferenceObject(obj);
                    }
                    else
                    {
                        IAlbianCachedObject objOld = (IAlbianCachedObject)objWeak.Target;
                        if (((int)DateTime.Now.Subtract(objOld.CreateTime).TotalSeconds) <= objOld.Timespan)
                        {
                            throw new AlbianCachedException(string.Format("the {0} cached is exist in the {1} cached.", key,
                                                                          _name));
                        }
                        IAlbianCachedObject obj = new AlbianCachedObject
                        {
                            Timespan = seconds,
                            Key = key,
                            Value = value,
                            CreateTime = DateTime.Now
                        };
                        _ht[key] = WeakReferenceObject.CreateWeakReferenceObject(obj);
                    }
                }
                else
                {
                    IAlbianCachedObject obj = new AlbianCachedObject
                                                  {
                                                      Timespan = seconds,
                                                      Key = key,
                                                      Value = value,
                                                      CreateTime = DateTime.Now
                                                  };
                    _ht.Add(key, WeakReferenceObject.CreateWeakReferenceObject(obj));
                }
            }
        }

        public void Set(string key, object value, int seconds)
        {
            if (ValidateService.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", string.Format("key is null or empty in the {0} cahced.", _name));
            }
            if (0 >= seconds)
            {
                throw new ArgumentNullException("seconds",
                                                string.Format("seconds is less or eq 0 in the {0} cahced.", _name));
            }

            lock (_locker)
            {
                if (_ht.ContainsKey(key))
                {
                    IAlbianCachedObject obj = new AlbianCachedObject
                                                  {
                                                      Timespan = seconds,
                                                      Key = key,
                                                      Value = value,
                                                      CreateTime = DateTime.Now
                                                  };
                    _ht[key] = WeakReferenceObject.CreateWeakReferenceObject(obj);
                }
                else
                {
                    IAlbianCachedObject obj = new AlbianCachedObject
                                                  {
                                                      Timespan = seconds,
                                                      Key = key,
                                                      Value = value,
                                                      CreateTime = DateTime.Now
                                                  };
                    _ht.Add(key, WeakReferenceObject.CreateWeakReferenceObject(obj));
                }
            }
        }

        public void Update(string key, object value, int seconds)
        {
            if (ValidateService.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", string.Format("key is null or empty in the {0} cahced.", _name));
            }
            if (0 >= seconds)
            {
                throw new ArgumentNullException("seconds",
                                                string.Format("seconds is less or eq 0 in the {0} cahced.", _name));
            }

            lock (_locker)
            {
                if (!_ht.ContainsKey(key))
                {
                    throw new AlbianCachedException(string.Format("the {0} cached is not exist in the {1} cached.", key,
                                                                  _name));
                }
                IAlbianCachedObject obj = new AlbianCachedObject
                                              {
                                                  Timespan = seconds,
                                                  Key = key,
                                                  Value = value,
                                                  CreateTime = DateTime.Now
                                              };
                _ht[key] = WeakReferenceObject.CreateWeakReferenceObject(obj);
            }
        }


        public void Remove(string key)
        {
            if (ValidateService.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", string.Format("key is null or empty in the {0} cahced.", _name));
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
                    _logger.FatalFormat("clean the cached named {0} is success.", _name);
                }
            }
        }

        #endregion

        protected void Cleanup()
        {
            if (null != _logger)
            {
                _logger.InfoFormat("the cleanup event for {0} cached is stratup.", _name);
            }
            var thread = new Thread(delegate()
                                        {
                                            if (null != _logger)
                                            {
                                                _logger.InfoFormat("clean the {0} cached at {1}.", _name,
                                                                   DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss"));
                                            }
                                            lock (_locker)
                                            {
                                                ICollection keys = _ht.Keys;
                                                foreach (object key in keys)
                                                {
                                                    WeakReference objWeak = (WeakReference)_ht[key];
                                                    if (null == objWeak || !objWeak.IsAlive || null == objWeak.Target)
                                                    {
                                                        _ht.Remove(key);
                                                        continue;
                                                    }

                                                    IAlbianCachedObject obj = (IAlbianCachedObject)objWeak.Target;
                                                    if (
                                                        ((int) DateTime.Now.Subtract(obj.CreateTime).TotalSeconds) >=
                                                        obj.Timespan)
                                                    {
                                                        _ht.Remove(key);
                                                    }
                                                }
                                            }
                                            Thread.Sleep(5*60*1000);
                                        })
                             {
                                 IsBackground = true,
                                 Priority = ThreadPriority.Normal
                             };
            thread.Start();
        }
    }
}
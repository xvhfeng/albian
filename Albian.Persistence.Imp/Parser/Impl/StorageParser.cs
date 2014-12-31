#region

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Albian.Kernel;
using Albian.Persistence.Enum;
using Albian.Persistence.Imp.Cache;
using Albian.Persistence.Model;
using Albian.Persistence.Model.Impl;

#endregion

namespace Albian.Persistence.Imp.Parser.Impl
{
    public class StorageParser : FreeStorageParser
    {
        public static string DefaultStorageName
        {
            get { return "DefaultStorage"; }
        }

        public override void Loading()
        {
            Init(@"config/Storage.config");
            base.Loading();
        }

        protected override IDictionary<string, IStorageAttribute> ParserStorages(XmlNode node)
        {
            if (null == node)
            {
                throw new ArgumentNullException("node");
            }
            XmlNodeList nodes = node.SelectNodes("Storage");
            if (null == nodes || 0 == nodes.Count)
            {
                throw new Exception("the Storage node is empty in the Storage..config");
            }
            IDictionary<string, IStorageAttribute> dic = new Dictionary<string, IStorageAttribute>();
            int idx = 0;
            foreach (XmlNode n in nodes)
            {
                IStorageAttribute storageAttribute = ParserStorage(n);
                if (null != storageAttribute)
                {
                    dic.Add(storageAttribute.Name, storageAttribute);
                    if (0 == idx)
                    {
                        //insert the default storage
                        //the default is the first storage
                        StorageCache.InsertOrUpdate(DefaultStorageName, storageAttribute);
                    }
                    idx++;
                }
            }

            if (0 == dic.Count)
            {
                throw new Exception("the error in the storage.config");
            }
            return dic;
        }

        protected override IStorageAttribute ParserStorage(XmlNode node)
        {
            if (null == node)
            {
                throw new ArgumentNullException("node");
            }
            IStorageAttribute storageAttribute = GenerateStorageAttribute(node);
            if (string.IsNullOrEmpty(storageAttribute.Uid) && !storageAttribute.IntegratedSecurity)
            {
                throw new Exception("the database authentication mechanism is error.");
            }
            StorageCache.InsertOrUpdate(storageAttribute.Name, storageAttribute);
            return storageAttribute;
        }

        private static IStorageAttribute GenerateStorageAttribute(XmlNode node)
        {
            IStorageAttribute storageAttribute = new StorageAttribute();
            object oName;
            object oServer;
            object oDateBase;
            object oUid;
            object oPassword;
            object oMinPoolSize;
            object oMaxPoolSize;
            object oTimeout;
            object oPooling;
            object oIntegratedSecurity;
            object oDbClass;
            object oCharset;
            object oTransactional;

            foreach (XmlNode n in node.ChildNodes)
            {
                switch (n.Name)
                {
                    case "Name":
                        {
                            if (XmlFileParser.TryGetNodeValue(n, out oName))
                            {
                                storageAttribute.Name = oName.ToString().Trim();
                            }
                            break;
                        }
                    case "DatabaseStyle":
                        {
                            if (XmlFileParser.TryGetNodeValue(n, out oDbClass))
                            {
                                storageAttribute.DatabaseStyle = ConvertToDatabaseStyle.Convert(oDbClass.ToString());
                            }
                            break;
                        }
                    case "Server":
                        {
                            if (XmlFileParser.TryGetNodeValue(n, out oServer))
                            {
                                storageAttribute.Server = oServer.ToString().Trim();
                            }
                            break;
                        }
                    case "Database":
                        {
                            if (XmlFileParser.TryGetNodeValue(n, out oDateBase))
                            {
                                storageAttribute.Database = oDateBase.ToString().Trim();
                            }
                            break;
                        }
                    case "Uid":
                        {
                            if (XmlFileParser.TryGetNodeValue(n, out oUid))
                            {
                                storageAttribute.Uid = oUid.ToString().Trim();
                            }
                            break;
                        }
                    case "Password":
                        {
                            if (XmlFileParser.TryGetNodeValue(n, out oPassword))
                            {
                                storageAttribute.Password = oPassword.ToString().Trim();
                            }
                            break;
                        }
                    case "MinPoolSize":
                        {
                            if (XmlFileParser.TryGetNodeValue(n, out oMinPoolSize))
                            {
                                storageAttribute.MinPoolSize = Math.Abs(int.Parse(oMinPoolSize.ToString().Trim()));
                            }
                            break;
                        }
                    case "MaxPoolSize":
                        {
                            if (XmlFileParser.TryGetNodeValue(n, out oMaxPoolSize))
                            {
                                storageAttribute.MaxPoolSize = Math.Abs(int.Parse(oMaxPoolSize.ToString().Trim()));
                            }
                            break;
                        }
                    case "Timeout":
                        {
                            if (XmlFileParser.TryGetNodeValue(n, out oTimeout))
                            {
                                storageAttribute.Timeout = Math.Abs(int.Parse(oTimeout.ToString().Trim()));
                            }
                            break;
                        }
                    case "Pooling":
                        {
                            if (XmlFileParser.TryGetNodeValue(n, out oPooling))
                            {
                                storageAttribute.Pooling = bool.Parse(oPooling.ToString().Trim());
                            }
                            break;
                        }
                    case "IntegratedSecurity":
                        {
                            if (XmlFileParser.TryGetNodeValue(n, out oIntegratedSecurity))
                            {
                                string sIntegratedSecurity = oIntegratedSecurity.ToString().Trim();
                                if ("false" == sIntegratedSecurity.ToLower() || "no" == sIntegratedSecurity.ToLower())
                                    storageAttribute.IntegratedSecurity = false;
                                if ("true" == sIntegratedSecurity.ToLower()
                                    || "yes" == sIntegratedSecurity.ToLower() || "sspi" == sIntegratedSecurity.ToLower())
                                    storageAttribute.IntegratedSecurity = true;
                            }
                            break;
                        }
                    case "Charset":
                        {
                            if (XmlFileParser.TryGetNodeValue(n, out oCharset))
                            {
                                storageAttribute.Charset = oCharset.ToString().Trim();
                            }
                            break;
                        }
                    case "Transactional":
                        {
                            if (XmlFileParser.TryGetNodeValue(n, out oTransactional))
                            {
                                storageAttribute.Transactional = bool.Parse(oTransactional.ToString().Trim());
                            }
                            break;
                        }
                }
            }
            if (string.IsNullOrEmpty(storageAttribute.Name))
            {
                throw new Exception("the name is empty in the storage.config");
            }
            if (string.IsNullOrEmpty(storageAttribute.Server))
            {
                throw new Exception("the Server is empty in the storage.config");
            }
            if (string.IsNullOrEmpty(storageAttribute.Database))
            {
                throw new Exception("the Database is empty in the storage.config");
            }
            return storageAttribute;
        }

        public static string BuildConnectionString(IStorageAttribute storageAttribute)
        {
            var sbString = new StringBuilder(150);
            sbString.AppendFormat("Server={0};Initial Catalog={1};", storageAttribute.Server, storageAttribute.Database);
            if (storageAttribute.IntegratedSecurity)
            {
                sbString.Append("Integrated Security=SSPI;");
            }
            else
            {
                sbString.AppendFormat("User ID={0};Password={1};", storageAttribute.Uid, storageAttribute.Password);
            }
            if (0 != storageAttribute.Timeout)
            {
                sbString.AppendFormat("Connection Timeout={0};", storageAttribute.Timeout);
            }
            if (DatabaseStyle.MySql == storageAttribute.DatabaseStyle)//chinese charset
            {
                sbString.AppendFormat("charset = {0};",storageAttribute.Charset);//chinese charset，and the space with both sides from = must exist
                                                    //do not ask me why must exist space,then konwed by god only
                                                    // if you have chinese,the chinese word with 2 char length
            }
            return sbString.ToString();
        }

        public static IStorageAttribute GetStorage(string storageName)
        {
            if (null == storageName)
            {
                throw new ArgumentNullException("storageName");
            }
            if (!StorageCache.Exist(storageName))
            {
                return null;
            }
            return (IStorageAttribute) StorageCache.Get(storageName);
        }
    }
}
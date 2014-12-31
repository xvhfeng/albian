using System.Collections.Generic;
using System.IO;
using System.Reflection;
using log4net;

namespace Albian.Foundation
{
    public class PropertiesFileParserService
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<string, string> _items;

        public PropertiesFileParserService(string file)
        {
            Load(file);
        }

        public string GetValue(string field, string defValue)
        {
            return (GetValue(field) == null) ? (defValue) : (GetValue(field));
        }

        public string GetValue(string field)
        {
            return (_items.ContainsKey(field)) ? (_items[field]) : (null);
        }

        protected void Load(string file)
        {
            _items = new Dictionary<string, string>();
            foreach (string line in File.ReadAllLines(file))
            {
                if ((!string.IsNullOrEmpty(line)) &&
                    (!line.StartsWith(";")) &&
                    (!line.StartsWith("#")) &&
                    (!line.StartsWith("'")) &&
                    (line.Contains("=")))
                {
                    int index = line.IndexOf('=');
                    string key = line.Substring(0, index).Trim();
                    string value = line.Substring(index + 1).Trim();

                    if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                        (value.StartsWith("'") && value.EndsWith("'")))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }
                   
                    if (_items.ContainsKey(key))
                    {
                        if (null != _logger)
                        {
                            _logger.WarnFormat("the key:{0} is exist in the {1} file.", key, file);
                        }
                        continue;
                    }
                    _items.Add(key, value);
               
                }
            }
        }
    }
}
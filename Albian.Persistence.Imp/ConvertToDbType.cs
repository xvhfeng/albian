#region

using System;
using System.Data;

#endregion

namespace Albian.Persistence.Imp
{
    public static class ConvertToDbType
    {
        /// <summary>
        /// 将程序类型转换成数据库支持类型
        /// </summary>
        /// <param name="type">需要转换的类型</param>
        /// <returns></returns>
        public static DbType Convert(Type type)
        {
            switch (type.Name.Trim().ToLower())
            {
                case "boolean":
                case "bool":
                    return DbType.Boolean;
                case "byte":
                    return DbType.Byte;
                case "byte[]":
                    return DbType.Binary;
                case "char":
                case "string":
                    return DbType.String;
                case "datetime":
                    return DbType.DateTime;
                case "datetimeoffset":
                    return DbType.DateTimeOffset;
                case "decimal":
                    return DbType.Decimal;
                case "double":
                case "float":
                    return DbType.Double;
                case "guid":
                    return DbType.Guid;
                case "int16":
                    return DbType.Int16;
                case "uint16":
                    return DbType.UInt16;
                case "int32":
                    return DbType.Int32;
                case "uint32":
                    return DbType.UInt32;
                case "System.int64":
                    return DbType.Int64;
                case "System.uint64":
                    return DbType.UInt64;
                case "System.Single":
                    return DbType.Single;
                default:
                    return DbType.String;
            }
        }

        public static DbType Convert(string type)
        {
            switch (type.Trim().ToLower())
            {
                case "boolean":
                case "bool":
                    return DbType.Boolean;
                case "byte":
                    return DbType.Byte;
                case "byte[]":
                    return DbType.Binary;
                case "char":
                case "string":
                    return DbType.String;
                case "datetime":
                    return DbType.DateTime;
                case "datetimeoffset":
                    return DbType.DateTimeOffset;
                case "decimal":
                    return DbType.Decimal;
                case "double":
                case "float":
                    return DbType.Double;
                case "guid":
                    return DbType.Guid;
                case "int16":
                    return DbType.Int16;
                case "uint16":
                    return DbType.UInt16;
                case "int32":
                    return DbType.Int32;
                case "uint32":
                    return DbType.UInt32;
                case "System.int64":
                    return DbType.Int64;
                case "System.uint64":
                    return DbType.UInt64;
                case "System.Single":
                    return DbType.Single;
                default:
                    return DbType.String;
            }
        }
    }
}
#region

using Albian.Persistence.Enum;

#endregion

namespace Albian.Persistence.Imp
{
    public class ConvertToDatabaseStyle
    {
        public static DatabaseStyle Convert(string dbClass)
        {
            switch (dbClass.Trim().ToLower())
            {
                case "sqlserver":
                    {
                        return DatabaseStyle.SqlServer;
                    }
                case "oracle":
                    {
                        return DatabaseStyle.Oracle;
                    }
                case "mysql":
                    {
                        return DatabaseStyle.MySql;
                    }
                default:
                    {
                        return DatabaseStyle.SqlServer;
                    }
            }
        }
    }
}
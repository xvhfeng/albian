#region

using Albian.Persistence.Enum;

#endregion

namespace Albian.Persistence.Imp
{
    public static class ConvertToPermissionMode
    {
        public static PermissionMode Convert(string value)
        {
            switch (value.Trim().ToLower())
            {
                case "r":
                    {
                        return PermissionMode.R;
                    }
                case "w":
                    {
                        return PermissionMode.W;
                    }
                case "wr":
                case "rw":
                default:
                    {
                        return PermissionMode.WR;
                    }
            }
        }
    }
}
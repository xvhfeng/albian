#region

using Albian.Persistence.Enum;
using Albian.Persistence.Model;

#endregion

namespace Albian.Persistence.Imp.Model
{
    public class OrderByCondition : IOrderByCondition
    {
        private string _propertyName;

        private SortStyle _sortStyle = SortStyle.Asc;

        #region IOrderByCondition Members

        public virtual string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

        public virtual SortStyle SortStyle
        {
            get { return _sortStyle; }
            set { _sortStyle = value; }
        }

        #endregion
    }
}
using System;
using System.Data;
using System.Reflection;

namespace Core.Helpers
{
    public static class MapHelper
    {
        public static T ToObject<T>(this DataRow dataRow) where T : new()
        {
            T item = new T();
            foreach (DataColumn column in dataRow.Table.Columns)
            {
                PropertyInfo propertyInfo = item.GetType().GetProperty(column.ColumnName);

                if (propertyInfo != null && dataRow[column] != DBNull.Value)
                {
                    propertyInfo.SetValue(item, dataRow[column], null);
                }
            }

            return item;
        }
    }
}

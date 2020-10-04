using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MsSqlUtilityCore
{
    public static class Extensions
    {
        public static bool ColumnExists(this IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

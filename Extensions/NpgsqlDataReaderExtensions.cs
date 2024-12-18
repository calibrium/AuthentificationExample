using System.Data.Common;
using System.Text.Json;

namespace AuthentificationExample.Server.Extensions
{
    public static class NpgsqlDataReaderExtensions
    {
        public static T? GetNullableFieldValue<T>(this DbDataReader reader, string ordinalName) where T : class
        {
            int ordinal = reader.GetOrdinal(ordinalName);
            return !reader.IsDBNull(ordinal) ? reader.GetFieldValue<T>(ordinal) : default(T);
        }
    }
}

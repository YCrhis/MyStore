using System.Text.Json;

namespace MyStore.Utilities
{
    public static class SessionExtensions
    {
        public static void SessionSet<T>(this ISession session, string key, T Value)
        {
            session.SetString(key, JsonSerializer.Serialize(Value));
        }

        public static T? SessionGet<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}

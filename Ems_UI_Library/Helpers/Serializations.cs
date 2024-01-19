
using System.Text.Json;

namespace Ems_UI_Service.Helpers
{
    public static class Serializations
    {
       // public static string SerializeObj<T>(T obj) => JsonSerializer.Serialize(obj, typeof(T));
        public static string SerializeObj<T>(T obj) => JsonSerializer.Serialize(obj);
        public static T DeserializeJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString);
        public static IList<T> DeserializeJsonStringList<T>(string jsonString) => JsonSerializer.Deserialize<IList<T>>(jsonString);
    }
}

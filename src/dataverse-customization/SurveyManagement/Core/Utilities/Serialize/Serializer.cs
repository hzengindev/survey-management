using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Core.Utilities.Serialize
{
    public class Serializer
    {
        public static string Serialize<T>(object value)
        {
            string json = string.Empty;
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings
            {
                UseSimpleDictionaryFormat = true
            });
            using (MemoryStream ms = new MemoryStream())
            {
                js.WriteObject(ms, value);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    json = sr.ReadToEnd();
                }
            }
            return json;
        }

        public static T Deserialize<T>(string value)
        {
            T data = default(T);
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(value)))
            {
                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings
                {
                    UseSimpleDictionaryFormat = true
                });
                data = (T)deserializer.ReadObject(ms);
            }
            return data;
        }
    }
}
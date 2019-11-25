using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace LatteLocator.Core.Common
{
    public static class Helpers
    {
        
        //-----------Generic DataContractJsonSerialization----------------//

        public static string Serialize(object o)
        {
            string json = null;

            try
            {
                Debug.WriteLine($"-----Helpers----- Serialize: {o.GetType().FullName}");
                var jsonSerializer = new DataContractJsonSerializer(o.GetType());

                using (var stream = new MemoryStream())
                {
                    jsonSerializer.WriteObject(stream, o);
                    byte[] buffer = stream.ToArray();
                    json = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Helpers.Serialize Exception: {ex}");
            }

            return json;
        }

        public static T Deserialize<T>(string json)
        {
            T instance = default(T);

            try
            {
                Debug.WriteLine($"-----Helpers----- Deserialize");
                var jsonDeserializer = new DataContractJsonSerializer(typeof(T));
                
                using (var memStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    instance = (T)jsonDeserializer.ReadObject(memStream);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Helpers.Deserialize Exception: {ex}");
            }

            return instance;
        }
    }
}

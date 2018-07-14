using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;

namespace ZCompileNLP.Json
{
    public static class JsonUtil
    {
        public static T ToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);//微软.NET库转化Json不行
            //var ser = new DataContractJsonSerializer(typeof(T));
            //var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            //stream.Position = 0;
            //return (T)ser.ReadObject(stream);
        }

        public static string ToJson<T>(T obj)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            var stream = new MemoryStream();
            ser.WriteObject(stream, obj);
            var db = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(db, 0, (int)stream.Length);
            var dataString = Encoding.UTF8.GetString(db);
            return dataString;
        }
    }
}

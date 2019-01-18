using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.RISCV.Integration.Client
{
    public class JsonHelper
    {
        public static string Serialize(object source)
        {
            return JsonConvert.SerializeObject(source, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });
        }

        public static T Deserialize<T>(string source)
        {
            return JsonConvert.DeserializeObject<T>(source, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });
        }

        public static T Clone<T>(T source)
        {
            return JsonHelper.Deserialize<T>(JsonHelper.Serialize(source));
        }
    }
}

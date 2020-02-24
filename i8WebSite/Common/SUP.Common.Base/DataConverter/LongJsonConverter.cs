using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP.Common.Base
{
    public class LongJsonConverter : CustomCreationConverter<long>
    {
        public override long Create(Type objectType)
        {
            return 0;
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //base.WriteJson(writer, value, serializer);
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                string s = value.ToString();
                writer.WriteValue(s);
            }
        }

    }

}

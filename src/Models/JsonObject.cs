using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Models
{
    public class JsonObject
    {
        #region Ctors

        private JsonObject(string id, JsonTypes type)
        {
            Id = id;
            JsonType = type;
            Fields = new JsonFields(this);
        }

        private JsonObject(string id, JArray a)
            : this(id, JsonTypes.Array)
        {
            for (int i = 0; i < a.Count; i++)
            {
                Fields.Add(Create($"[{i}]", a[i]));
            }
        }

        private JsonObject(string id, JObject o)
            : this(id, JsonTypes.Object)
        {
            foreach (var pair in o.Properties())
            {
                Fields.Add(Create(pair.Name, pair.Value));
            }
        }

        public JsonObject(string id, JValue v)
            : this(id, JsonTypes.Value)
        {
            Value = v;
        }

        #endregion

        public string Id { get; }

        public JValue Value { get; }

        public JsonTypes JsonType { get; }

        public JsonObject Parent { get; set; }

        public JsonFields Fields { get; }

        public bool ContainsField(string id, JsonTypes type)
        {
            var field = Fields[id];
            return (field != null && field.JsonType == type);
        }

        public bool ContainsField(string id)
        {
            return (Fields[id] != null);
        }

        /// <summary>
        /// Tries to parse the input and create a <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="json">The object serialized in JSON format</param>
        /// <returns>A tree object containing the parsed input</returns>
        public static JsonObject Create(string json)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject(json);
                return Create("ROOT", obj);
            }
            catch (Exception e)
            {
                throw new JsonParseError(e.Message, e);
            }
        }

        private static JsonObject Create(string id, object obj)
        {
            if (obj is JArray a)
                return new JsonObject(id, a);
            else if (obj is JObject o)
                return new JsonObject(id, o);
            else
                return new JsonObject(id, (JValue)obj);
        }
    }
}
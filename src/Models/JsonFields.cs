using System;
using System.Collections;
using System.Collections.Generic;

namespace Models
{
    public class JsonFields : IEnumerable<JsonObject>
    {
        readonly JsonObject _parent;
        readonly List<JsonObject> _fields;
        readonly Dictionary<string, JsonObject> _fieldsById;

        public JsonFields(JsonObject parent)
        {
            _parent = parent;
            _fields = new List<JsonObject>();
            _fieldsById = new Dictionary<string, JsonObject>();
        }

        public IEnumerator<JsonObject> GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(JsonObject field)
        {
            field.Parent = _parent;
            _fields.Add(field);
            _fieldsById[field.Id] = field;
        }

        public int Count => _fields.Count;

        public JsonObject this[int index] => _fields[index];

        public JsonObject this[string id]
        {
            get
            {
                if (_fieldsById.TryGetValue(id, out JsonObject result))
                    return result;
                return null;
            }
        }

        public bool ContainsId(string id)
        {
            return _fieldsById.ContainsKey(id);
        }
    }
}

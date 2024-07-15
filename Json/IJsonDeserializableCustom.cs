using System.Collections.Generic;

namespace c1tr00z.AssistLib.Json {
    public interface IJsonDeserializableCustom : IJsonDeserializable {
        void DeserializeCustom(Dictionary<string, object> json);
    }
}
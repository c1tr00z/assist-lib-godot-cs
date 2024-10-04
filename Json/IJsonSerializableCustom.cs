using System.Collections.Generic;

namespace c1tr00z.AssistLib.Json {
    public interface IJsonSerializableCustom : IJsonSerializable {
        void SerializeCustom(Dictionary<string, object> json);
    }
}
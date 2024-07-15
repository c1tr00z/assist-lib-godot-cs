using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace c1tr00z.AssistLib.Json {
    public static class JsonReflectionUtils {

        #region Private Fields

        private static Dictionary<Type, List<FieldInfo>> _serializedFields = new Dictionary<Type, List<FieldInfo>>();

        #endregion

        #region Class Implementation

        public static List<FieldInfo> GetJsonSerializedFields(this Type type) {

            var typeInterfaces = type.GetInterfaces();
            
            if (!typeInterfaces.Contains(typeof(IJsonSerializable)) && !typeInterfaces.Contains(typeof(IJsonDeserializable))) {
                return new List<FieldInfo>();
            }
            
            if (!_serializedFields.ContainsKey(type)) {
                var fieldsList = type.GetFields(BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public)
                    .Where(f => f.GetCustomAttributes().Any(a => a is JsonSerializableFieldAttribute)).ToList();
                
                _serializedFields.Add(type, fieldsList);
            }

            return _serializedFields[type];
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace c1tr00z.AssistLib.Common;

public static class ReflectionUtils {

        #region Private Fields

        private static Dictionary<string, Type> _types = new Dictionary<string, Type>();

        private static List<Type> _typesList = new List<Type>();
        
        #endregion

        #region Class Implementation

        public static Dictionary<string, Type> GetTypes() {
            if (_types == null || _types.Count == 0) {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                _types = assemblies.ToList().SelectMany(a => a.GetTypes()).ToUniqueDictionary(t => t.FullName, t => t);
            }

            return _types;
        }

        public static List<Type> GetTypesList() {
            if (_typesList == null || _typesList.Count == 0) {
                _typesList = GetTypes().Values.ToUniqueList();
            }

            return _typesList;
        }

        public static Type GetTypeByName(string name) {
            var types = GetTypes();
            if (types.ContainsKey(name)) {
                return types[name];
            }

            return null;
        }

        public static List<Type> GetSubclassesOf<T>(bool includeAbstract = true) {
            return GetSubclassesOf(typeof(T), includeAbstract);
        }

        public static List<Type> GetSubclassesOf(Type baseClass, bool includeAbstract = true) {
            if (baseClass == null) {
                return GetTypesList();
            }

            var types = GetTypesList().Where(t => t.IsSubclassOf(baseClass));

            if (!includeAbstract) {
                types = types.Where(t => !t.IsAbstract);
            }

            return types.ToList();
        }

        public static List<Type> GetTypesByInterface(Type interfaceType, bool includeAbstract = true) {
            if (interfaceType == null) {
                return GetTypesList();
            }

            var allTypes = GetTypesList().Where(t => t.GetInterfaces().Contains(interfaceType));

            if (!includeAbstract) {
                allTypes = allTypes.Where(t => !t.IsAbstract);
            }
            
            return allTypes.ToList();
        }

        public static List<Type> GetTypesByInterface<T>(bool includeAbstract = true) {
            return GetTypesByInterface(typeof(T), includeAbstract);
        }

        public static IEnumerable<PropertyInfo> GetPublicProperties(this Type type) {
            return type.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
        }

        public static PropertyInfo GetPublicPropertyInfo(this Type type, string fieldName) {
            return type.GetProperty(fieldName, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
        }

        public static string GetPropertyNameByType(this PropertyInfo propertyInfo) {
            return $"{propertyInfo.PropertyType.Name}/{propertyInfo.Name}";
        }

        public static string GetValidFullName(this Type type) {

            if (type == null) {
                return null;
            }

            if (!type.IsGenericType) {
                return type.FullName;
            }

            var genericArgs = type.GetGenericArguments();
            var genericArgsString = "";
            for (var i = 0; i < genericArgs.Length; i++) {
                if (i > 0) {
                    genericArgsString += ",";
                }

                genericArgsString += genericArgs[i].FullName;
            }


            return $"{type.FullName.Split('`').First()}<{genericArgsString}>";
        }

        #endregion
    }
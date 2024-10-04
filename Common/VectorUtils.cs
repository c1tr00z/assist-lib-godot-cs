using System.Globalization;
using System.Linq;
using Godot;

namespace c1tr00z.AssistLib.Utils {
    public static class VectorUtils {

        #region Class Implementations

        public static Vector2 ToVector2(this Vector4 v) {
            return new Vector2(v.X, v.Y);
        }

        public static Vector2 ToVector2(this Vector3 v) {
            return new Vector2(v.X, v.Y);
        }

        public static Vector3 ToVector3(this Vector4 v) {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static Vector3 ToVector3(this Vector2 v) {
            return new Vector3(v.X, v.Y, 0);
        }

        public static Vector4 ToVector4(this Vector3 v) {
            return new Vector4(v.X, v.Y, v.Z, 0);
        }

        public static Vector4 ToVector4(this Vector2 v) {
            return new Vector4(v.X, v.Y, 0, 0);
        }

        public static Vector3 RandomV3(float minValue, float maxValue) {
            return new Vector3((float)GD.RandRange(minValue, maxValue), (float)GD.RandRange(minValue, maxValue),
                (float)GD.RandRange(minValue, maxValue));
        }

        public static Vector3 RandomV3(int minValue, int maxValue) {
            return new Vector3(GD.RandRange(minValue, maxValue), GD.RandRange(minValue, maxValue),
                GD.RandRange(minValue, maxValue));
        }

        public static Vector3 RandomV3(Vector3 minValue, Vector3 maxValue) {
            return new Vector3((float)GD.RandRange(minValue.X, maxValue.X), (float)GD.RandRange(minValue.Y, maxValue.Y),
                (float)GD.RandRange(minValue.Z, maxValue.Z));
        }
        
        public static Vector2 RandomV2(Vector2 minValue, Vector2 maxValue) {
            return new Vector2((float)GD.RandRange(minValue.X, maxValue.X),
                (float)GD.RandRange(minValue.Y, maxValue.Y));
        }

        public static Vector3 ToVector3XZ(this Vector2 v) {
            return new Vector3(v.X, 0, v.Y);
        }

        public static Vector3 ToVector3XZ(this Vector3 v) {
            return new Vector3(v.X, 0, v.Y);
        }

        public static Vector3 RemoveY(this Vector3 v) {
            return new Vector3(v.X, 0, v.Z);
        }

        /* 
     * found https://answers.unity.com/questions/1134997/string-to-vector3.html
     * then modified
    */

        private static string[] SplitVectorString(string vectorString) {
            if (string.IsNullOrEmpty(vectorString)) {
                return new string[0];
            }

            // Remove the parentheses
            if (vectorString.StartsWith("(") && vectorString.EndsWith(")")) {
                vectorString = vectorString.Substring(1, vectorString.Length - 2);
            }

            return vectorString.Split(',');
        }

        public static bool TryParse(string vectorString, out Vector4 result) {
            result = Vector4.Zero;
            var sArray = SplitVectorString(vectorString);

            if (sArray.Length < 2) {
                return false;
            }

            var resultArray = new float[4];

            for (var i = 0; i < 4; i++) {
                if (i >= sArray.Length) {
                    break;
                }

                float floatValue;
                if (float.TryParse(sArray[i], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out floatValue)) {
                    resultArray[i] = floatValue;
                } else {
                    return false;
                }
            }

            // store as a Vector4
            result = new Vector4(resultArray[0], resultArray[1], resultArray[2], resultArray[3]);

            return true;
        }

        public static bool TryParse(string vectorString, out Vector3 result) {
            result = Vector3.Zero;
            Vector4 vector4;
            if (TryParse(vectorString, out vector4)) {
                result = vector4.ToVector3();
                return true;
            }

            return false;
        }

        public static bool TryParse(string vectorString, out Vector2 result) {
            result = Vector2.Zero;
            Vector4 vector4;
            if (TryParse(vectorString, out vector4)) {
                result = vector4.ToVector2();
                return true;
            }

            return false;
        }

        public static string ToString(this Vector4 vector, string format, CultureInfo cultureInfo) {
            return string.Format("\"({0}, {1}, {2}, {3})\"",
                vector.X.ToString(format, cultureInfo),
                vector.Y.ToString(format, cultureInfo),
                vector.Z.ToString(format, cultureInfo),
                vector.W.ToString(format, cultureInfo));
        }

        public static string ToString(this Vector3 vector, string format, CultureInfo cultureInfo) {
            return string.Format("\"({0}, {1}, {2})\"",
                vector.X.ToString(format, cultureInfo),
                vector.Y.ToString(format, cultureInfo),
                vector.Z.ToString(format, cultureInfo));
        }

        public static string ToString(this Vector2 vector, string format, CultureInfo cultureInfo) {
            return string.Format("\"({0}, {1})\"",
                vector.X.ToString(format, cultureInfo),
                vector.Y.ToString(format, cultureInfo));
        }

        public static string ToInvariantCultureString(this Vector2 vector) {
            return vector.ToString("G", CultureInfo.InvariantCulture);
        }

        public static string ToInvariantCultureString(this Vector3 vector) {
            return vector.ToString("G", CultureInfo.InvariantCulture);
        }

        public static string ToInvariantCultureString(this Vector4 vector) {
            return vector.ToString("G", CultureInfo.InvariantCulture);
        }

        public static bool Approximately(Vector3 a, Vector3 b, float threshold) {
            return (b - a).Length() <= threshold;
        }

        public static Vector3 Average(params Vector3[] args) {
            var sum = Vector3.Zero;
            foreach (var v in args) {
                sum += v;
            }

            return sum / args.Length;
        }
        
        public static Vector2 Average(params Vector2[] args) {
            var sum = Vector2.Zero;
            foreach (var v in args) {
                sum += v;
            }

            return sum / args.Length;
        }

        #endregion
    }
}

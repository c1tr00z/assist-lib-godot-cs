using System.Text;

namespace c1tr00z.AssistLib.Common;

public static class StringExt {
    
    #region Class Implementation

    public static bool IsNullOrEmpty(this string str) {
        return string.IsNullOrEmpty(str);
    }

    public static string SnakeCaseToCamelCase(this string str) {
        var substrings = str.Split('_');
        var builder = new StringBuilder();
        foreach (var substring in substrings) {
            if (substring.Length == 0) {
                continue;
            }
            var substringCamelCase = "";
            if (substring.Length > 0) {
                substringCamelCase += substring[0].ToString().ToUpper();
            }

            if (substring.Length > 1) {
                substringCamelCase += substring.Substring(1).ToLower();
            }

            builder.Append(substringCamelCase);
        }

        return builder.ToString();
    }

    #endregion
}
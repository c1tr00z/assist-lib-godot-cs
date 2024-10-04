using System;

namespace projectwitch.addons.AssistLib.Common;

public static class DecimalsExt {
    
    #region Class Implementation

    public static bool IsApproximately(this float num1, float num2, float tolerance = 0.001f) {
        return Math.Abs(num1 - num2) < tolerance;
    }
    
    public static bool IsApproximately(this double num1, double num2, double tolerance = 0.001f) {
        return Math.Abs(num1 - num2) < tolerance;
    }

    #endregion
}
using System.Collections.Generic;
using System.Linq;

namespace projectwitch.addons.AssistLib.Spreadsheets;

public static class SpreadsheetImporter {

    #region Class Implementation

    public static bool ParseCSV(string csvString, out List<List<string>> parsed) {
        parsed = new List<List<string>>();
        
        var rows = csvString.Split('\r', '\n');
        foreach (var row in rows) {
            parsed.Add(row.Split(',').ToList());
        }

        return true;
    }

    #endregion
}
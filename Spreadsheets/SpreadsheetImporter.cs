using System;
using System.Collections.Generic;
using System.Linq;

namespace projectwitch.addons.AssistLib.Spreadsheets;

public static class SpreadsheetImporter {

    #region Class Implementation

    public static bool ParseCSV(string csvString, out List<List<string>> parsed) {
        var parsedPage = new List<List<string>>();
        parsed = new List<List<string>>();
        
        var rows = csvString.Split('\r', '\n');
        foreach (var row in rows) {
            parsed.Add(row.Split(',').ToList());
        }

        parsed = parsedPage;
        return true;
    }

    #endregion
}
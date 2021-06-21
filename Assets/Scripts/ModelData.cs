using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ModelData {
    public List<string> columnHeaders;
    public List<List<float>> rows = new List<List<float>>();
    private bool _readData = false;

    public void ReadCsv() {
        // read the csv file at this location
        using var reader = new StreamReader(@"./Assets/Data/modelData.csv");

        // first row contains the column names so we need to parse strings first
        var columnHeader = false;
        while (!reader.EndOfStream) {
            var line = reader.ReadLine();
            // skip empty lines
            if (line == null) continue;
            // our delimiter is a `,`
            var data = line.Split(',');
            // check to see if we should parse column headers or actual data 
            if (columnHeader == false) {
                columnHeaders = data.ToList();
                columnHeader = true;

                for (var i = 0; i < columnHeaders.Count; i++) {
                   rows.Add(new List<float>()); 
                }
            } else {
                // if we encounter anything other than a float this will fail
                for (var i = 0; i < data.Length; i++) {
                    rows[i].Add(float.Parse(data[i]));
                }
            }
        }

        _readData = true;
    }

    public List<float> GetColumnData(string columnName) {
        // Quick sanity check to make sure the user has first read the data before trying to access it.
        if (!_readData) {
            Debug.LogWarning("Trying to get column data before it has been read. " +
                             "Make sure to call ReadCsv before trying to retrieve data.");
            return null;
        }

        // Look for the columnName in the columnHeaders
        for (var i = 0; i < columnHeaders.Count; i++) {
            if (columnHeaders[i] == columnName) {
                return rows[i];
            }
        }

        // We return null if the columnName doesn't match any of the columnHeaders
        return null;
    }
}

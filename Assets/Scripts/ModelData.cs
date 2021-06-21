using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ModelData {
    private List<string> _columnHeaders;
    private readonly List<List<float>> _rows = new List<List<float>>();

    public ModelData() {
        ReadCsv();
    }

    private void ReadCsv() {
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
                _columnHeaders = data.ToList();
                columnHeader = true;

                for (var i = 0; i < _columnHeaders.Count; i++) {
                   _rows.Add(new List<float>()); 
                }
            } else {
                // if we encounter anything other than a float this will fail
                for (var i = 0; i < data.Length; i++) {
                    _rows[i].Add(float.Parse(data[i]));
                }
            }
        }

    }

    public List<float> GetColumnData(string columnName) {
        // Look for the columnName in the columnHeaders
        for (var i = 0; i < _columnHeaders.Count; i++) {
            if (_columnHeaders[i] == columnName) {
                return _rows[i];
            }
        }

        // We return null if the columnName doesn't match any of the columnHeaders
        return null;
    }
}

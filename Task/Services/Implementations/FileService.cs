using System.Data;
using System.IO;
using System.Text;
using ExcelDataReader;

using Task.Services.Interfaces;

namespace Task.Services.Implementations;

public class FileService : IFileService
{
    public DataTable ImportFile(string path)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
        using var streamval = File.Open(path, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(streamval);
        var configuration = new ExcelDataSetConfiguration
        {
            ConfigureDataTable = _ =>  new ExcelDataTableConfiguration
            {
                UseHeaderRow = false
            }
        };
        var dataSet = reader.AsDataSet(configuration);

        if (dataSet.Tables.Count > 0)
        {
            return dataSet.Tables[0];
        }

        return new DataTable();
    }

    public void ExportFile(DataTable data, string path)
    {
        var lines =  data.AsEnumerable()
            .Select(row => string.Join(",", row.ItemArray.Select(val => $"\"{val}\"")));
        using var fs = new FileStream(path, FileMode.Truncate, FileAccess.Write);
        using var sw = new StreamWriter(fs);
        foreach (var line in lines)
             sw.WriteLine(line);
    }
}
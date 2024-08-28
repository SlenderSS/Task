

using System.Data;

namespace Task.Services.Interfaces;

public interface IFileService
{
    DataTable ImportFile(string path);
    void ExportFile(DataTable data, string path);
}
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DryIoc.ImTools;
using ExcelDataReader;
using Microsoft.Win32;

namespace Task.ViewModels;

public class MainViewModel : BindableBase
{
    private DataTable _data;
    private int _selectedRowIndex = -1;
    private DataGridCellInfo _currentCell;
    private bool _canSave;
    private string _filePath = string.Empty;

    public bool CanSave
    {
        get => _canSave;
        set => SetProperty(ref _canSave, value);
    }

    public DataTable Data
    {
        get => _data;
        set => SetProperty(ref _data, value);
    }
    public DataGridCellInfo CurrentCell
    {
        get => _currentCell;
        set => SetProperty(ref _currentCell, value);
            
        
    }
    public int SelectedRowIndex
    {
        get => _selectedRowIndex;
        set => SetProperty(ref _selectedRowIndex, value);
    }

    #region Commands
    public ICommand CloseCommand { get; set; }
    public ICommand MoveWindowCommand { get; set; }
    
    public ICommand SaveCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public ICommand ImportFileCommand { get; set; }
    public ICommand ExportFileCommand { get; set; }
    public ICommand AddRowCommand { get; set; }
    public ICommand CopyRowCommand { get; set; }
    public ICommand RemoveRowCommand { get; set; }

    #endregion

    public MainViewModel()
    {
        #region Init commands
        CloseCommand = new DelegateCommand(
            () => Application.Current.Shutdown(), 
            () => true);

        MoveWindowCommand = new DelegateCommand(
            () => Application.Current.MainWindow.DragMove(),
            () => true);
        
        ImportFileCommand = new DelegateCommand(ImportFile);
        
        ExportFileCommand = new DelegateCommand( () => ExportFile());

        AddRowCommand = new DelegateCommand(
            () => Data.Rows.Add(),
            () => Data != null)
            .ObservesProperty(() => Data);
        
        CopyRowCommand = new DelegateCommand(
            () => Clipboard.SetText(string.Join(", ", Data.Rows[SelectedRowIndex].ItemArray)), 
            CanDoWithRow)
            .ObservesProperty(() => SelectedRowIndex);
        
        RemoveRowCommand = new DelegateCommand(
            () => Data.Rows.Remove(Data.Rows[SelectedRowIndex]),
            CanDoWithRow)
            .ObservesProperty(() => SelectedRowIndex);
        
        


        #endregion


    }



    #region Private Methods

    private void CopyRow()
    {
        ;
    }

    private bool CanDoWithRow() => 
        SelectedRowIndex != -1 && SelectedRowIndex <= Data.Rows.Count - 1;

    private async void ExportFile(bool isNewFile = true)
    {
        if (isNewFile)
        {
            
        }
        string path = "";
        var lines =  Data.AsEnumerable()
            .Select(row => string.Join(",", row.ItemArray.Select(val => $"\"{val}\"")));


        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter =  "Excel files (.xlsm)|*.xlsm";
        if (saveFileDialog.ShowDialog() == true)
        {
            path = saveFileDialog.FileName;
            
            await using var fs = new FileStream(path, FileMode.Truncate, FileAccess.Write);
            await using var sw = new StreamWriter(fs);
            foreach (var line in lines)
                await sw.WriteLineAsync(line);
            
            MessageBox.Show("Export successful!","Success");
        }

       
    }
    
    private void ImportFile()
    {
        OpenFileDialog choofdlog = new OpenFileDialog();
        choofdlog.Filter =  "Excel files (*.xls or .xlsm)|.xls;*.xlsm";
        if (choofdlog.ShowDialog() == true)
        {
            _filePath = choofdlog.FileName;
            
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            using var streamval = File.Open(_filePath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(streamval);
            var configuration = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _=>  new ExcelDataTableConfiguration
                {
                    UseHeaderRow = false
                }
            };
            var dataSet = reader.AsDataSet(configuration);

            if (dataSet.Tables.Count > 0)
            {
                Data = dataSet.Tables[0];
                        
            }
        }
    }
    

    #endregion
}
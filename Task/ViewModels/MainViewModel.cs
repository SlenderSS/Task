using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

using ExcelDataReader;
using Microsoft.Win32;
using Microsoft.Xaml.Behaviors;
using Task.Helpers;
using Task.Services.Interfaces;

namespace Task.ViewModels;

public class MainViewModel : BindableBase
{
    private readonly IFileService _fileService;

    #region Private fields

    private DataTable _data;
    private int _selectedRowIndex = -1;

    private bool _canSave;
    private string _filePath = string.Empty;

    #endregion

    #region Public Properties

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

    public int SelectedRowIndex
    {
        get => _selectedRowIndex;
        set => SetProperty(ref _selectedRowIndex, value);
    }


    #endregion

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
    public ICommand AutoGeneratingColumnCommand { get; set; }
    #endregion

    public MainViewModel(IFileService fileService)
    {
        _fileService = fileService;

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


        AutoGeneratingColumnCommand = new DelegateCommand<object>(OnAutoGeneratingColumn);

        #endregion


    }



    #region Private Methods

    private bool CanDoWithRow() => 
        SelectedRowIndex != -1 && SelectedRowIndex <= Data.Rows.Count - 1;

    
    private void OnAutoGeneratingColumn(object parameter)
    {
        if (parameter is DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column is DataGridBoundColumn boundColumn)
            {
                var binding = (Binding)boundColumn.Binding;
                binding.ValidationRules.Add(new DataGridCellValitationRule());
            }
        }
    }

    private async void ExportFile(bool isNewFile = true)
    {
        string path = string.Empty;

        if (!isNewFile)
        {
            path = _filePath;
        }
        else
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter =  "Excel files (.xlsm)|*.xlsm";
            if (saveFileDialog.ShowDialog() == true)
            {
                path = saveFileDialog.FileName;
            }
        }

        _fileService.ExportFile(Data, path);
        
        MessageBox.Show("Export successful!","Success");
  
    }

    private void ImportFile()
    {
        OpenFileDialog choofdlog = new OpenFileDialog();
        choofdlog.Filter =  "Excel files (*.xls or .xlsm)|.xls;*.xlsm";
        if (choofdlog.ShowDialog() == true)
        {
            _filePath = choofdlog.FileName;
            Data = _fileService.ImportFile(_filePath);
        }
    }
    

    #endregion
}




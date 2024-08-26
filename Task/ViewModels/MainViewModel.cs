using System.Windows;
using System.Windows.Input;

namespace Task.ViewModels;

public class MainViewModel : BindableBase
{


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

        #endregion

    }



    #region Private Methods

    

    #endregion
}
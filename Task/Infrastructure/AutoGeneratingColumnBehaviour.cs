using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Task.Infrastructure;


public class AutoGeneratingColumnBehavior : Behavior<DataGrid>
{
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register("Command", typeof(ICommand), typeof(AutoGeneratingColumnBehavior), new PropertyMetadata(null));

    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.AutoGeneratingColumn += OnAutoGeneratingColumn;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.AutoGeneratingColumn -= OnAutoGeneratingColumn;
    }

    private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        if (Command != null && Command.CanExecute(e))
        {
            Command.Execute(e);
        }
    }
}
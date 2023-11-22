namespace SimpleApp;

using System.IO;
using System.Reflection;
using System.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public string Folder {  get; set; }

    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = this;
        Folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}

using System.Windows;

namespace CSharpTraceRoute
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowDataContext();
        }

        private void StartTraceRouteButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindowDataContext context = (MainWindowDataContext)DataContext;
            context.StartTraceRoute();
        }
    }
}

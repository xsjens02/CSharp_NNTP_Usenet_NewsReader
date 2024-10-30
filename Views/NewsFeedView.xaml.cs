using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using UsenetProgram.Services;
using UsenetProgram.ViewModels;

namespace UsenetProgram.Views
{
    /// <summary>
    /// Interaction logic for NewsFeedView.xaml
    /// </summary>
    public partial class NewsFeedView : Window
    {
        public NewsFeedView()
        {
            InitializeComponent();
            this.DataContext = new NewsFeedViewModel();
            Loaded += NewsFeedView_Loaded;
            Closing += NewsFeedView_Closing;
        }

        private void NewsFeedView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IWindowService vm)
            {
                vm.Close += () => this.Close();
            }
        }

        private void NewsFeedView_Closing(object? sender, CancelEventArgs e)
        {
            NetworkManager.Instance.Reset();
        }
    }
}

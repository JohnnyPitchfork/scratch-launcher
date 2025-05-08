using System;
using System.Windows;
using Microsoft.Web.WebView2.Wpf;

namespace ScratchLauncher
{
    public partial class ScratchJrWindow : Window
    {
        public ScratchJrWindow()
        {
            InitializeComponent();
        }

        private void ReturnToLauncher_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}

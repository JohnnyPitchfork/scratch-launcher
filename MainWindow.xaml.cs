using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Wpf;

namespace ScratchLauncher
{
    public partial class MainWindow : Window
    {
        private Window scratchJrWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LaunchScratchDesktop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("C:\\Program Files (x86)\\Scratch 3\\Scratch 3.exe");
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to launch Scratch Desktop. Please ensure it is installed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LaunchScratchJr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                scratchJrWindow = new Window
                {
                    Title = "ScratchJr",
                    WindowState = WindowState.Maximized,
                    WindowStyle = WindowStyle.None,
                    Topmost = true
                };

                var webView = new WebView2();
                webView.Source = new Uri("https://codejr.org/scratchjr/index.html");

                var returnButton = new Button
                {
                    Content = "Return to Launcher",
                    FontSize = 16,
                    Padding = new Thickness(10),
                    Margin = new Thickness(0, 10, 0, 10)
                };
                returnButton.Click += (s, e) => { scratchJrWindow.Close(); this.Show(); };

                var stackPanel = new StackPanel();
                stackPanel.Children.Add(returnButton);
                stackPanel.Children.Add(webView);

                scratchJrWindow.Content = stackPanel;
                scratchJrWindow.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load ScratchJr. Please ensure WebView2 is installed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

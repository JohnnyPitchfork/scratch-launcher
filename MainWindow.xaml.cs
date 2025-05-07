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

            // Basic UI Setup
            this.Title = "Scratch Launcher";
            this.WindowState = WindowState.Maximized; // Fullscreen
            this.ResizeMode = ResizeMode.NoResize; // Prevent resizing
            this.WindowStyle = WindowStyle.None; // Remove title bar
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen; // Start centered
            this.Topmost = true; // Keep app in focus

            var grid = new Grid();

            var stackPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(10)
            };

            var titleLabel = new TextBlock
            {
                Text = "Choose Your App",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };

            var scratchDesktopButton = new Button
            {
                Content = "Launch Scratch Desktop",
                FontSize = 16,
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 10)
            };
            scratchDesktopButton.Click += LaunchScratchDesktop_Click;

            var scratchJrButton = new Button
            {
                Content = "Launch ScratchJr",
                FontSize = 16,
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 10)
            };
            scratchJrButton.Click += LaunchScratchJr_Click;

            var exitButton = new Button
            {
                Content = "Exit",
                FontSize = 16,
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 10)
            };
            exitButton.Click += ExitApp_Click;

            stackPanel.Children.Add(titleLabel);
            stackPanel.Children.Add(scratchDesktopButton);
            stackPanel.Children.Add(scratchJrButton);
            stackPanel.Children.Add(exitButton);

            grid.Children.Add(stackPanel);

            this.Content = grid;
        }

        private void LaunchScratchDesktop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("C:\\Program Files (x86)\\Scratch 3\\Scratch 3.exe");
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
                    Width = 1280,
                    Height = 800,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
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

        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            if (!IsRunningAsAdministrator())
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = Process.GetCurrentProcess().MainModule.FileName,
                    UseShellExecute = true,
                    Verb = "runas" // This triggers the UAC prompt
                };

                try
                {
                    Process.Start(processInfo);
                    Application.Current.Shutdown();
                }
                catch (Exception)
                {
                    MessageBox.Show("Administrator privileges are required to close the application.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private bool IsRunningAsAdministrator()
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }
    }
}

using CodingTest.Application.Abstractions.Services;
using CodingTest.Domain.Entities;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CodingTest.App
{
    public partial class MainWindow : Window
    {
        private readonly IFileMonitorService _fileMonitorService;
        private readonly IFileExporter _fileExporter;

        public ObservableCollection<TradeData> Trades { get; set; } = new();
        private bool isMonitoring;

        public MainWindow(IFileMonitorService fileMonitorService, IFileExporter fileExporter)
        {
            InitializeComponent();
            _fileMonitorService = fileMonitorService;
            _fileExporter = fileExporter;
            DataContext = this;
        }
        private void OnDataLoaded(List<TradeData> newTrades)
        {
            Dispatcher.Invoke(() =>
            {
                foreach (var trade in newTrades)
                {
                    Trades.Add(trade);
                }
            });
        }
        private void BrowseFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                DirectoryPathInput.Text = dialog.FileName;
        }
        private async void StartMonitoring_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(MonitoringIntervalInput.Text, out int interval) || interval <= 0)
            {
                MessageBox.Show("Invalid interval value!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string directoryPath = DirectoryPathInput.Text;
            if (!Directory.Exists(directoryPath))
            {
                MessageBox.Show("Selected directory does not exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (isMonitoring)
            {
                MessageBox.Show("Monitoring is already running.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            isMonitoring = true;
            // Start the monitoring process asynchronously
            _ = _fileMonitorService.MonitorFilesAsync(interval, directoryPath, OnDataLoaded);
            MessageBox.Show("Monitoring started.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void StopMonitoring_Click(object sender, RoutedEventArgs e)
        {
            if (isMonitoring)
            {
                _fileMonitorService.StopMonitoring();
                isMonitoring = false;
                MessageBox.Show("Monitoring stopped.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Monitoring is not running.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ClearList_Click(object sender, RoutedEventArgs e)
        {
            Trades.Clear();
        }
        private void BrowseExportFolder_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = folderDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
                ExportDirectoryPathInput.Text = folderDialog.SelectedPath;
        }
        private async void ExportData_Click(object sender, RoutedEventArgs e)
        {
            string exportDirectory = ExportDirectoryPathInput.Text;

            if (string.IsNullOrEmpty(exportDirectory) || !Directory.Exists(exportDirectory))
            {
                MessageBox.Show("Invalid export directory path", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // We set the format based on the button clicked format
            string format = (sender as Button).Content.ToString().Split(' ')[1].ToLower();

            try
            {
                await _fileExporter.ExportDataToFileAsync(Trades.ToList(), exportDirectory, format);

                MessageBox.Show($"{format.ToUpper()} file exported successfully.", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting file: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

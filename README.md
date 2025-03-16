📌 Project Description
This is a C# WPF application designed to monitor a specified directory for new files (CSV, XML, TXT) at a configurable interval. When new files appear, the application loads and processes them in parallel, displaying the results in the GUI without blocking the main thread.

🚀 Features
Directory Monitoring: Monitors a user-defined directory at a configurable frequency.
Multi-Format File Support: Loads and processes CSV, XML, and TXT files.
Plugin-Based Loaders: Easily extendable with additional file format support.
Asynchronous Processing: Ensures smooth UI performance without freezing.
Configurable Settings: Directory path and monitoring frequency are adjustable via configuration.
🏗 Project Structure
FileMonitoringService: Watches the directory and detects new files.
FileLoaders: Separate loaders for CSV, XML, and TXT files.
MainWindow.xaml: WPF UI to display the processed data.
App.config: Contains configurable settings (monitoring interval, directory path, enabled loaders).
⚙ Technologies Used
C# (.NET 6)
WPF (Windows Presentation Foundation)
Task Parallel Library (TPL) for async processing
📂 Setup & Usage
Clone the repository:
sh
Copy
Edit
git clone https://github.com/yourusername/FileMonitoringApp.git
Open the solution in Visual Studio.
Modify App.config to set your preferred directory path and monitoring interval.
Run the application.


using System.Configuration;
using System.Drawing.Printing;
using System.Linq;
using System.Windows;

using IssuePrinter.Core;

namespace IssuePrinter.Wpf
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ConfigureJiraSettings();
            ConfigurePrinterInputBox();
        }

        private void ConfigureJiraSettings()
        {
            HostInputBox.Text = ConfigurationManager.AppSettings["JiraHost"];
            UsernameInputBox.Text = ConfigurationManager.AppSettings["JiraUsername"];
            PasswordInputBox.Password = ConfigurationManager.AppSettings["JiraPassword"];

            IssueInputBox.Text = ConfigurationManager.AppSettings["JiraDefaultIssueNumber"];
        }

        private void ConfigurePrinterInputBox()
        {
            foreach(string printerName in PrinterSettings.InstalledPrinters)
            {
                PrinterInputBox.Items.Add(printerName);
            }
            PrinterInputBox.SelectedItem = PrinterInputBox.Items.OfType<string>().Last();
        }

        private void PrintIssueButton_Click(object sender, RoutedEventArgs e)
        {
            var printService = CreatePrintService();
            printService.PrintIssue(IssueInputBox.Text);
        }

        private void PrintSprintButton_Click(object sender, RoutedEventArgs e)
        {
            var printService = CreatePrintService();
            printService.PrintSprintIssues(SprintInputBox.Text);
        }

        private PrintService CreatePrintService()
        {
            var ticketPrintServiceConfig =
                new TicketPrintServiceConfig
                {
                    JiraHost = HostInputBox.Text,
                    JiraUsername = UsernameInputBox.Text,
                    JiraPassword = PasswordInputBox.Password,
                    PrinterName = (string)PrinterInputBox.SelectedItem
                };

            return new PrintService(ticketPrintServiceConfig);
        }
    }
}
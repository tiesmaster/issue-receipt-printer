using System.Configuration;
using System.Drawing.Printing;
using System.Linq;

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

        private void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var ticketPrintServiceConfig = new TicketPrintServiceConfig
            {
                JiraHost = HostInputBox.Text,
                JiraUsername = UsernameInputBox.Text,
                JiraPassword = PasswordInputBox.Password,
                PrinterName = (string)PrinterInputBox.SelectedItem,
            };

            var printService = new PrintService(ticketPrintServiceConfig);
            printService.PrintIssue(IssueInputBox.Text);
        }
    }
}
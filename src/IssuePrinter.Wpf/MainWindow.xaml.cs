using System.Drawing.Printing;
using System.Linq;

namespace IssuePrinter.Wpf
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ConfigurePrinterInputBox();
        }

        private void ConfigurePrinterInputBox()
        {
            foreach(string printerName in PrinterSettings.InstalledPrinters)
            {
                PrinterInputBox.Items.Add(printerName);
            }
            PrinterInputBox.SelectedItem = PrinterInputBox.Items.OfType<string>().Last();
        }
    }
}
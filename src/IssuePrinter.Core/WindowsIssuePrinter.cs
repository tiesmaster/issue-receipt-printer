using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;

using IssuePrinter.Core.Models;
using IssuePrinter.Core.Resources;

namespace IssuePrinter.Core
{
    public interface IIssuePrinter
    {
        void PrintIssue(IssueCard issue);
        void PrintIssues(IEnumerable<IssueCard> issues);
    }

    public class WindowsIssuePrinter : IIssuePrinter
    {
        private readonly string _printerName;

        public WindowsIssuePrinter(string printerName)
        {
            _printerName = printerName;
        }

        public void PrintIssue(IssueCard issue)
        {
            if (issue == null) return;
            PrintIssues(new[] { issue });
        }

        public void PrintIssues(IEnumerable<IssueCard> issues)
        {
            if (issues == null) return;

            var pendingIssues = new Queue<IssueCard>(issues);
            var printDocument = NewPrinterDocument(_printerName);
            PrintQueuedIssuesToDocument(pendingIssues, printDocument);
        }

        private static void PrintQueuedIssuesToDocument(Queue<IssueCard> pendingIssues, PrintDocument printDocument)
        {
            printDocument.PrintPage += (_, e) =>
            {
                if (pendingIssues.Count > 0)
                {
                    var issue = pendingIssues.Dequeue();
                    PrintIssueToGraphics(issue, e.Graphics);
                }
                e.HasMorePages = pendingIssues.Count > 0;
            };
            printDocument.Print();
        }

        private static void PrintIssueToGraphics(IssueCard issue, Graphics outputDevice)
        {
            var printer = new GraphicsIssuePrinter(issue, outputDevice);
            printer.PrintIssue();
        }

        private static PrintDocument NewPrinterDocument(string printerName)
        {
            var pd = new PrintDocument
            {
                PrinterSettings =
                {
                    PrinterName = printerName
                },
                DefaultPageSettings =
                {
                    Landscape = true,
                    Margins = new Margins(0,0,0,0)
                }
            };

            return pd;
        }

        private class GraphicsIssuePrinter
        {
            private readonly IssueCard _issue;
            private readonly Graphics _outputDevice;

            public GraphicsIssuePrinter(IssueCard issue, Graphics outputDevice)
            {
                _issue = issue;
                _outputDevice = outputDevice;
            }

            internal void PrintIssue()
            {
                PrintKey();
                PrintSeparator(70, 400);
                PrintSummary();
                PrintPriority();
                PrintRank();
                PrintStoryPoints();
                PrintType();
            }

            private void PrintKey()
            {
                var titleFont = new Font(FontFamily.GenericSansSerif, 36);
                _outputDevice.DrawString(_issue.Key, titleFont, Brushes.Black, 0, 10);
            }

            private void PrintSeparator(int yOffset, int lenght)
            {
                var boldBlackPen = new Pen(Color.Black, 3);

                _outputDevice.DrawLine(boldBlackPen, 0, yOffset, lenght, yOffset);
            }

            private void PrintSummary()
            {
                var summaryFont = new Font(FontFamily.GenericSansSerif, 16);
                var lineHeight = summaryFont.GetHeight(_outputDevice);
                float offsetY = 80;

                var wrappedSummary = _issue.GetSummaryWrapped(35);
                foreach (var line in wrappedSummary)
                {
                    _outputDevice.DrawString(line, summaryFont, Brushes.Black, 0, offsetY);
                    offsetY += lineHeight;
                }
            }

            private void PrintPriority()
            {
                var priorityIcon = PriorityToImage(_issue.Priority);
                _outputDevice.DrawImage(priorityIcon, 350, 20);
            }

            private Image PriorityToImage(Priority priority)
            {
                switch (priority)
                {
                    case Priority.Minor:
                        return Icons.minor;
                    case Priority.Major:
                        return Icons.major;
                    case Priority.Critical:
                        return Icons.critical;
                    case Priority.Blocker:
                        return Icons.blocker;
                    default:
                        throw new ArgumentException();
                }
            }

            private void PrintRank()
            {
                var font = new Font(FontFamily.GenericSansSerif, 12);
                _outputDevice.DrawString("#" + _issue.Rank, font, Brushes.Black, 0, 250);
            }

            private void PrintStoryPoints()
            {
                var titleFont = new Font(FontFamily.GenericSansSerif, 12);
                _outputDevice.DrawString("SP " + _issue.StoryPoints, titleFont, Brushes.Black, 350, 250);
            }

            private void PrintType()
            {
                var titleFont = new Font(FontFamily.GenericSansSerif, 12);
                _outputDevice.DrawString(_issue.Type, titleFont, Brushes.Black, 175, 250);
            }
        }
    }
}
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
        void PrintIssue(IssueCard issueCard);
        void PrintIssues(IEnumerable<IssueCard> issues);
    }

    public class WindowsIssuePrinter : IIssuePrinter
    {
        private readonly PrintDocument _printDocument;
        private readonly Queue<IssueCard> _pendingIssues;

        public WindowsIssuePrinter(string printerName)
        {
            _printDocument = NewPrinterDocument(printerName);
            _printDocument.PrintPage += HandleNextPage;
            _pendingIssues = new Queue<IssueCard>();
        }

        public void PrintIssue(IssueCard issueCard)
        {
            if (issueCard == null) return;

            _pendingIssues.Enqueue(issueCard);
            _printDocument.Print();
        }

        public void PrintIssues(IEnumerable<IssueCard> issues)
        {
            if (issues == null) return;

            foreach (var issue in issues)
            {
                _pendingIssues.Enqueue(issue);
            }

            _printDocument.Print();
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

        private void HandleNextPage(object sender, PrintPageEventArgs ev)
        {
            if (_pendingIssues.Count > 0)
            {
                var issue = _pendingIssues.Dequeue();
                var printer = new GraphicsIssuePrinter(issue, ev.Graphics);
                printer.PrintIssue();
            }

            ev.HasMorePages = _pendingIssues.Count > 0;    
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
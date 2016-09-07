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
                PrintIssue(ev.Graphics, issue);
            }

            ev.HasMorePages = _pendingIssues.Count > 0;    
        }

        private static void PrintIssue(Graphics outputDevice, IssueCard issue)
        {
            PrintKey(outputDevice, issue);
            PrintSeparator(outputDevice, 70, 400);
            PrintSummary(outputDevice, issue);
            PrintPriority(outputDevice, issue);
            PrintRank(outputDevice, issue);
            PrintStoryPoints(outputDevice, issue);
            PrintType(outputDevice, issue);
        }

        private static void PrintKey(Graphics outputDevice, IssueCard issueCard)
        {
            var titleFont = new Font(FontFamily.GenericSansSerif, 36);
            outputDevice.DrawString(issueCard.Key, titleFont, Brushes.Black, 0, 10);      
        }

        private static void PrintSeparator(Graphics outputDevice, int yOffset, int lenght)
        {
            var boldBlackPen = new Pen(Color.Black, 3);

            outputDevice.DrawLine(boldBlackPen, 0, yOffset, lenght, yOffset);
        }

        private static void PrintSummary(Graphics outputDevice, IssueCard issueCard)
        {
            var summaryFont = new Font(FontFamily.GenericSansSerif, 16);
            var lineHeight = summaryFont.GetHeight(outputDevice);
            float offsetY = 80;

            var wrappedSummary = issueCard.GetSummaryWrapped(35);
            foreach (var line in wrappedSummary)
            {
                outputDevice.DrawString(line, summaryFont, Brushes.Black, 0, offsetY);
                offsetY += lineHeight;
            }            
        }
        
        private static void PrintPriority(Graphics outputDevice, IssueCard issue)
        {
            var priorityIcon = PriorityToImage(issue.Priority);
            outputDevice.DrawImage(priorityIcon,350,20);
        }

        private static Image PriorityToImage(Priority priority)
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

        private static void PrintRank(Graphics outputDevice, IssueCard issue)
        {
            var font = new Font(FontFamily.GenericSansSerif, 12);
            outputDevice.DrawString("#"+issue.Rank, font, Brushes.Black, 0, 250);  
        }

        private static void PrintStoryPoints(Graphics outputDevice, IssueCard issue)
        {
            var titleFont = new Font(FontFamily.GenericSansSerif, 12);
            outputDevice.DrawString("SP " + issue.StoryPoints, titleFont, Brushes.Black, 350, 250);
        }

        private static void PrintType(Graphics outputDevice, IssueCard issue)
        {
            var titleFont = new Font(FontFamily.GenericSansSerif, 12);
            outputDevice.DrawString(issue.Type, titleFont, Brushes.Black, 175, 250);
        }
    }
}
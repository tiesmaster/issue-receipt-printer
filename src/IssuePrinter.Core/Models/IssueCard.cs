using System.Collections.Generic;

namespace IssuePrinter.Core.Models
{
    public class IssueCard
    {
        public string Key { get; set; }
        public string Summary { get; set; }
        public Priority Priority { get; set; }
        public string Type { get; set; }
        public string Rank { get; set; }
        public string StoryPoints { get; set; }

        public IEnumerable<string> GetSummaryWrapped(int maxLengthPerLine)
        {
            var text = Summary;

            // Return empty list of strings if the text was empty
            if (text.Length == 0) return new List<string>();

            var words = text.Split(' ');
            var lines = new List<string>();
            var currentLine = "";

            foreach (var currentWord in words)
            {

                if ((currentLine.Length > maxLengthPerLine) ||
                    ((currentLine.Length + currentWord.Length) > maxLengthPerLine))
                {
                    lines.Add(currentLine);
                    currentLine = "";
                }

                if (currentLine.Length > 0)
                    currentLine += " " + currentWord;
                else
                    currentLine += currentWord;
            }

            if (currentLine.Length > 0)
                lines.Add(currentLine);

            return lines;
        }
    }

    public enum Priority
    {
        Minor = 4,
        Major = 3,
        Critical = 2,
        Blocker = 1
    }
}
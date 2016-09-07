using System;
using System.Collections.Generic;

using IssuePrinter.Core;
using IssuePrinter.Core.Models;

namespace IssuePrinter.Web.Test
{
    internal class TestJiraClient : IProjectManagementServiceClient
    {
        public IssueCard GetIssue(string key)
        {
            return new IssueCard
            {
                Key = "FIN-1234",
                Rank = "3",
                Priority = Priority.Major,
                StoryPoints = "5",
                Summary = "This is a test issue",
                Type = "Hoi"
            };
        }

        public IEnumerable<IssueCard> GetIssuesForSprint(string sprintId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IssueCard> GetIssuesFromQueryLanguage(string jql)
        {
            throw new NotImplementedException();
        }
    }
}
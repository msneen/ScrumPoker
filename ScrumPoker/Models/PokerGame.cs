using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumPoker.Models
{
    public class PokerGame
    {
        public TaskEstimate UserEstimate { get; set; }
        public List<TaskEstimate> Votes { get; set; }
        public UserProfile UserProfile { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
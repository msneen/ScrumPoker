using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumPoker.Models
{
    public class PokerGame
    {
        public List<TaskEstimate> Votes { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
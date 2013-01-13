using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumPoker.ViewModels.Poker
{
    public class PokerVm
    {
        public List<string> Colors { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string FirstName { get; set; }

        public List<Project> Projects { get; set; }        
    }
}
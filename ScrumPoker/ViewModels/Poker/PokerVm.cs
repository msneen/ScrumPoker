using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumPoker.ViewModels.Poker
{
    public class PokerVm
    {
        public int ProjectId { get; set; }

        public List<Project> Projects { get; set; }        
    }
}
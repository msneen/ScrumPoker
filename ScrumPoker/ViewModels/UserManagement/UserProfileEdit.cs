using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScrumPoker.Models;

namespace ScrumPoker.ViewModels.UserManagement
{
    public class UserProfileEdit
    {
        public UserProfile UserProfile { get; set; }

        public List<ScrumPoker.Role> Roles { get; set; }
    }
}
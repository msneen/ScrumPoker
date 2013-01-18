using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace ScrumPoker.Hubs
{
    //[HubName("notifier")]
    public class Notifier : Hub
    {
        public void Notify(string message, string groupName)
        {
            //for some reason, the groups are not working, should be able to do Clients.OthersInGroup(groupName).addNotification(message, groupName);
            
            Clients.Others.addNotification(message, groupName);
        }

        public Task Join(string groupName)
        {
            return Groups.Add(Context.ConnectionId, groupName);
        }
    }
}
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnDemandService.Hubs
{
    public class OrderHub : Hub
    {
        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<OrderHub>();
            context.Clients.All.displayStatus();
        }
    }
}
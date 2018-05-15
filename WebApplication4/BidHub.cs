using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using WebApplication4.Models;

namespace WebApplication4
{
    [HubName("LiveBid")]
    public class BidHub : Hub
    {
        private static int bidprice =0;
        public void AddBid()
        {
            bidprice += 1; 

            this.Clients.All.onBidRecorded(bidprice);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            bidprice -= 1;
            this.Clients.All.onBidRecorded(bidprice);
            return base.OnDisconnected(stopCalled);
        }
    }
}
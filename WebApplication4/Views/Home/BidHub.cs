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
        
        private static double bidprice = 0;
        public void AddBid(Item_table newbid)
        {
            bidprice = newbid.prod_cbid;     

            this.Clients.All.onBidRecorded(bidprice);
        }
        
    }
}
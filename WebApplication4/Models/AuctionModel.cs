using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class AuctionModel
    {
        public IEnumerable<Item_table> item_table { get; set; }
        public double? newbid { get; set; }
        public int? Id { get; set; }

    }
}
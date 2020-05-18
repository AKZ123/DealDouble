using DealDouble.Data;
using DealDouble.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeakDouble.Services
{
    public class BidsServices
    {
        public bool AddBid(Bid bid)
        {
            DealDoubleContext context = new DealDoubleContext();
            context.Bids.Add(bid);

            return context.SaveChanges() > 0 ;
        }
    }
}

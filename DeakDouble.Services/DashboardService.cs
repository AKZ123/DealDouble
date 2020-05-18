using DealDouble.Data;
using DealDouble.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeakDouble.Services
{
    public class DashboardService
    {
        DealDoubleContext context = new DealDoubleContext();

        public int GetUserCount()
        {
            return context.Users.Count();
        }

        public int GetAuctionCount()
        {
            return context.Auctions.Count();
        }

        public int GetBidsCount()
        {
            return context.Bids.Count();
        }

        public List<Comment> GetCommentsByUser(string userId, int entityID)
        {
            return context.Comments.Where(x => x.UserID == userId).Where(x=>x.EntityID==entityID).OrderByDescending(x=> x.TimeStamp).ToList();
        }
    }
}

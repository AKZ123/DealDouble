using DealDouble.Data;
using DealDouble.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeakDouble.Services
{
    public class AuctionsServices
    {
        public List<Auction> GetAllAuction()
        {
            DealDoubleContext context = new DealDoubleContext();
            return context.Auctions.ToList();
        }

        public List<Auction> GetPromotedAuction()
        {
            DealDoubleContext context = new DealDoubleContext();
            return context.Auctions.Take(4).ToList();
        }

        public List<Auction> SearchAuctions(int? categoryID, string searchTerm, int? pageNo, int pageSize)
        {
            DealDoubleContext context = new DealDoubleContext();

            var auctions = context.Auctions.AsQueryable();

            if (categoryID.HasValue && categoryID.Value>0)
            {
                auctions = auctions.Where(x => x.CategoryID == categoryID.Value);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                auctions = auctions.Where(x => x.Title.ToLower().Contains(searchTerm.ToLower()));
            }

            pageNo = pageNo ?? 1;
            //pageNo = pageNo.HasValue ? pageNo.Value : 1;

            var skipCount = (pageNo.Value - 1) * pageSize;
            return auctions.OrderByDescending(x=>x.CategoryID).Skip(skipCount).Take(pageSize).ToList();
        }

        public int GetAuctionCount(int? categoryID, string searchTerm)
        {
            DealDoubleContext context = new DealDoubleContext();

            var auctions = context.Auctions.AsQueryable();

            if (categoryID.HasValue && categoryID.Value > 0)
            {
                auctions = auctions.Where(x => x.CategoryID == categoryID.Value);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                auctions = auctions.Where(x => x.Title.ToLower().Contains(searchTerm.ToLower()));
            }
            
            //.OrderByDescending(x => x.CategoryID)
            return auctions.Count();
        }


        public Auction GetAuctionbyID(int ID)
        {
            DealDoubleContext context = new DealDoubleContext();
            return context.Auctions.Find(ID);
        }

        public List<Auction> GetAuctionsbyIDs(List<int> IDs)
        {
            DealDoubleContext context = new DealDoubleContext();
            return IDs.Select(id => context.Auctions.Find(id)).ToList();
        }

        public void SaveAuction(Auction auction)
        {
            DealDoubleContext context = new DealDoubleContext();
            context.Auctions.Add(auction);
            context.SaveChanges();
        }

        public void UpdateAuction(Auction auction)
        {
            DealDoubleContext context = new DealDoubleContext();

            var existingAuction = context.Auctions.Find(auction.ID);               //first Take Auction from DB.

            context.AuctionPictures.RemoveRange(existingAuction.AuctionPictures);  //second Remove all AuctionPictures table 1 to * relational data of The auction.

            context.Entry(existingAuction).CurrentValues.SetValues(auction);       //Set new data on first selected auction data.

            context.AuctionPictures.AddRange(auction.AuctionPictures);             //Add all new 1 to * relational data of upadted Auction on AuctionPictures table.
            //context.Entry(auction).State=System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteAuction(Auction auction)
        {
            DealDoubleContext context = new DealDoubleContext();
            context.Entry(auction).State = System.Data.Entity.EntityState.Deleted;
            //context.Auctions.Remove(auction);
            context.SaveChanges();
        }
    }
}

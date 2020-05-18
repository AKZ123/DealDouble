using DealDouble.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DealDouble.Web.ViewModels
{
    public class AuctionsListingViewModel : PageViewModel
    {
        public List<Auction> Auctions { get; set; }
        public int? CategoryID { get; set; }
        public string SearchTerm { get; set; }

        public Pager Pager { get; set; }
        public int? PageNo { get; internal set; }
        public List<Category> Categories { get; set; }
    }

    public class AuctionsViewModel : PageViewModel
    {
        public List<Auction> AllAuction { get; set; }
        public List<Auction> PromotedAuction { get; set; }
    }

    public class CreateAuctionViewModel : PageViewModel
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(150)]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        [Range(1, 1000000, ErrorMessage = "Actual ammount must be 1 to 1000000")]
        public decimal ActualAmount { get; set; }
        public DateTime? StartingTime { get; set; }
        public DateTime? EndingTime { get; set; }
        public string auctionPictures { get; set; }

        public List<Category> Categories { get; set; }
        public List<AuctionPicture> auctionPicturesList { get; set; }
    }

    public class AuctionDetailViewModel : PageViewModel
    {
        public Auction Auction { get; set; }

        public List<Comment> Comments { get; set; }
        public int EntityID { get; set; }

        public decimal BidsAmount { get; set; }

        public DealDoubleUser LatestBidder { get; set; }

    }
}
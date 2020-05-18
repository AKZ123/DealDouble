using DeakDouble.Services;
using DealDouble.Entities;
using DealDouble.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DealDouble.Web.Controllers
{
    public class AuctionsController : Controller
    {
        AuctionsServices AuctionsServices = new AuctionsServices();
        CategoriesServices categoriesService = new CategoriesServices();
        SharedService sharedService = new SharedService();

        // GET: Auctions

        public ActionResult Index(int? categoryID, string searchTerm, int? pageNo)
        {
            AuctionsListingViewModel model = new AuctionsListingViewModel();

            model.PageTitel = "Auction Index";
            model.PageDescription = "This is Auction Index";

            model.CategoryID = categoryID;
            model.SearchTerm = searchTerm;
            model.PageNo = pageNo;

            model.Categories = categoriesService.GetAllCatrgories();

            return PartialView(model);
        }

        
        public ActionResult Listing(int? categoryID, string searchTerm, int? pageNo)
        {
            var pageSize = 5;
            AuctionsListingViewModel model = new AuctionsListingViewModel();

            //model.Auctions = AuctionsServices.GetAllAuction();
            model.Auctions = AuctionsServices.SearchAuctions(categoryID, searchTerm, pageNo, pageSize);

            var totalAuctions = AuctionsServices.GetAuctionCount(categoryID, searchTerm);

            model.Pager = new Pager(totalAuctions, pageNo, pageSize);
            //model.PageNo = pageNo ?? 1;

            return PartialView(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            CreateAuctionViewModel model = new CreateAuctionViewModel();
            model.Categories = categoriesService.GetAllCatrgories();

            return PartialView(model);
        }
        [HttpPost]
        public JsonResult Create(CreateAuctionViewModel model)
        {
            JsonResult result = new JsonResult();

            if (ModelState.IsValid)
            {
                Auction auction = new Auction();
                auction.CategoryID = model.CategoryID;
                auction.Title = model.Title;
                auction.Description = model.Description;
                auction.ActualAmount = model.ActualAmount;
                auction.StartingTime = model.StartingTime;
                auction.EndingTime = model.EndingTime;

                if (!string.IsNullOrEmpty(model.auctionPictures))
                {
                    var pictureIDs = model.auctionPictures.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(ID => int.Parse(ID)).ToList();
                    auction.AuctionPictures = new List<AuctionPicture>();
                    auction.AuctionPictures.AddRange(pictureIDs.Select(x => new AuctionPicture() { PictureID = x }).ToList());
                }
                //foreach (var picID in pictureIDs)
                //{
                //    var auctionPicture = new AuctionPicture();
                //    auctionPicture.PictureID = picID;

                //    auction.AuctionPictures.Add(auctionPicture);
                //}

                AuctionsServices.SaveAuction(auction);
                //return RedirectToAction("Listing");
                result.Data = new { Success = true };
            }
            else
            {
                result.Data = new { Success = false,Error="Unable to save Auction.Please inter valid values" };
                //return PartialView(model);
            }

            return result;
        }

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            CreateAuctionViewModel model = new CreateAuctionViewModel();

            var auction = AuctionsServices.GetAuctionbyID(ID);

            model.ID = auction.ID;
            model.Title = auction.Title;
            model.CategoryID = auction.CategoryID;
            model.Description = auction.Description;
            model.ActualAmount = auction.ActualAmount;
            model.StartingTime = auction.StartingTime;
            model.EndingTime = auction.EndingTime;

            model.Categories = categoriesService.GetAllCatrgories();
            model.auctionPicturesList = auction.AuctionPictures;

            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Edit(CreateAuctionViewModel model)
        {
            Auction auction = new Auction();
            auction.ID = model.ID;
            auction.CategoryID = model.CategoryID;
            auction.Title = model.Title;
            auction.Description = model.Description;
            auction.ActualAmount = model.ActualAmount;
            auction.StartingTime = model.StartingTime;
            auction.EndingTime = model.EndingTime;

            if (!string.IsNullOrEmpty(model.auctionPictures))
            {
                var pictureIDs = model.auctionPictures.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(ID => int.Parse(ID)).ToList();
                auction.AuctionPictures = new List<AuctionPicture>();
                auction.AuctionPictures.AddRange(pictureIDs.Select(x => new AuctionPicture() {AuctionID=auction.ID, PictureID = x }).ToList());
            }
            
            AuctionsServices.UpdateAuction(auction);
            return RedirectToAction("Listing");
        }

        //[HttpGet]
        //public ActionResult Delete(int ID)
        //{

        //    var auction = service.GetAuctionbyID(ID);
        //    return View(auction);
        //}
        [HttpPost]
        public ActionResult Delete(Auction auction)
        {

            AuctionsServices.DeleteAuction(auction);
            return RedirectToAction("Listing");
        }

        [HttpGet]
        public ActionResult Details(int ID)
        {
            AuctionDetailViewModel model = new AuctionDetailViewModel();
            model.Auction = AuctionsServices.GetAuctionbyID(ID);

            model.BidsAmount = model.Auction.ActualAmount + model.Auction.Bids.Sum(x => x.BidAmount);

            var latestBider =  model.Auction.Bids.OrderByDescending(x => x.Timestamp).FirstOrDefault();
            
            model.LatestBidder = latestBider != null ? latestBider.User : null;

            //model.Comments = sharedService.GetComments(12, model.Auction.ID);
            model.Comments = sharedService.GetComments((int)EntityEnums.Auction, model.Auction.ID);

            model.EntityID = (int)EntityEnums.Auction;
            model.PageTitel = "Auction Index"+model.Auction.Title;
            model.PageDescription = model.Auction.Description.Substring(0,10);
            return View(model);
        }
    }
}
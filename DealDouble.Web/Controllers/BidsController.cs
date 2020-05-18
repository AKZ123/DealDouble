using DeakDouble.Services;
using DealDouble.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DealDouble.Web.Controllers
{
    public class BidsController : Controller
    {
        BidsServices service = new BidsServices();

        [HttpPost]
        public ActionResult Bid(int ID)
        {
            JsonResult result = new JsonResult();

            if (User.Identity.IsAuthenticated)
            {
                Bid bid = new Bid();

                bid.AuctionID = ID;
                bid.UserID = User.Identity.GetUserId();
                bid.Timestamp = DateTime.Now;
                bid.BidAmount = 10;

                var bidResult= service.AddBid(bid);

                if (bidResult)
                {
                    result.Data = new { Success = true };
                }
                else
                {
                    result.Data = new { Success = false, Message="Unable to add bid to Auction." };
                }
            }
            else
            {
                result.Data = new { Success = false, Message = "User need to Log in for Bid." };
            }

            //return View();
            return result;
        }
    }
}
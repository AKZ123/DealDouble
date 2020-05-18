using DeakDouble.Services;
using DealDouble.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DealDouble.Web.Controllers
{
    public class HomeController : Controller
    {
        AuctionsServices service = new AuctionsServices();

        public ActionResult Index()
        {
            AuctionsViewModel vModel = new AuctionsViewModel();

            vModel.PageTitel = "Home Page";
            vModel.PageDescription = "This is Home Page";
            vModel.AllAuction = service.GetAllAuction();
            vModel.PromotedAuction = service.GetPromotedAuction();

            return View(vModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
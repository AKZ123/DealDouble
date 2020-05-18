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
    public class CategoriesController : Controller
    {
        CategoriesServices categoriesService = new CategoriesServices();

        // GET: Categories
        [HttpGet]
        public ActionResult Index()
        {
            CategoriesListingViewModel model = new CategoriesListingViewModel();

            model.PageTitel = "Categories Index";
            model.PageDescription = "This is Categories Index";

            return View(model);
        }


        public ActionResult Listing()
        {
            CategoriesListingViewModel model = new CategoriesListingViewModel();

            model.Categories = categoriesService.GetAllCatrgories();

            return PartialView(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            //CreateCategoryViewModel model = new CreateCategoryViewModel();
            //return PartialView(model);
            return PartialView();

        }
        [HttpPost]
        public ActionResult Create(CreateCategoryViewModel model)
        {
            Category category = new Category();
            category.Name = model.Name;
            category.Description = model.Description;

            categoriesService.SaveCategory(category);
            return RedirectToAction("Listing");
        }

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            CreateCategoryViewModel model = new CreateCategoryViewModel();

            var category = categoriesService.GetCategorybyID(ID);

            model.ID = category.ID;
            model.Name = category.Name;
            model.Description = category.Description;

            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Edit(CreateCategoryViewModel model)
        {
            Category category = new Category();
            category.ID = model.ID;
            category.Name= model.Name;
            category.Description = model.Description;

            categoriesService.UpdateCategory(category);
            return RedirectToAction("Listing");
        }

        ////[HttpGet]
        ////public ActionResult Delete(int ID)
        ////{

        ////    var auction = service.GetAuctionbyID(ID);
        ////    return View(auction);
        ////}
        [HttpPost]
        public ActionResult Delete(Category category)
        {
            categoriesService.DeleteCategory(category);
            return RedirectToAction("Listing");
        }

        //[HttpGet]
        //public ActionResult Details(int ID)
        //{
        //    AuctionDetailViewModel model = new AuctionDetailViewModel();
        //    model.Auction = AuctionsServices.GetAuctionbyID(ID);

        //    model.PageTitel = "Auction Index" + model.Auction.Title;
        //    model.PageDescription = model.Auction.Description.Substring(0, 10);
        //    return View(model);
        //}
    }
}
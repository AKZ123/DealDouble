using DealDouble.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealDouble.Web.ViewModels
{
    public class CategoriesListingViewModel : PageViewModel
    {
        public List<Category> Categories { get; set; }

    }

    public class CategoriesViewModel : PageViewModel
    {
        public List<Category> AllCategories { get; set; }
    }

    public class CreateCategoryViewModel : PageViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Auction> Auction { get; set; }
    }

    public class CategoryDetailViewModel : PageViewModel
    {
        public Category Category { get; set; }
        
    }
}
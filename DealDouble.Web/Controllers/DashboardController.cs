using DeakDouble.Services;
using DealDouble.Entities;
using DealDouble.Web.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DealDouble.Web.Controllers
{
    public class DashboardController : Controller
    {
        DashboardService service = new DashboardService();
        AuctionsServices auctionsServices = new AuctionsServices();

        private DealDoubleUserManager _userManager;
        private DealDoubleRoleManager _roleManager;


        public DashboardController()
        {
        }

        public DashboardController(DealDoubleUserManager userManager, DealDoubleRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public DealDoubleUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<DealDoubleUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public DealDoubleRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<DealDoubleRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel();

            model.UserCount = service.GetUserCount();
            model.AuctionCount = service.GetAuctionCount();
            model.BidsCount = service.GetBidsCount();

            return View(model);
        }

        public ActionResult Users(string roleID, string searchTerm, int? pageNo)
        {
            UserViewModel model = new UserViewModel();

            model.RoleID = roleID;
            model.SearchTerm = searchTerm;
            model.PageNo = pageNo;
            //model.Roles = new List<IdentityRole>();
            model.Roles = RoleManager.Roles.ToList();


            return View(model);
        }

        public ActionResult UsersListing(string roleID, string searchTerm, int? pageNo)
        {
            var pageSize = 5;
            UsersListingViewModel model = new UsersListingViewModel();

            model.RoleID = roleID;
            model.SearchTerm = searchTerm;
            //model.Users = UserManager.Users.ToList();

            var users = UserManager.Users;  //.AsQueryable();

            if (!string.IsNullOrEmpty(roleID))
            {
                //users = users.Where(x => x.Roles.Where(y => y.RoleId == roleID).Count()>0);
                users = users.Where(x => x.Roles.FirstOrDefault(y => y.RoleId == roleID) != null);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(x => x.Email.ToLower().Contains(searchTerm.ToLower()));
            }

            pageNo = pageNo ?? 1;
            //pageNo = pageNo.HasValue ? pageNo.Value : 1;

            var skipCount = (pageNo.Value - 1) * pageSize;
            model.Users = users.OrderByDescending(x => x.Roles.Count).Skip(skipCount).Take(pageSize).ToList();

            model.Pager = new Pager(users.Count(), pageNo, pageSize);
            //model.PageNo = pageNo ?? 1;

            return PartialView(model);
        }

        public ActionResult Role(string searchTerm, int? pageNo)
        {
            RolesViewModel model = new RolesViewModel();

            model.SearchTerm = searchTerm;
            model.PageNo = pageNo;

            return View(model);
        }

        public ActionResult RoleListing(string searchTerm, int? pageNo)
        {
            RolesListingViewModel model = new RolesListingViewModel();

            model.SearchTerm = searchTerm;
            //model.Pager.CurrentPage = pageNo ?? 1 ;
            //model.Roles = new List<IdentityRole>();
            model.Roles = RoleManager.Roles.ToList();


            return PartialView(model);
        }


        public async Task<ActionResult> userDetails(string userID, bool IsPartial = false)
        {
            UsersDetailViewModel model = new UsersDetailViewModel();

            var user = await UserManager.FindByIdAsync(userID);

            if (user != null)
            {
                model.Users = user;
            }

            if (IsPartial && Request.IsAjaxRequest())
            {
                return PartialView("_UsersDetails", model);
            }
            else
            {
                return View(model);
            }
        }

        public async Task<ActionResult> UsersRoles(string userID)
        {
            UserRolesViewModel model = new UserRolesViewModel();

            model.AvailableRoles = RoleManager.Roles.ToList();

            if (!string.IsNullOrEmpty(userID))
            {
                model.User = await UserManager.FindByIdAsync(userID);

                if (model.User != null)
                {
                    model.UserRoles = model.User.Roles.Select(userRole => model.AvailableRoles.FirstOrDefault(role => role.Id == userRole.RoleId)).ToList();
                }
            }
            return PartialView("_UsersRoles", model);
        }

        public async Task<ActionResult> AssignUserRole(string userID, string roleID)
        {
            if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(roleID))
            {
                var user = await UserManager.FindByIdAsync(userID);
                if (user != null)
                {
                    var role = await RoleManager.FindByIdAsync(roleID);
                    if (role != null)
                    {
                        await UserManager.AddToRoleAsync(user.Id, role.Name);
                    }
                }
            }
            
            return RedirectToAction("UsersRoles",new { userID = userID });
        }

        public async Task<ActionResult> DeleteUserRole(string userID, string roleID)
        {
            if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(roleID))
            {
                var user = await UserManager.FindByIdAsync(userID);
                if (user != null)
                {
                    var role = await RoleManager.FindByIdAsync(roleID);
                    if (role != null)
                    {
                        await UserManager.RemoveFromRoleAsync(user.Id, role.Name);
                    }
                }
            }

            return RedirectToAction("UsersRoles", new { userID = userID });
        }

        public async Task<ActionResult> UserComments(string userID)
        {
            UserCommentsViewModel model = new UserCommentsViewModel();

            model.User = await UserManager.FindByIdAsync(userID);

            if (!string.IsNullOrEmpty(userID))
            {
                if (model.User != null)
                {
                    model.UserComments = service.GetCommentsByUser(userID, (int)EntityEnums.Auction);

                    if (model.UserComments != null && model.UserComments.Count > 0)
                    {
                        var auctiinIDs = model.UserComments.Select(x => x.RecordID).ToList();

                        model.CommentedAuctions = auctionsServices.GetAuctionsbyIDs(auctiinIDs);
                    }

                }
            }
            return PartialView("_UserComments", model);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using DealDouble.Entities;

namespace DealDouble.Web.ViewModels
{
    public class DashboardViewModel
    {
        public int UserCount { get; set; }
        public int AuctionCount { get; set; }
        public int BidsCount { get; set; }
    }

    public class UserViewModel
    {
        public string RoleID { get; set; }
        public string SearchTerm { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public int? PageNo { get; set; }
    }

    public class UsersListingViewModel
    {
        public List<DealDoubleUser> Users { get; set; }
        public string RoleID { get; set; }
        public Pager Pager { get; set; }
        public string SearchTerm { get; set; }

        public int? PageNo { get; set; }
    }


    public class RolesViewModel
    {
        public string SearchTerm { get; set; }
        public int? PageNo { get; set; }

        //public List<IdentityRole> Roles { get; set; }
    }

    public class RolesListingViewModel
    {
        public List<IdentityRole> Roles { get; set; }
        public Pager Pager { get; set; }
        public string SearchTerm { get; set; }
    }


    public class UsersDetailViewModel : PageViewModel
    {
        public DealDoubleUser Users { get; set; }
    }


    public class UserRolesViewModel
    {
        public List<IdentityRole> AvailableRoles { get; internal set; }
        public DealDoubleUser User { get; internal set; }
        public IEnumerable<IdentityRole> UserRoles { get; internal set; }
    }

    public class UserCommentsViewModel
    {
        public List<Comment> UserComments { get; set; }
        public List<Auction> CommentedAuctions { get; set; }
        public DealDoubleUser User { get; set; }
    }
}
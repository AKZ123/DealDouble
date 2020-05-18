using DealDouble.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealDouble.Data
{
    public class DealDoubleDBInitializer : CreateDatabaseIfNotExists<DealDoubleContext>
    {
        protected override void Seed(DealDoubleContext context)
        {
            //base.Seed(context);
            SeedRoles(context);
            SeedUsers(context);
        }

        public void SeedRoles(DealDoubleContext context)
        {
            List<IdentityRole> rolesInDealDouble = new List<IdentityRole>();

            rolesInDealDouble.Add(new IdentityRole() { Name = "Administrator" });
            rolesInDealDouble.Add(new IdentityRole() { Name = "Moderator" });
            rolesInDealDouble.Add(new IdentityRole() { Name = "User" });

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            foreach (IdentityRole role in rolesInDealDouble)
            {
                if (!roleManager.RoleExists(role.Name))
                {
                    var result = roleManager.Create(role);

                    if (result.Succeeded) continue;
                }
            }
        }

        public void SeedUsers(DealDoubleContext context)
        {
            var usersStore = new UserStore<DealDoubleUser>(context);
            var usersManager = new UserManager<DealDoubleUser>(usersStore);

            DealDoubleUser admin = new DealDoubleUser();
            admin.Email = "admin@gmail.com";
            admin.UserName = "admin@gmail.com";
            var password = "Admin@12345";

            if (usersManager.FindByEmail(admin.Email)==null)
            {
                var result = usersManager.Create(admin, password);

                if (result.Succeeded)
                {
                    usersManager.AddToRole(admin.Id, "Administrator");
                    usersManager.AddToRole(admin.Id, "Moderator");
                    usersManager.AddToRole(admin.Id, "User");
                }
            }
        }

    }
}

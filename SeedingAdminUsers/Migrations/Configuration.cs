namespace SeedingAdminUsers.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.SqlClient;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SeedingAdminUsers.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SeedingAdminUsers.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // create a default user   
            var user1 = new ApplicationUser();
            user1.UserName = "abel130@gmail.com";
            user1.Email = "abel130@gmail.com";
            string userPWD = "Abel!30";

            // create an admin role 
            UserManager.Create(user1, userPWD);

            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin role  
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var role2 = new IdentityRole();
                role2.Name = "Limited";
                roleManager.Create(role2);
            }

            //Here we create a Admin  user who will maintain the website
            var adminuser = new ApplicationUser();
            adminuser.UserName = "admin@gmail.com";
            adminuser.Email = "admin@gmail.com";
            userPWD = "Admin!30";
            adminuser.IsAdmin = true;
            adminuser.FirstName = "Abel";
            adminuser.LastName = "Toth";
            var chkUser = UserManager.Create(adminuser, userPWD);

            //Add admin User to Role Admin   
            if (chkUser.Succeeded)
            {
                var result1 = UserManager.AddToRole(adminuser.Id, "admin");
            }

            // creating another role  - name it what you like   
            if (!roleManager.RoleExists("Restricted"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Restricted";
                roleManager.Create(role);

            }

            var article1 = new Article();
            article1.ArticleID = 1;
            article1.UserName = user1.UserName;
            article1.ArticleTitle = "How to be ...";
            article1.ArticleDescription = "blah blah blah";
            article1.PublishDate = DateTime.Now;
            try
            {
                //adding the fault to the database
                context.Articles.AddOrUpdate(article1);
            }
            catch (SqlException ex)
            {
                var error = ex.InnerException;
            }

            var comment1 = new Comment();
            comment1.CommentID = 1;
            comment1.ArticleID = 1;
            comment1.CommentBody = "this is a great article";
            comment1.UserName = user1.UserName;
            comment1.CommentDate = DateTime.Now;
            try
            {
                //adding the first response
                context.Comments.AddOrUpdate(comment1);
            }
            catch (SqlException ex)
            {
                var error = ex.InnerException;
            }

            var comment2 = new Comment();
            comment2.CommentID = 2;
            comment2.ArticleID = 1;
            comment2.CommentBody = "naw its no";
            comment2.UserName = user1.UserName;
            comment2.CommentDate = DateTime.Now;
            try
            {
                //adding the first response
                context.Comments.AddOrUpdate(comment2);
            }
            catch (SqlException ex)
            {
                var error = ex.InnerException;
            }
        }
    }
}

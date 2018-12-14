namespace FileProcessor.Migrations
{
    using FileProcessor.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FileProcessor.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FileProcessor.Models.ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.AddOrUpdate
               (
               rl => rl.Name,
               new IdentityRole { Name = "Admin" },
               new IdentityRole { Name = "User" }
               );
                context.SaveChanges();
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var passwordHash = new PasswordHasher();

            if (!context.Users.Any(n => n.FullName == "NT"))
            {
                var admin = new ApplicationUser
                {
                    FullName = "NT",
                    UserName = "noks@test.com",
                    Email = "noks@test.com",
                    PasswordHash = passwordHash.HashPassword("Password@1")
                };
                userManager.Create(admin);
                userManager.AddToRole(admin.Id, "Admin");
                context.SaveChanges();
            }

            if (!context.Users.Any(u => u.FullName == "Noks"))
            {
                var user = new ApplicationUser
                {
                    FullName = "Noks",
                    UserName = "noks@t.com",
                    Email = "noks@t.com",
                    PasswordHash = passwordHash.HashPassword("Password@2")
                };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "User");
                context.SaveChanges();

            }
        }
    }
}

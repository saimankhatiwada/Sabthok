using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SabthokFoodModel.Utillity;
using SabthokFoodWeb.Models;

namespace SabthokFoodWeb.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }



        public void Initialize()
        {
            try
            {
                if(_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch(Exception ex)
            {

            }


            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Indi)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Comp)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUsers
                {
                    UserName = "saimankhatiwada9611@gmail.com",
                    Email = "saimankhatiwada9611@gmail.com",
                    Name = "Saiman Khatiwada",
                    PhoneNumber = "9862884166",
                    StreetAddress = "TankiSinwari,Biratnagar",
                    State = "Provience No.2",
                    PostalCode = "NULL",
                    City = "Biratnagar",
                }, "9d9dfbfbfdfd@S").GetAwaiter().GetResult();

                ApplicationUsers user = _context.ApplicationUser.FirstOrDefault(u => u.Email == "saimankhatiwada9611@gmail.com");

                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

            }
        }
    }
}

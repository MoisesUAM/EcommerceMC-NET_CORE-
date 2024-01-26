using Ecommerce.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Ecommerce.Models.Catalog;

namespace Ecommerce.DAL.InitConfiguration
{
    public class DbInitializerImpl : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public const string AdminRole = "Administrator";
        public const string ClientRole = "Client";
        public const string EmployeeRole = "Employee";

        public DbInitializerImpl(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            
        }
        public void Initialize()
        {
            try
            {
               if(_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate(); //Ejecuta migraciones pendientes
                }
            }
            catch (Exception)
            {

                throw;
            }

            //Datos iniciales
            if (_context.Roles.Any(r => r.Name == AdminRole)) return;

            _roleManager.CreateAsync(new IdentityRole(AdminRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(ClientRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(EmployeeRole)).GetAwaiter().GetResult();

            //Crea usuario admin inicial para configuracion de la aplicacion

            _userManager.CreateAsync(new UserModel{
                UserName = "moises65907@gmail.com",
                Email = "moises65907@gmail.com",
                Name = "Moises",
                LastName = "Coto Salazar",
                EmailConfirmed = true
            }, "Admin123*").GetAwaiter().GetResult();

            //Se asigna rol admin al usuario inicial

            UserModel? user = _context.UserModels.Where(u => u.UserName == "moises65907@gmail.com").FirstOrDefault();
            _userManager.AddToRoleAsync(user!, AdminRole).GetAwaiter().GetResult();
        }
    }
}

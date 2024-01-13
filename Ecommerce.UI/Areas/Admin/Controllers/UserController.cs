using Ecommerce.BLL.Notifications;
using Ecommerce.BLL.Utilities.Interfaces;
using Ecommerce.DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.AdminRole)]
    public class UserController : Controller
    {
        private readonly IUnitWork _UnitWork;
        private readonly ApplicationDbContext _dbContext;

        public UserController(IUnitWork unitWork, ApplicationDbContext dbContext)
        {
            _UnitWork = unitWork;
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var _userList = await _UnitWork.UserModelRepository.GetAll();
            var _usersRolesList = await _dbContext.UserRoles.ToListAsync();
            var _rolesList = await _dbContext.Roles.ToListAsync();

            foreach (var user in _userList) 
            {
                //Obtener la lista de roles id que estan asociados al id de user en la tabla AspNetUserRoles
                var rolesInUser = _usersRolesList.Where(u => u.UserId == user.Id).Select(ur => ur.RoleId).ToList();
                //obteener de la _rolesList los nombre de rol filtrados por el id de la lista rolesInUser de la tabla AspNetRoles 
                user.Roles = _rolesList.Where(r => rolesInUser.Contains(r.Id)).Select(r => r.Name).ToList();
            }

            return Json(new { data= _userList });

        }

        [HttpPost]
        public async Task<IActionResult> UserActionLock([FromBody] string id)
        {

            var user = await _UnitWork.UserModelRepository.GetFirst(u=>u.Id == id);
            var _message = "";
            if(user == null)
            {
                return Json(new { success=false, message="A ocurrido un error Usuario no existe" });
            }

            //Si el usuario esta bloqueado entra aca y lo desbloquea poniendo la fecha de lockoutEnd igual a la actual
            if (user.LockoutEnd != null && user.LockoutEnd> DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now;

                _message = "Se Desbloqueo exitosamente el Usuario!";
            }else
            //Si el usuario esta desbloqueado entra aca lo lo bloquea poniendo una fecha mayor en 1000 year al lockoutEnd
            {
                user.LockoutEnd = DateTime.Now.AddYears(1000);

                _message = "Se Bloqueo exitosamente el Usuario!";
                
            }

            await _UnitWork.Save();
            return Json(new { success = true, message = _message });
        }

        #endregion
    }
}
